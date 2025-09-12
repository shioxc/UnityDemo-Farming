using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class EntitySaveData
{
    public int entityId;
    public int dataId;
    public int stage;
    public int lastDay;
    public EntityEnum type;
    public VectorIntData cellpos;
    public Dictionary<string,object> extra;
}

public class EntitySaveDatabase
{
    public string mapName;
    public List<EntitySaveData> database;
}
public class EntitySaveDatabaseList
{
    public List<EntitySaveDatabase> databaseList;
}

[System.Serializable]
public class VectorIntData
{
    public int x, y, z;
    public VectorIntData(Vector3Int pos)
    {
        x=pos.x; 
        y=pos.y; 
        z=pos.z;
    }
    public Vector3Int ToVector3Int()
    {
        return new Vector3Int(x,y,z);
    }
}

