using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TileDataManager : MonoBehaviour
{
    public static void Save(string filePath,TileDatabase database)
    {
        var settings = new JsonSerializerSettings();
        settings.Converters.Add(new Vector3IntKeyConverter());
        string json = JsonConvert.SerializeObject(database,settings);
        File.WriteAllText(filePath, json);

    }

    public static TileDatabase Load(string filePath)
    {
        var settings = new JsonSerializerSettings();
        settings.Converters.Add(new Vector3IntKeyConverter());
        if (!File.Exists(filePath))
        {
            return new TileDatabase();
        }
        string json = File.ReadAllText(filePath);

        return JsonConvert.DeserializeObject<TileDatabase>(json,settings);
    }
}
