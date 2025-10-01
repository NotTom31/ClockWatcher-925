//Sets up a interface that forces scripts to include the two methods below. Only include this interface if data needs to be saved from that object.
public interface IDataPersistance
{
    void LoadData(GameData date);

    void SaveData(GameData date);
}
