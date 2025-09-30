using UnityEngine;
using System;
using System.IO;
using UnityEngine.Profiling;
using System.Collections.Generic;
public class FileDataHandler
{ 
    private string dataDirPath = string.Empty;

    private string dataFileName = string.Empty;

    private bool useEncryption = false;

    private readonly string encryptionCodeWord = "Halloween";

    private readonly string backupExtension = ".bak";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dataDirPath">Directory Path for game save file</param>
    /// <param name="dataFileName">Name of game save file</param>
    /// <param name="useEncryption">Encrpyt the game save file</param>
    public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
        this.useEncryption = useEncryption;
    }
    /// <summary>
    /// Load in the data from a save file.
    /// </summary>
    /// <returns></returns>
    public GameData Load(string profileID, bool allowRestorFromBackup = true)
    {
        if (profileID == null)
        {
            return null;
        }

        string fullPath = Path.Combine(dataDirPath, profileID, dataFileName);
        GameData loadedData = null;
        if(File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                if (useEncryption)
                {
                    dataToLoad = EncrpytDecrypt(dataToLoad);
                }
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                if (allowRestorFromBackup)
                {
                    Debug.LogError("Failed to load data file. Attempting to rollback.\n" + e);
                    bool rollbackSuccess = AttemptRollback(fullPath);
                    if (rollbackSuccess)
                    {
                        loadedData = Load(profileID, false);
                    }
                }
                else
                {
                    Debug.LogError("Error Occured when trying to load file at path: " + fullPath + " and backup did not work.\n" + e);
                }
            }
        }

        return loadedData;
    }
    /// <summary>
    /// Saves the game data to a file.
    /// </summary>
    /// <param name="data">Data that is being saved to a file.</param>
    public void Save(GameData data, string profileID)
    {
        if(profileID == null)
        {
            return;
        }    

        string fullPath = Path.Combine(dataDirPath, profileID, dataFileName);
        string backupFilePath = fullPath + backupExtension;

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(data, true);

            if(useEncryption)
            {
                dataToStore = EncrpytDecrypt(dataToStore);
            }

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }

            GameData verifiedGameData = Load(profileID);

            if (verifiedGameData != null)
            {
                File.Copy(fullPath, backupFilePath, true);
            }
            else
            {
                throw new Exception("Save File could not be verified and back up could not be created.");
            }
        }
        catch (Exception e) 
        {
            Debug.LogError("Error occured when saving game data to file: " + fullPath + "\n" + e);
        }
    }

    public void Delete(string profileId)
    {
        if (profileId == null)
        {
            return;
        }

        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);

        try
        {
            if (File.Exists(fullPath)) 
            {
            Directory.Delete(Path.GetDirectoryName(fullPath));

            }
            else
            {
                Debug.LogWarning("Tried to delete profile data, but data was not found at path: " + fullPath);
            }

        }
        catch (Exception e)
        {
            Debug.LogError("Failed to delete profile data from profile ID: "+ profileId+ " at path: " + fullPath + "\n" + e);
        }


    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, GameData> loadAllProfiles()
    {
        Dictionary<string,GameData> profileDictionary = new Dictionary<string,GameData>();

        IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(dataDirPath).EnumerateDirectories();

        foreach (DirectoryInfo dirInfo in dirInfos)
        {
            string profileId = dirInfo.Name;

            string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
            if(!File.Exists(fullPath))
            {
                Debug.LogWarning("Skipping directory when loading all profiles because it does not contain data: " + profileId);
                continue;
            }

            GameData profileData = Load(profileId);

            if(profileData != null)
            {
                profileDictionary.Add(profileId, profileData);
            }
            else
            {
                Debug.LogError("Tried to laod profile but something went wrong ProfileId: " + profileId);  
            }
        }

        return profileDictionary;
    }

    /// <summary>
    /// Returns the profile ID of the last saved game.
    /// </summary>
    /// <returns></returns>
    public string GetMostRecentlyUpdatedProfileID()
    {
        string mostRecentProfileID = null;
        Dictionary<string, GameData> profilesGameData = loadAllProfiles();

        foreach(KeyValuePair<string,GameData> pair in profilesGameData)
        {
            string profileID = pair.Key;
            GameData gameData = pair.Value;

            if (gameData == null)
            {
                continue;
            }
            if(mostRecentProfileID == null)
            {
                mostRecentProfileID = profileID;
            }
            else
            {
                DateTime mostRecentDateTime = DateTime.FromBinary(profilesGameData[mostRecentProfileID].lastUpdated);
                DateTime newDateTime = DateTime.FromBinary(gameData.lastUpdated);

                if(newDateTime > mostRecentDateTime)
                {
                    mostRecentProfileID = profileID;
                }
            }
        }
        return mostRecentProfileID;
    }

    /// <summary>
    /// Encrpyt and Decrypt the Save File
    /// </summary>
    /// <param name="data">Data to encrpyt or decrypt</param>
    /// <returns></returns>
    private string EncrpytDecrypt(string data)
    {
        string modifiedData = "";

        for(int i =  0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }
        return modifiedData;
    }

    private bool AttemptRollback(string fullPath)
    {
        bool success = false;

        string backupFilePath = fullPath + backupExtension;
        try
        {
            if(File.Exists(backupFilePath))
            {
                File.Copy(backupFilePath, fullPath, true);
                success = true;
                Debug.LogWarning("Had to roll back to backup file at: " + backupFilePath);   
            }
            else
            {
                throw new Exception("Tried to roll back, but no backup file exists to rollback to.");
            }
        }
        catch(Exception ex)
        {
            Debug.Log("Error occured when trying to roll back to backup file at: " + backupFilePath + '\n' + ex);
        }

        return success;
    }
}
   

