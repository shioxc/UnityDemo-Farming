using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;

public class SaveSlotUI : MonoBehaviour,IPointerClickHandler,IPointerDownHandler,IPointerUpHandler,IPointerExitHandler
{
    public bool newGame;
    public GameObject newGameInterface;
    public GameObject SaveInterface;
    public TMP_Text Day;
    public TMP_Text MoneyNum;
    public TMP_Text PlayTime;
    public InputSystemUIInputModule InputSystemUIInputModule;
    public string saveId;

    public StringListEventSO LoadSceneEventSO;
    public StringListEventSO UnLoadSceneEventSO;
    public List<string> ScenesToUnload;
    public List<string> nextScene;
    public void NewGameSlotCreate()
    {
        newGame = true;
        newGameInterface.SetActive(true);
        SaveInterface.SetActive(false);
    }
    public void SaveSlotCreate(string s)
    {
        newGame = false;
        newGameInterface.SetActive(false);
        SaveInterface.SetActive(true);
#if UNITY_EDITOR
        s = Path.Combine(Path.Combine(Application.dataPath, "Data"), s);
        s = Path.Combine(s, "GeneralData.json");
#else
        s = Path.Combine(Path.Combine(Application.persistentDataPath, "Data"), s);
        s = Path.Combine(s, "GeneralData.json");
#endif
        string json = File.ReadAllText(s);
        GeneralDatabase database = JsonUtility.FromJson<GeneralDatabase>(json);
        Day.text = $"第{database.database.totalDay}天";
        MoneyNum.text = $"{database.database.gold}";
        PlayTime.text = $"{database.database.playTime/60}:{database.database.playTime%60}";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        InputSystemUIInputModule.enabled = false;
        string filePath;
#if UNITY_EDITOR
        filePath = Path.Combine(Application.dataPath, "Data");
#else
        filePath = Path.Combine(Application.persistentDataPath, "Data");
#endif
        if (newGame)
        {
            saveId = Guid.NewGuid().ToString();

        }
        CheckSaveStruct(filePath);
        DataManager.instance.SetSaveId(saveId);
        DataManager.instance.LoadAll();
        UnLoadSceneEventSO.RaiseEvent(ScenesToUnload);
        LoadSceneEventSO.RaiseEvent(nextScene);
    }
    public void CheckSaveStruct(string filePath)//创建存档结构
    {
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }
        string savePath = Path.Combine(filePath, saveId);
        while (Directory.Exists(savePath)&&newGame)
        {
            saveId = Guid.NewGuid().ToString();
            savePath = Path.Combine(filePath, saveId);
        }
        if (newGame)
        {
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
        }
        string dropSavePath = Path.Combine(savePath, "DropPath");
        {
            if (!Directory.Exists(dropSavePath))
            {
                Directory.CreateDirectory(dropSavePath);
            }
        }
        savePath = Path.Combine(savePath, "Map");
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

    }
    public void OnPointerDown(PointerEventData eventData)
    {
        gameObject.GetComponent<Image>().color = new Color(0.8f, 0.8f, 0.8f, 1f);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        gameObject.GetComponent<Image>().color = Color.white;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gameObject.GetComponent<Image>().color = Color.white;
    }
}
