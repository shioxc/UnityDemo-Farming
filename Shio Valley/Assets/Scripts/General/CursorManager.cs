using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D iniCursor;
    private Texture2D currentCursor;
    public Texture2D harvestCursor;
    private Texture2D cursor;
    public StringEventSO OnCursorChangedEventSO;

    private void Awake()
    {
        cursor = iniCursor;
        SetCursor(cursor);
        DontDestroyOnLoad(gameObject);
    }
    private void OnEnable()
    {
        OnCursorChangedEventSO.OnEventraised += ChangeCursor;
    }
    private void OnDisable()
    {
        OnCursorChangedEventSO.OnEventraised -= ChangeCursor;
    }
    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            SetCursor(cursor);
        }
    }

    private void SetCursor(Texture2D cs)
    {
        if (currentCursor != cs)
        {
            Cursor.SetCursor(cs, new Vector2(6, 4), CursorMode.Auto);
            currentCursor = cs;
        }
    }
    private void ChangeCursor(string cs)
    {
        if(cs == "HarvestCursor")
        {
            cursor = harvestCursor;
        }
        else if(cs == "IniCursor")
        {
            cursor = iniCursor;
        }
        if(currentCursor!=cursor)
        SetCursor(cursor);
    }

}
