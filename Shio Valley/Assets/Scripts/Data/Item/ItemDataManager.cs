using System.IO;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;

public static class ItemDataManager
{
#if UNITY_EDITOR 
    private static string filePath => Path.Combine(Application.dataPath, "Data/items.json");
#else
    private static string filePath => Path.Combine(Application.persistentDataPath, "Data/items.json");
#endif
    public static ItemDatabase Load()
    {
        if (!File.Exists(filePath))
        {
            Debug.LogWarning("Item database not found, creating new one.");
            string path;
#if UNITY_EDITOR
            path = Path.Combine(Application.dataPath, "IniData/items.json");
            string _json = File.ReadAllText(path);
            ItemDatabase _itemDatabase = JsonUtility.FromJson<ItemDatabase>(_json);
            foreach (Item item in _itemDatabase.items)
            {
                if (item.canUsedinseason == null || item.canUsedinseason.Count != 4)
                {
                    item.canUsedinseason = new List<bool>();
                    for (int i = 0; i < 4; i++)
                    {
                        item.canUsedinseason.Add(false);
                    }
                }
            }
            return _itemDatabase;
#endif
            return new ItemDatabase { items = new List<Item>() };
        }
        string json = File.ReadAllText(filePath);
        ItemDatabase itemDatabase = JsonUtility.FromJson<ItemDatabase>(json);
        foreach (Item item in itemDatabase.items)
        {
            if(item.canUsedinseason==null||item.canUsedinseason.Count!=4)
            {
                item.canUsedinseason = new List<bool>();
                for(int i =0;i<4;i++)
                {
                    item.canUsedinseason.Add(false);
                }
            }
        }
        return itemDatabase;
    }
    public static void Save(ItemDatabase database)
    {
        string json = JsonUtility.ToJson(database);
        if(File.Exists(filePath))
        File.WriteAllText(filePath, json);
#if UNITY_EDITOR
        string iniFilePath = Path.Combine(Application.dataPath, "IniData/items.json");
        File.WriteAllText(iniFilePath, json);
#endif
#if UNITY_EDITOR
    UnityEditor.AssetDatabase.Refresh();
#endif
    }
    public static async Task IniLoad(string filePath)
    {
        var handle = Addressables.LoadAssetAsync<TextAsset>("items");
        await handle.Task;
        if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
        {
            string json = handle.Result.text;
            File.WriteAllText(filePath, json);
            Addressables.Release(handle);
        }
        ItemLoader.SetItems();
    }
}
