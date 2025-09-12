using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class EntityEveryMap : MonoBehaviour
{
    public Tilemap tilemap;
    public GameObject mapLoader;
    public Dictionary<Vector3Int,List<Entity>> Entites = new Dictionary<Vector3Int,List<Entity>>();
    private string mapName;
    public VoidEventSO EntityUpdateEventSO;
    public EntityListEventChannelSO GetEntityListEventSO;
    public Vector3IntAndEntityEventSO DeleteEntityEventSO;
    public CreateEntityEventSO CreateEntityEventSO;
    public void Awake()
    {
        mapName = mapLoader.GetComponent<MapLoader>().mapName;
        LoadEntity();
    }
    private void OnEnable()
    {
        CreateEntityEventSO.OnEventRaised += CreateEntity;
        DeleteEntityEventSO.OnEventRaised += DeleteEntity;
        GetEntityListEventSO.OnRequest += ProvideEntityList;
        EntityUpdateEventSO.onEventRaised += Refresh;
    }
    private void OnDisable()
    {
        CreateEntityEventSO.OnEventRaised -= CreateEntity;
        DeleteEntityEventSO.OnEventRaised -= DeleteEntity;
        GetEntityListEventSO.OnRequest -= ProvideEntityList;
        EntityUpdateEventSO.onEventRaised -= Refresh;
        Refresh();
    }
    private void LoadEntity()
    {
        if(!EntitySaveLoader.instance.entityDatabase.TryGetValue(mapName,out var value))
        {
            EntitySaveDatabase database = new EntitySaveDatabase();
            database.mapName = mapName;
            database.database = new List<EntitySaveData>();
            EntitySaveLoader.instance.entityDatabase[mapName] = database;
            Refresh();
            return;
        }
        foreach(var data in EntitySaveLoader.instance.entityDatabase[mapName].database)
        {
            GameObject entity = EntityDatabase.GetPrefab(data.entityId);
            Vector3Int startPos = data.cellpos.ToVector3Int();//左下角

            Vector3 vec = new Vector3((float)data.cellpos.x + 0.5f, (float)data.cellpos.y + 0.5f, 0f);
            GameObject obj = Instantiate(entity, vec, Quaternion.identity, transform);
            obj.GetComponent<Entity>().Initialized(data);

            if (!Entites.TryGetValue(data.cellpos.ToVector3Int(), out var entityList))
            {
                List<Entity> entities = new List<Entity>();
                Entites[data.cellpos.ToVector3Int()] = entities;
            }

            for (int i= 0;i < obj.GetComponent<Entity>().data.col;i++)
            {
                for(int j= 0;j<obj.GetComponent<Entity>().data.row;j++)
                {
                    Vector3Int _pos = new Vector3Int(startPos.x+i,startPos.y+j,0);
                    Entites[_pos].Add(obj.GetComponent<Entity>());
                }
            }//大型实体
        }
        EntityUpdateEventSO.RaiseEvent();
    }
    private void Refresh()
    {
        List<EntitySaveData> entities = new List<EntitySaveData>();
        foreach(Transform entity in transform)
        {
            entity.GetComponent<Entity>().cellPos = new Vector3Int(Mathf.FloorToInt(entity.position.x),Mathf.FloorToInt(entity.position.y),Mathf.FloorToInt(entity.position.z));
            entities.Add(entity.GetComponent<Entity>().ToSaveData());
        }
        EntitySaveLoader.instance.entityDatabase[mapName].database = entities;
    }

    private void ProvideEntityList(Vector3Int pos,UnityAction<List<Entity>> callback)
    {
        if(!Entites.TryGetValue(pos, out List<Entity> entities))
        {
            entities = new List<Entity>();
            Entites[pos] = entities;
        }
        callback?.Invoke(entities);
    }
    private void DeleteEntity(Vector3Int pos,Entity entity)
    {
        for (int i = 0;i<entity.data.col;i++)
        {
            for(int j=0;j<entity.data.row;j++)
            {
                Vector3Int _pos = new Vector3Int(pos.x + i, pos.y + j, 0);
                if(entity.type == EntityEnum.Crop)
                {
                    Entites[_pos][0].GetComponent<SoilEntity>().isPlanted = false;
                }
                Entites[_pos].Remove(entity);
            }
        }
    }

    private void CreateEntity(Vector3Int cellpos,int prefabId,EntitySaveData data)
    {
        GameObject entity = EntityDatabase.GetPrefab(prefabId);
        Vector3 vec = new Vector3((float)cellpos.x + 0.5f, (float)cellpos.y + 0.5f, 0f);
        GameObject obj = Instantiate(entity, vec, Quaternion.identity, transform);
        obj.GetComponent<Entity>().Initialized(data);
        Vector3Int startPos = data.cellpos.ToVector3Int();
        if (!Entites.TryGetValue(data.cellpos.ToVector3Int(), out var entityList))
        {
            List<Entity> entities = new List<Entity>();
            Entites[data.cellpos.ToVector3Int()] = entities;
        }

        for (int i = 0; i < obj.GetComponent<Entity>().data.col; i++)
        {
            for (int j = 0; j < obj.GetComponent<Entity>().data.row; j++)
            {
                Vector3Int _pos = new Vector3Int(startPos.x + i, startPos.y + j, 0);
                Entites[_pos].Add(obj.GetComponent<Entity>());
            }
        }
    }
}
