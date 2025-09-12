using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Newtonsoft.Json;
[System.Serializable]
public class TileData
{
    public string tileName;
    public int layer;
    public int digedLevel;//0~4
    public bool isWatered;
    public bool hasPlacement;
    public bool isPlanted;
    public bool canPlace;
    public bool canPlant;
    public bool canFish;

    public int rotation;
}
[System.Serializable]
public class TileDatabase
{
    public Dictionary<Vector3Int, Dictionary<int, TileData>> cells = new Dictionary<Vector3Int, Dictionary<int, TileData>>();
}
