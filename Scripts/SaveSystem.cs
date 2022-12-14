using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveData (GameMaster gm) {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/savedata.fuga";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData(gm);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SaveData Load() {
        string path = Application.persistentDataPath + "/savedata.fuga";
        if(File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();
            
            return data;
        } else {
            //Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
