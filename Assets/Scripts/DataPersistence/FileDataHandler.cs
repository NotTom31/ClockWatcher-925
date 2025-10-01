using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
public class FileDataHandler
{
    private string dataDirPath = string.Empty;

    private string dataFileName = string.Empty;

    private bool useEncryption = false;

    private readonly string encryptionCodeWord = "Halloween";

    private readonly string backupExtension = ".bak";

    /// <summary>
    /// A FileDataHandler object parses data either from the game or a file and loads back into either a file or the game.
    /// </summary>
    /// <param name="dataDirPath">Directory path for game save file</param>
    /// <param name="dataFileName">Name of game save file</param>
    /// <param name="useEncryption">Encrpyt the game save file</param>
    public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
        this.useEncryption = useEncryption;
    }

    /// <summary>
    /// Load in the data from a saved .game file.
    /// </summary>
    /// <returns>Saved data from a file.</returns>
    public GameData Load(string profileID, bool allowRestorFromBackup = true)
    {
        //If a dataFile ID is not assigned, return null to prevent crashes.
        if (profileID == null)
        {
            return null;
        }

        //Creates the filepath to find the gamefile.
        string fullPath = Path.Combine(dataDirPath, profileID, dataFileName);
        GameData loadedData = null;

        //Make sure file exists to prevent errors.
        if (File.Exists(fullPath))
        {
            try
            {
                //Load the data into a stream then read the file and load it into the dataToLoad
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                //If the file is encrpyed, decrpyt it.
                if (useEncryption)
                {
                    dataToLoad = EncrpytDecrypt(dataToLoad);
                }

                //Deserialize the dataToLoad and load it into a GameData object.
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                //If the file does not load, grab the back up file.
                if (allowRestorFromBackup)
                {
                    Debug.LogError("Failed to load data file. Attempting to rollback.\n" + e);
                    bool rollbackSuccess = AttemptRollback(fullPath);
                    if (rollbackSuccess)
                    {
                        //If the data rollback was successful, attempt to reload the data and disallow allowRestorFromBackup to prevent looping.
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
    /// Saves the game data to a .game file.
    /// </summary>
    /// <param name="data">Data that is being saved to a file.</param>
    public void Save(GameData data, string profileID)
    {
        //If a dataFile ID is not assigned, return null to prevent crashes.
        if (profileID == null)
        {
            return;
        }

        //create the save path for the .game file and .game.bak file.
        string fullPath = Path.Combine(dataDirPath, profileID, dataFileName);

        string backupFilePath = fullPath + backupExtension;

        try
        {
            //create the directory to save the files in.
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            //Serialize the GameData object
            string dataToStore = JsonUtility.ToJson(data, true);

            //If encrpytion is enabled, encrypt the serialized data.
            if (useEncryption)
            {
                dataToStore = EncrpytDecrypt(dataToStore);
            }

            //Write the data through a stream and writer to prevent file locking if game crashes.
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }

            //Validate the file is not corrupted before continuing.
            GameData verifiedGameData = Load(profileID);

            //Copy the data to a backup file.
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

    /// <summary>
    /// Deletes a game file from the directory.
    /// </summary>
    /// <param name="profileId">Associated ID with the game file.</param>
    public void Delete(string profileId)
    {
        //If a profile ID is not assigned, return null to prevent crashes.
        if (profileId == null)
        {
            return;
        }

        //Create the path we need to search for.
        string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);

        //Verify the file exists and elete it if it does.
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
            Debug.LogError("Failed to delete profile data from profile ID: " + profileId + " at path: " + fullPath + "\n" + e);
        }


    }

    /// <summary>
    /// Generates a dictory of the profile IDs assocaited with the saved games.
    /// </summary>
    /// <returns>A list of Profile IDs with the associated saved game.</returns>
    public Dictionary<string, GameData> loadAllProfiles()
    {
        Dictionary<string, GameData> profileDictionary = new Dictionary<string, GameData>();

        //Get the saved files in the directory
        IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(dataDirPath).EnumerateDirectories();

        //Loop through the saved files in the directory.
        foreach (DirectoryInfo dirInfo in dirInfos)
        {
            //Get the profile ID from the dictornay object and create the full path.
            string profileId = dirInfo.Name;

            string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
            if (!File.Exists(fullPath))
            {
                Debug.LogWarning("Skipping directory when loading all profiles because it does not contain data: " + profileId);
                continue;
            }

            //Load the data from the full path.
            GameData profileData = Load(profileId);

            //Load the profile id and game data from the file into the dictonary.
            if (profileData != null)
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
    /// <returns>Profile ID</returns>
    public string GetMostRecentlyUpdatedProfileID()
    {
        //Load all the saved games from the directory.
        string mostRecentProfileID = null;
        Dictionary<string, GameData> profilesGameData = loadAllProfiles();

        foreach (KeyValuePair<string, GameData> pair in profilesGameData)
        {
            string profileID = pair.Key;
            GameData gameData = pair.Value;

            if (gameData == null)
            {
                continue;
            }
            //Assign the first ID if the returning ID is null.
            if (mostRecentProfileID == null)
            {
                mostRecentProfileID = profileID;
            }
            else
            {
                //Loads the date time from the GameData for the current loop vs the mostRecenetID
                DateTime mostRecentDateTime = DateTime.FromBinary(profilesGameData[mostRecentProfileID].lastUpdated);
                DateTime newDateTime = DateTime.FromBinary(gameData.lastUpdated);

                //assign the ID if the current loop is greater
                if (newDateTime > mostRecentDateTime)
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

        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }
        return modifiedData;
    }

    /// <summary>
    /// Attempt to load the .bak file and save it over the .game file if the data is corrupted.
    /// </summary>
    /// <param name="fullPath"></param>
    /// <returns></returns>
    private bool AttemptRollback(string fullPath)
    {
        bool success = false;

        string backupFilePath = fullPath + backupExtension;
        try
        {
            if (File.Exists(backupFilePath))
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
        catch (Exception ex)
        {
            Debug.Log("Error occured when trying to roll back to backup file at: " + backupFilePath + '\n' + ex);
        }

        return success;
    }
}


