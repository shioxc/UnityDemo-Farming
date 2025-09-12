using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EntitySaveLoader : MonoBehaviour, ISaveable
{
    public static EntitySaveLoader instance;
    public Dictionary<string,EntitySaveDatabase> entityDatabase = new Dictionary<string, EntitySaveDatabase>();//mapName,database
    public void Awake()
    {
        if(instance != null&&instance !=this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    public void Start()
    {
        DataManager.instance.Register(instance);
    }
    public void OnDisable()
    {
        DataManager.instance.UnRegister(instance);
    }

    public void OnSave(string saveId)
    {
        saveId = Path.Combine("Data",saveId);
        string filePath = Path.Combine(saveId, "Entity.json");
#if UNITY_EDITOR
        filePath = Path.Combine(Application.dataPath, filePath);
#else
        filePath = Path.Combine(Application.persistentDataPath, filePath);
#endif
        EntitySaveDatabaseList entitySaveDatabaseList = new EntitySaveDatabaseList();
        entitySaveDatabaseList.databaseList = new List<EntitySaveDatabase>();
        foreach (var database in entityDatabase)
        {
            entitySaveDatabaseList.databaseList.Add(database.Value);
        }
        EntitySavaDataManager.Save(entitySaveDatabaseList, filePath);
    }

    public void OnLoad(string saveId)
    {
        saveId = Path.Combine("Data", saveId);
        string filePath = Path.Combine(saveId, "Entity.json");
#if UNITY_EDITOR
        filePath = Path.Combine(Application.dataPath, filePath);
#else
        filePath = Path.Combine(Application.persistentDataPath, filePath);
#endif
        if (!File.Exists(filePath))
        {
            var task = EntitySavaDataManager.IniLoad(filePath);
            DataManager.instance.AddTask(task);
            return;
        }
        EntitySaveDatabaseList entities = EntitySavaDataManager.Load(filePath);
        if (entities.databaseList == null) return;
        foreach (var entity in entities.databaseList) 
        {
            instance.entityDatabase[entity.mapName] = entity;
        }
    }
}
