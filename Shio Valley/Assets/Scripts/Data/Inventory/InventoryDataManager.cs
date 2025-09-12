using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;

public class InventoryDataManager : MonoBehaviour
{

    public static void Save(InventoryDatabase database,string filePath)
    {
    #if UNITY_EDITOR
        filePath = Path.Combine(Application.dataPath,filePath);
    #else
        filePath = Path.Combine(Application.persistentDataPath, filePath);
    #endif
    string json = JsonUtility.ToJson(database);
        File.WriteAllText(filePath, json);
    }

    public static InventoryDatabase Load(string filePath) 
    {
    #if UNITY_EDITOR
        filePath = Path.Combine(Application.dataPath, filePath);
    #else
        filePath = Path.Combine(Application.persistentDataPath, filePath);
    #endif
        if (!File.Exists(filePath))
        {
            return new InventoryDatabase { inventories = new List<Inventory>() };
        }
        string json = File.ReadAllText(filePath);
        return JsonUtility.FromJson<InventoryDatabase>(json);
    }
    public static async Task IniLoad(string filePath)
    {
        string finalFilePath;
#if UNITY_EDITOR
        finalFilePath = Path.Combine(Application.dataPath, filePath);
#else
        finalFilePath = Path.Combine(Application.persistentDataPath, filePath);
#endif
        var handle = Addressables.LoadAssetAsync<TextAsset>("Inventories");
        await handle.Task;
        if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
        {
            string json = handle.Result.text;
            File.WriteAllText(finalFilePath, json);
            Addressables.Release(handle);
            InventoryLoader.instance.database = Load(filePath);
        }
        
    }
}
