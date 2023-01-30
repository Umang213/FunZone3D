using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

public static class SaveSystem
{
    public class unlockableData
    {
        int id;
        int Price;

        /*public unlockableData(Unlockable unlockable)
        {
            this.id = unlockable.id;
            this.Price = unlockable.price;
        }*/
    }

    //public List<unlockableData> allUnlockableDatas = new List<unlockableData>();

    //public static void SaveData(Unlockable unlockable)
    public static void SaveData(List<unlockableData> allUnlockable)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/player.data";
        FileStream stream = new FileStream(path, FileMode.Create);

        //unlockableData data = new unlockableData(unlockable);
        List<unlockableData> data = allUnlockable;

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static unlockableData LoadData()
    {
        string path = Application.persistentDataPath + "/player.data";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            unlockableData data = formatter.Deserialize(stream) as unlockableData;
            stream.Close();
            return data;
        }
        else
        {
            return null;
        }
    }
}
