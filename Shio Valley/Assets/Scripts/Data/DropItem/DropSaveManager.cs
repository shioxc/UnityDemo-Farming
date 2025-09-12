using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public static class DropSaveManager
{
    public static void Save(DropSaveDatabaseList database, string filePath)
    {
        string json = JsonUtility.ToJson(database);
        File.WriteAllText(filePath, json);
    }

    public static DropSaveDatabaseList Load(string filePath)
    {
        DropSaveDatabaseList database = new DropSaveDatabaseList();
        if (!File.Exists(filePath))
        {
            Save(database, filePath);
        }
        string json = File.ReadAllText(filePath);
        database = JsonUtility.FromJson<DropSaveDatabaseList>(json);
        return database;
    }
}
