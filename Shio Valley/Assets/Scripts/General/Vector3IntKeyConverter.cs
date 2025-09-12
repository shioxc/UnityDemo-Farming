using Newtonsoft.Json;
using UnityEngine;
using System;
using System.Collections.Generic;

public class Vector3IntKeyConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(Dictionary<Vector3Int, Dictionary<int, TileData>>);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var temp = serializer.Deserialize<Dictionary<string, Dictionary<int, TileData>>>(reader);
        var result = new Dictionary<Vector3Int, Dictionary<int, TileData>>();
        foreach (var kvp in temp)
        {
            Vector3Int pos = ParseVector3Int(kvp.Key);
            result[pos] = kvp.Value;
        }
        return result;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var dict = (Dictionary<Vector3Int, Dictionary<int, TileData>>)value;
        var temp = new Dictionary<string, Dictionary<int, TileData>>();
        foreach (var kvp in dict)
        {
            temp[kvp.Key.ToString()] = kvp.Value;
        }
        serializer.Serialize(writer, temp);
    }

    private Vector3Int ParseVector3Int(string s)
    {
        s = s.Trim('(', ')');
        var parts = s.Split(',');
        return new Vector3Int(
            int.Parse(parts[0]),
            int.Parse(parts[1]),
            int.Parse(parts[2])
        );
    }
}