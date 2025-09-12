using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;

public class InventoryLoader : MonoBehaviour,ISaveable
{
    public InventoryDatabase database;
    public static InventoryLoader instance;
    public VoidEventSO OnPickUpEventSO;
    private void Awake()
    {
        if (instance != null && instance != this)
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
    }
    private void OnDisable()
    {
        DataManager.instance.UnRegister(instance);
    }
    public void OnLoad(string saveId)
    {
        saveId = Path.Combine("Data", saveId);
        string filePath = Path.Combine(saveId, "Inventories.json");
        string finalFilePath;
#if UNITY_EDITOR
        finalFilePath = Path.Combine(Application.dataPath, filePath);
#else
        finalFilePath = Path.Combine(Application.persistentDataPath, filePath);
#endif
        if (!File.Exists(finalFilePath))
        {
            var task = InventoryDataManager.IniLoad(filePath);
            DataManager.instance.AddTask(task);
            return;
        }
        instance.database = InventoryDataManager.Load(finalFilePath);
    }
    public void OnSave(string saveId)
    {
        saveId = Path.Combine("Data", saveId);
        string filePath = Path.Combine(saveId, "Inventories.json");
        InventoryDataManager.Save(instance.database, filePath);
    }

    public void AddItem(int id,int num)
    {
        Item item = ItemLoader.GetItemById(id);
        foreach (var slot in instance.database.inventories)
        {
            if (slot.itemId == id)
            {   
                if (num <= 0)
                {
                    OnPickUpEventSO.RaiseEvent();
                    return;
                }
                int _num = Mathf.Max(item.maxStack-slot.storeNum,0);
                int addNum = Mathf.Min(_num, num);
                slot.storeNum += addNum;
                num -= addNum;
            }
        }
        foreach (var slot in instance.database.inventories)
        {
            if (slot.itemId == 0)
            {
                if (num <= 0) 
                {
                    OnPickUpEventSO.RaiseEvent();
                    return;
                }
                int _num = Mathf.Max(item.maxStack - slot.storeNum, 0);
                int addNum = Mathf.Min(_num, num);
                slot.itemId = id;
                slot.storeNum += addNum;
                num -= addNum;
                
            }
        }
    }
    public bool CheckBag(int id,int num)
    {
        Item item = ItemLoader.GetItemById(id);
        foreach (var slot in instance.database.inventories)
        {
            if (slot.itemId == id)
            {
                num -= Mathf.Max(item.maxStack-slot.storeNum,0);
                if (num <= 0) return true;
            }
            else if(slot.itemId == 0)
            {
                num -= item.maxStack;
                if (num <= 0) return true;
            }
        }
        return false;
    }

    public void ReduceItem(int index,int num)
    {
        instance.database.inventories[index].storeNum -= num;
        if (instance.database.inventories[index].storeNum<=0)
        {
            instance.database.inventories[index].itemId = 0;
        }
        OnPickUpEventSO.RaiseEvent();
    }
}
