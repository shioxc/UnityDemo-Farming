using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DropSaveLoader : MonoBehaviour,ISaveable
{
    public static DropSaveLoader instance;
    public Dictionary<string, DropSaveDatabase> dropSaveDatabase = new Dictionary<string, DropSaveDatabase>();//mapName,database
    private void Awake()
    {
        if(instance != null&&instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    private void Start()
    {
        DataManager.instance.Register(instance);
    }
    private void OnDisable()
    {
        DataManager.instance.UnRegister(instance);
    }
    public void Add(string mapName,DropSaveData dropSaveData)
    {
        if (!instance.dropSaveDatabase.ContainsKey(mapName))
        {
            DropSaveDatabase database = new DropSaveDatabase();
            instance.dropSaveDatabase[mapName] = database;
            instance.dropSaveDatabase[mapName].mapName = mapName;
            database.database = new List<DropSaveData>();
        }
        instance.dropSaveDatabase[mapName].database.Add(dropSaveData);
    }

    public void OnSave(string saveId)
    {
        saveId = Path.Combine("Data", saveId);
        string filePath =Path.Combine(saveId,"DropPath/DropSaveData.json");
#if UNITY_EDITOR
        filePath = Path.Combine(Application.dataPath, filePath);
#else
        filePath = Path.Combine(Application.persistentDataPath, filePath);
#endif
        DropSaveDatabaseList dropSaveDatabaseList = new DropSaveDatabaseList();
        dropSaveDatabaseList.databaseList = new List<DropSaveDatabase>();
        foreach(var database in dropSaveDatabase)
        {
            dropSaveDatabaseList.databaseList.Add(database.Value);
        }
        DropSaveManager.Save(dropSaveDatabaseList,filePath);
    }
    public void OnLoad(string saveId)//没有文件就是没掉落物，直接生成空
    {
        saveId = Path.Combine("Data", saveId);
        string filePath = Path.Combine(saveId,"DropPath/DropSaveData.json");
#if UNITY_EDITOR
        filePath = Path.Combine(Application.dataPath, filePath);
#else
        filePath = Path.Combine(Application.persistentDataPath, filePath);
#endif
        DropSaveDatabaseList dropSaveDatabaseList = DropSaveManager.Load(filePath);
        foreach(var database in dropSaveDatabaseList.databaseList)
        {
            dropSaveDatabase[database.mapName] = database;
        }
    }
}
