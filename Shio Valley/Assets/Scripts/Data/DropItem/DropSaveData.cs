using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DropSaveData
{
    public float x;
    public float y;
    public float z;
    public int itemId;
    public int num;
}
[System.Serializable]
public class DropSaveDatabase
{
    public string mapName;
    public List<DropSaveData> database;
}
[System.Serializable]
public class DropSaveDatabaseList
{
    public List<DropSaveDatabase> databaseList;
}

