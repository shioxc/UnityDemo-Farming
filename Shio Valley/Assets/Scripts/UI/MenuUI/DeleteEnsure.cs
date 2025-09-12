using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DeleteEnsure : MonoBehaviour
{
    private string saveId;
    public StringEventSO ShowTipEventSO;
    public Transform content;
    public FolderReader FolderReader;
    public GameObject tip;

    private void OnEnable()
    {
        ShowTipEventSO.OnEventraised += ShowTip;
    }
    private void OnDisable()
    {
        ShowTipEventSO.OnEventraised -= ShowTip;
    }
    private void ShowTip(string saveId)
    {
        Debug.Log(saveId);
        this.saveId = saveId;
        tip.SetActive(true);
    }
    public void Confirm()
    {
#if UNITY_EDITOR
        string filePath = Path.Combine(Application.dataPath, "Data");
#else
        string filePath = Path.Combine(Application.persistentDataPath,"Data");
#endif
        filePath = Path.Combine(filePath,saveId);
        if (Directory.Exists(filePath))
        {
            Directory.Delete(filePath, true);
        }
        saveId = null;
        tip.SetActive(false);
        foreach (Transform t in content)
        {
            Destroy(t.gameObject);
        }
        FolderReader.RefreshList();
    }
public void Cancel()
    {
        saveId = null;
        tip.SetActive(false);
    }
}
