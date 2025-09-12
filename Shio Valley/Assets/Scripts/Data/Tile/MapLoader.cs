using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;
using System.Linq;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;
using System;

public class MapLoader : MonoBehaviour
{
    public string filePath;
    private string saveId;
    public Tilemap[] tilemaps;
    public string[] canPlant;
    public string[] canPlace;
    public string[] canFish;
    public string mapName;

    public StringAndTilemapsEventSO LoadMapEventSO;

    private Dictionary<string, TileBase> cache = new Dictionary<string, TileBase>();

    public VoidEventSO NextDayEventSO;
    public VoidEventSO SaveAllMapEventSO;
    private void Awake()
    {
        saveId = DataManager.instance.saveId;
        filePath = Path.Combine(saveId, filePath);
        filePath = Path.Combine("Data", filePath);
#if UNITY_EDITOR
        filePath = Path.Combine(Application.dataPath, filePath);
#else
        filePath = Path.Combine(Application.persistentDataPath, filePath);
#endif
    }
    private void Start()
    {
        if(!File.Exists(filePath))
        {
            Debug.Log("Load");
            IniSave();
        }
        else if (!TileLoader.instance.database.ContainsKey(mapName))
        {
            TileLoader.Load(mapName,filePath);
        }
        LoadMap();
    }
    private void OnEnable()
    {
        NextDayEventSO.onEventRaised += SaveAll;
    }
    private void OnDisable()
    {
        NextDayEventSO.onEventRaised -= SaveAll;
    }
    private void IniSave()
    {
        for (int layer = 0; layer < tilemaps.Length; layer++)
        {

            Tilemap map = tilemaps[layer];
            if (map == null) continue;
            BoundsInt bounds = map.cellBounds;
            if(!TileLoader.instance.database.TryGetValue(mapName,out var value))
            {
                value = new TileDatabase();
                value.cells = new Dictionary<Vector3Int, Dictionary<int, TileData>>();
                TileLoader.instance.database[mapName] = value;
            }
            foreach(var pos in bounds.allPositionsWithin)
            {
                TileBase tile = map.GetTile(pos);
                Matrix4x4 mat = map.GetTransformMatrix(pos);
                Quaternion rot = mat.rotation;
                Vector3 euler = rot.eulerAngles;
                int rotation = Mathf.RoundToInt(euler.z / 90) * 90;
                rotation = rotation % 360;
                if (tile != null)
                {
                    TileData data = new TileData
                    {
                        tileName = tile.name,
                        layer = layer,
                        digedLevel = 0,
                        isWatered = false,
                        hasPlacement = false,
                        isPlanted = false,
                        canPlace = false,
                        canPlant = false,
                        canFish = false,
                        rotation = rotation,
                    };
                    if(canPlant.Contains(tile.name))
                    {
                        data.canPlant = true;
                    }
                    if (canPlace.Contains(tile.name))
                    {
                        data.canPlace = true;
                    }
                    if(canFish.Contains(tile.name))
                    {
                        data.canFish = true;
                    }
                    if (!TileLoader.instance.database[mapName].cells.ContainsKey(pos))
                        TileLoader.instance.database[mapName].cells[pos] = new Dictionary<int, TileData>();
                    TileLoader.instance.database[mapName].cells[pos][layer] = data;
                }
            }
        }
        TileDataManager.Save(filePath, TileLoader.instance.database[mapName]);
    }

    private void LoadMap()
    {
        LoadMapEventSO.RaiseEvent(mapName, tilemaps);
    }

    public TileData GetTileData(Tilemap map,Vector3Int pos)
    {
        int index = Array.IndexOf(tilemaps,map);
        if(index==-1)
        {
            return null;
        }
        TileData tileData = new TileData();
        tileData = TileLoader.instance.database[mapName].cells[pos][index];
        return tileData;
    }

    private void SaveAll()
    {
        SaveAllMapEventSO.RaiseEvent();
    }
}
