using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.UI;

public class FolderReader : MonoBehaviour
{
#if UNITY_EDITOR
    private static string filePath => Path.Combine(Application.dataPath,"Data");
#else
    private static string filePath => Path.Combine(Application.persistentDataPath,"Data");
#endif
    public List<string> saveId = new List<string>();
    public GameObject content;
    public GameObject saveSlot;
    public InputSystemUIInputModule InputSystemUIInputModule;
    private void Start()
    {
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }
        string[] saveFolder = Directory.GetDirectories(filePath);
        foreach(var s in saveFolder)
        {
            saveId.Add(Path.GetFileName(s));
        }
        CreateSaveList();
    }
    public void CreateSaveList()
    {
        GameObject slot = Instantiate(saveSlot,content.transform);
        slot.GetComponent<SaveSlotUI>().NewGameSlotCreate();
        slot.GetComponent<SaveSlotUI>().InputSystemUIInputModule = InputSystemUIInputModule;
        foreach(var s in saveId)
        {
            slot = Instantiate(saveSlot,content.transform);
            slot.GetComponent <SaveSlotUI>().SaveSlotCreate(s); 
            slot.GetComponent<SaveSlotUI>().InputSystemUIInputModule = InputSystemUIInputModule;
            slot.GetComponent<SaveSlotUI>().saveId = s;
        }
    }
    public void RefreshList()
    {
        saveId.Clear();
        string[] saveFolder = Directory.GetDirectories(filePath);
        foreach (var s in saveFolder)
        {
            saveId.Add(Path.GetFileName(s));
        }
        CreateSaveList();
    }
}
