using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Tilemaps;
using System.Threading.Tasks;
using System.IO;
using System;

public class TileLoader : MonoBehaviour
{
    public Dictionary<string, TileDatabase> database;
    public static TileLoader instance;

    public Dictionary<string, TileBase> cache = new Dictionary<string, TileBase>();

    public StringAndTilemapsEventSO LoadMapEventSO;
    public VoidEventSO SaveAllMapEventSO;
    public List<string> mapNames;
    public List<string> mapFilePath;
    public Dictionary<string, string> allFilePath;//名字 路径
    private void Awake()
    {
        allFilePath = new Dictionary<string, string>();
        for(int i=0;i<mapNames.Count;i++)
        {
            allFilePath[mapNames[i]] = mapFilePath[i];
        }
        if (instance == null)
        {
            instance = this;
            instance.database = new Dictionary<string, TileDatabase>();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnEnable()
    {
        LoadMapEventSO.OnEvnetRaised += LoadMap;
        SaveAllMapEventSO.onEventRaised += SaveAll;
    }
    private void OnDisable()
    {
        LoadMapEventSO.OnEvnetRaised -= LoadMap;
        SaveAllMapEventSO.onEventRaised -= SaveAll;
    }
    public static void Load(string scene,string filePath)
    {
        instance.database[scene] = TileDataManager.Load(filePath);
    }
    private async void LoadMap(string mapName, Tilemap[] tilemaps)
    {
        await LoadTilesAsync(mapName, tilemaps);
    }

    private void SaveAll()
    {
        string saveId = DataManager.instance.saveId;
        foreach(var fileMap in allFilePath)
        {
            if(instance.database.ContainsKey(fileMap.Key))
            {
                string filePath = fileMap.Value;
                filePath = Path.Combine(saveId, filePath);
                filePath = Path.Combine("Data", filePath);
#if UNITY_EDITOR
                filePath = Path.Combine(Application.dataPath, filePath);
#else
                filePath = Path.Combine(Application.persistentDataPath, filePath);
#endif
                TileDataManager.Save(filePath, instance.database[fileMap.Key]);
            }
        }
    }

    private async Task LoadTilesAsync(string mapName,Tilemap[] maps)
    {
        for (int layer = 0;layer<maps.Length;layer++)
        {
            Tilemap map = maps[layer];
            map.ClearAllTiles();

            var allTileNames = instance.database[mapName].cells
                .SelectMany(cell => cell.Value.Values)
                .Select(tileData => tileData.tileName)
                .Distinct()
                .ToList();

            await PreloadTilesAsync(allTileNames);
            foreach (var cell in instance.database[mapName].cells)
            {
                Vector3Int pos = cell.Key;
                foreach (var kvp in cell.Value)
                {
                    if(kvp.Key != layer)continue;
                    TileData tileData = kvp.Value;
                    if (cache.TryGetValue(tileData.tileName, out var tileAsset))
                    {
                        map.SetTile(pos, tileAsset);
                        Quaternion rot = Quaternion.Euler(0, 0, tileData.rotation);
                        Matrix4x4 mat = Matrix4x4.TRS(Vector3.zero, rot, Vector3.one);
                        map.SetTransformMatrix(pos, mat);
                    }
                    else
                    {
                        Debug.LogWarning($"Tile 未加载成功: {tileData.tileName}");
                    }
                }
            }
        }
    }
    private async Task PreloadTilesAsync(List<string> tileNames)
    {
        var tasks = new List<Task>();

        foreach (var name in tileNames)
        {
            if (!cache.ContainsKey(name))
            {
                var handle = Addressables.LoadAssetAsync<TileBase>(name);
                var tcs = new TaskCompletionSource<bool>();
                handle.Completed += h =>
                {
                    if (h.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
                    {
                        cache[name] = h.Result;
                        tcs.SetResult(true);
                    }
                    else
                    {
                        Debug.LogWarning($"Addressables 预加载失败: {name}");
                        tcs.SetResult(false);
                    }
                };

                tasks.Add(tcs.Task);
            }
        }

        await Task.WhenAll(tasks);
    }
}
