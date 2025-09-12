using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;

public static class EntitySavaDataManager
{
    public static void Save(EntitySaveDatabaseList databaseList,string filePath)
    {
        string json = JsonConvert.SerializeObject(databaseList);
        File.WriteAllText(filePath, json);
        return;
    }
    public static EntitySaveDatabaseList Load(string filePath)
    {
        EntitySaveDatabaseList databaseList =new EntitySaveDatabaseList();
        List<EntitySaveDatabase> list = new List<EntitySaveDatabase>();
        databaseList.databaseList = list;
        if (!File.Exists(filePath))
        {
            Save(databaseList, filePath);
        }
        string json = File.ReadAllText(filePath);
        databaseList = JsonConvert.DeserializeObject<EntitySaveDatabaseList>(json);
        return databaseList;
    }
    public static async Task IniLoad(string finalFilePath)
    {
        var handle = Addressables.LoadAssetAsync<TextAsset>("Entity");
        await handle.Task;
        if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
        {
            string json = handle.Result.text;
            File.WriteAllText(finalFilePath, json);
            Addressables.Release(handle);
            EntitySaveDatabaseList entities = EntitySavaDataManager.Load(finalFilePath);
            foreach (var entity in entities.databaseList)
            {
                EntitySaveLoader.instance.entityDatabase[entity.mapName] = entity;
            }
        }
    }
}
