using UnityEngine;

public interface IDataPersistance
{
    void LoadData(GameData date);

    void SaveData(GameData date);
}
