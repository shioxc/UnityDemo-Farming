using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Threading.Tasks;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    public string saveId;
    private List<ISaveable> saveList = new List<ISaveable>();

    private List<Task> runningTasks = new List<Task>();

    public VoidEventSO AfterNextDayEventSO;
    public ItemUseDicSO ItemUseDicSO;
    public EntityListEventChannelSO EntityListEventChannelSO;
    public ItemIdToEntityDataIdDic entityDataDicSO;

    private void Awake()
    {
        ItemUseManager.Initialize(ItemUseDicSO,EntityListEventChannelSO,entityDataDicSO);
        string dataPath;
#if UNITY_EDITOR
        dataPath = Path.Combine(Application.dataPath,"Data");
#else
        dataPath = Path.Combine(Application.persistentDataPath, "Data");
#endif
        if(!Directory.Exists(dataPath))
        {
            Directory.CreateDirectory(dataPath);
        }
        if(instance != null&&instance!=this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void OnEnable()
    {
        AfterNextDayEventSO.onEventRaised += SaveAll; 
    }
    private void OnDisable()
    {
        AfterNextDayEventSO.onEventRaised -= SaveAll;
    }
    public void Register(ISaveable s)
    {
        if(!saveList.Contains(s))
            saveList.Add(s);
    }
    public void UnRegister(ISaveable s)
    {
        if(saveList.Contains(s))
            saveList.Remove(s);
    }
    public void SetSaveId(string saveId)
    {
        instance.saveId = saveId;
    }
    public void SaveAll()
    {
        foreach(var s in saveList)
        {
            s.OnSave(saveId);
        }
    }
    public void LoadAll()
    {
        foreach(var s in saveList)
        {
            s.OnLoad(saveId);
        }
    }
    public void AddTask(Task t)
    {
        lock (runningTasks)
        {
            runningTasks.Add(t);
        }
        t.ContinueWith(_ =>
        {
            lock (runningTasks)
            {
                runningTasks.Remove(t);
            }
        });
    }
    public async Task WaitAll()
    {
        Task[] tasksCopy;
        lock (runningTasks)
        {
            tasksCopy = runningTasks.ToArray();
        }
        await Task.WhenAll(tasksCopy);
    }
}
