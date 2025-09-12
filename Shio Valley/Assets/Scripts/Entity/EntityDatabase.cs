using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;

public static class EntityDatabase
{
    public static Dictionary<int,GameObject> prefabs = new Dictionary<int,GameObject>();
    public static Dictionary<int,EntityData> entityData = new Dictionary<int,EntityData>();

    public static async Task InitializeDic()
    {
        var handle = Addressables.LoadAssetsAsync<GameObject>("EntityPrefab",null);
        await handle.Task;
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            foreach (var prefab in handle.Result)
            {
                Entity entity = prefab.GetComponent<Entity>();
                prefabs[entity.entityId] = prefab;
            }
        }
        var handle1 = Addressables.LoadAssetsAsync<EntityData>("EntityData", null);
        await handle1.Task;
        if (handle1.Status == AsyncOperationStatus.Succeeded)
        {
            foreach (var data in handle1.Result)
            {
                entityData[data.id] = data;
            }
        }
    }
    public static GameObject GetPrefab(int id)
    {
        if(prefabs.TryGetValue(id, out var entity))
        {
            return entity;
        }
        return null;
    }
    public static EntityData GetEntityData(int id)
    {
        if(entityData.TryGetValue(id,out var entity))
        {
            return entity;
        }
        return null ;
    }
}
