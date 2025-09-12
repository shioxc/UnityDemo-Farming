using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
using System.Linq;
using System.Drawing.Drawing2D;
using UnityEngine.U2D;
using Unity.VisualScripting;

public class TileSelector : MonoBehaviour
{
    public Tilemap[] tilemaps;
    private Transform playerTransform;

    public TransformEventSo GetPlayerTransformEventSO;
    public VoidEventSO TryGetPlayerTransEventSO;
    public Color highlightColor = Color.green;
    private Color originalColor = Color.white;

    public Tilemap SelectedMap;
    public Vector3Int SelectedPos;
    public TileData SelectedTileData;

    public GameObject mapManager;
    public SpriteRenderer indicator;
    public GameobjectEventSO TileSelectorEventSO;
    int dx;
    int dy;
    public EntityEveryMap entityEveryMap;
    public StringEventSO OnCursorChangedEventSO;

    private void Start()
    {
        TileSelectorEventSO.RaiseEvent(gameObject);
        TryGetPlayerTransEventSO.RaiseEvent();
        indicator.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        GetPlayerTransformEventSO.OnEventRaised += GetPlayerTransform;
        
    }
    private void OnDisable()
    {
        ClearSelected();
        GetPlayerTransformEventSO.OnEventRaised -= GetPlayerTransform;
        indicator.gameObject.SetActive(false);
    }
    private void Update()
    {
        Vector3 mouseWolrdPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mouseWolrdPos.z = 0;
        Vector3Int cellPos = tilemaps[0].WorldToCell(mouseWolrdPos);
        Vector3Int playerPos = tilemaps[0].WorldToCell(playerTransform.position);

        dx = Mathf.Abs(cellPos.x - playerPos.x);
        dy = Mathf.Abs(cellPos.y - playerPos.y);
        if(dx<=1&&dy<=1)
        {
            SelectTile(cellPos,playerPos);
        }
        else
        {
            Vector3Int _pos = playerPos;
            int offset = playerTransform.GetComponent<SpriteRenderer>().flipX ? -1 : 1;
            _pos.x += offset;
            SelectTile(_pos,playerPos);
        }
    }
    void SelectTile(Vector3Int cellPos,Vector3Int playerPos)
    {
        Transform activeChild = null;
        foreach (Transform child in playerTransform.Find("Colliders"))
        {
            if (child.gameObject.activeSelf)
            {
                activeChild = child;
                break;
            }
        }
        ClearSelected();
        foreach (var map in tilemaps)
        {
            TileBase mouseTile = map.GetTile(cellPos);
            TileBase playerTile = map.GetTile(playerPos);
            Tilemap map1 = null;
            if (playerTile != null&&map1 == null)
            {
                map1 = map;
            }
            if (mouseTile != null)
            {
                if (map1 == map)
                {
                    if(activeChild.gameObject.layer != map.gameObject.layer)
                    {
                        break;
                    }
                    SelectedMap = map;
                    SelectedPos = cellPos;
                    originalColor = map.GetColor(cellPos);
                    SelectedTileData = mapManager.GetComponent<MapLoader>().GetTileData(map, cellPos);
                    if (dx <= 1 && dy <= 1)
                    {
                        SetIndicator(cellPos,map);
                        CheckCursor(SelectedPos);
                    }
                    else
                    {
                        OnCursorChangedEventSO.RaiseEvent("IniCursor");
                    }
                    return;
                }
            }
        }
        OnCursorChangedEventSO.RaiseEvent("IniCursor");
    }
    private void SetIndicator(Vector3Int cellPos,Tilemap map)
    {
        map.SetColor(cellPos, highlightColor);
        indicator.gameObject.SetActive(true);
        Vector3 cellCenter = map.GetCellCenterWorld(cellPos);
        indicator.transform.position = cellCenter;
    }
    void ClearSelected()
    {
        if (SelectedMap != null)
        {
            SelectedMap.SetColor(SelectedPos, originalColor);
            SelectedMap = null;
        }
        if(SelectedTileData != null)
        {
            SelectedTileData = null;
        }
        indicator.gameObject.SetActive(false);
    }
    private void GetPlayerTransform(Transform _transform)
    {
        playerTransform = _transform;
    }

    private void CheckCursor(Vector3Int pos)
    {
        if(!entityEveryMap.Entites.TryGetValue(pos,out var entities))
        {
            OnCursorChangedEventSO.RaiseEvent("IniCursor");
            return;
        }
        for(int i=entities.Count-1;i>=0;i--)
        {
            if(entities[i].GetComponent<CropEntity>())
            {
                CropEntity crop = entities[i].GetComponent<CropEntity>();
                if (crop.stage == crop.data.stages.Count-1)
                {
                    OnCursorChangedEventSO.RaiseEvent("HarvestCursor");
                    return;
                }
            }
        }
        OnCursorChangedEventSO.RaiseEvent("IniCursor");
    }
}
