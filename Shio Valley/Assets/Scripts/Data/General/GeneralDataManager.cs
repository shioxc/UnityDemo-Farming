using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

public class GeneralDataManager : MonoBehaviour
{
    public static void Save(GeneralDatabase database,string filePath)
    {
    #if UNITY_EDITOR
        filePath = Path.Combine(Application.dataPath, filePath);
    #else
        filePath = Path.Combine(Application.persistentDataPath, filePath);
    #endif
        string json = JsonUtility.ToJson(database);
        File.WriteAllText(filePath, json);
    }
    public static GeneralDatabase Load(string filePath)
    {
    #if UNITY_EDITOR
        filePath = Path.Combine(Application.dataPath, filePath);
    #else
        filePath = Path.Combine(Application.persistentDataPath, filePath);
    #endif
        if (!File.Exists(filePath))
        {
            GeneralDatabase database = new GeneralDatabase();
            Save(database,filePath);
            return database;
        }
        string json = File.ReadAllText(filePath);
        return JsonUtility.FromJson<GeneralDatabase>(json);
    }
    public static async Task IniLoad(string filePath)
    {
        string finalFilePath;
#if UNITY_EDITOR
        finalFilePath = Path.Combine(Application.dataPath, filePath);
#else
        finalFilePath = Path.Combine(Application.persistentDataPath, filePath);
#endif
        var handle = Addressables.LoadAssetAsync<TextAsset>("GeneralData");
        await handle.Task;
        if(handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
        {
            string json = handle.Result.text;
            File.WriteAllText(finalFilePath, json);
            Addressables.Release(handle);
            GeneralDataLoader.instance.database = Load(filePath);
        }
    }
}
