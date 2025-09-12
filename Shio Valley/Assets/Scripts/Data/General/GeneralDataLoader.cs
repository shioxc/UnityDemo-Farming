using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GeneralDataLoader : MonoBehaviour,ISaveable
{
    public GeneralDatabase database;

    public static GeneralDataLoader instance;
    public VoidEventSO NextDayEventSO;
    private void Awake()
    {
        if(instance != null && instance != this)
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
    private void OnEnable()
    {
        if(DataManager.instance != null)    
        DataManager.instance.Register(instance);
        NextDayEventSO.onEventRaised += NewDay;
    }
    private void OnDisable()
    {
        DataManager.instance.UnRegister(instance);
        NextDayEventSO.onEventRaised -= NewDay;
    }
    private void NewDay()
    {
        instance.database.database.totalDay++;
        instance.database.database.day++;
        instance.database.database.date++;
        if(instance.database.database.date >7)
        {
            instance.database.database.date = 1;
        }
        if(instance.database.database.day >28)
        {
            instance.database.database.day = 1;
            instance.database.database.season = (instance.database.database.season + 1);
        }
    }

    public void OnLoad(string saveId)
    {
        saveId = Path.Combine("Data",saveId);
        string filePath = Path.Combine(saveId, "GeneralData.json");
        string finalFilePath;
#if UNITY_EDITOR
        finalFilePath = Path.Combine(Application.dataPath, filePath);
#else
        finalFilePath = Path.Combine(Application.persistentDataPath, filePath);
#endif
        if (!File.Exists(finalFilePath))
        {
            var task = GeneralDataManager.IniLoad(filePath);
            DataManager.instance.AddTask(task);
            return;
        }
        instance.database = GeneralDataManager.Load(finalFilePath);
    }

    public void OnSave(string saveId)
    {
        saveId = Path.Combine("Data", saveId);
        string filePath = Path.Combine(saveId, "GeneralData.json");
        GeneralDataManager.Save(instance.database,filePath);
    }

}
