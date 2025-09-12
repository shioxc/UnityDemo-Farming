using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ItemLoader : MonoBehaviour
{
#if UNITY_EDITOR
    private static string filePath => Path.Combine(Application.dataPath, "Data/items.json");
#else
    private static string filePath => Path.Combine(Application.persistentDataPath, "Data/items.json");
#endif
    private static ItemDatabase database;
    private static Dictionary<int, Item> items = new Dictionary<int, Item>();
    private void Awake()
    {
        if (!File.Exists(filePath))
        {
            var Task = ItemDataManager.IniLoad(filePath);
            return;
        }
        database = ItemDataManager.Load();
        foreach (var item in database.items)
        {
            items[item.id] = item;
        }
        DontDestroyOnLoad(gameObject);
    }
    public static ItemDatabase GetItemDatabase()
    {
        return database;
    }
    public static Item GetItemById(int id)
    {
        return items[id];
    }
    public static void SetItems()
    {
        database = ItemDataManager.Load();
        foreach (var item in database.items)
        {
            items[item.id] = item;
        }
    }
}
