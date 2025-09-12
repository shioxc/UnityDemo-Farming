using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DropManager : MonoBehaviour
{
    public GameObject mapManager;
    public DropItem dropPrefab;
    private ObjectPool<DropItem> dropPool;
    public Dictionary<DropItem, DropSaveData> DropItemDic = new Dictionary<DropItem, DropSaveData>();
    private bool ini;

    public Vector3IntIntEventSO PlayerDropItemEventSO;
    public VoidEventSO TryGetPlayerTranseventSO;
    public TransformEventSo GetPlayerTransformEventSO;
    private Transform player;

    public Vector3AndEntityEventSO EntityDropItemEventSO;
    private void Awake()
    {
        dropPool = new ObjectPool<DropItem>(dropPrefab,50,transform);
        LoadCurDropItem();
    }
    private void OnEnable()
    {
        EntityDropItemEventSO.OnEventRaised += EntityDropItem;
        PlayerDropItemEventSO.OnEventRaised += PlayerDropItem;
        GetPlayerTransformEventSO.OnEventRaised += GetPlayerTransform;
    }
    private void OnDisable()
    {
        EntityDropItemEventSO.OnEventRaised -= EntityDropItem;
        PlayerDropItemEventSO.OnEventRaised -= PlayerDropItem;
        GetPlayerTransformEventSO.OnEventRaised -= GetPlayerTransform;
    }
    private void LoadCurDropItem()
    {
        string mapName = mapManager.GetComponent<MapLoader>().mapName;
        if (!DropSaveLoader.instance.dropSaveDatabase.ContainsKey(mapName)) return;
        ini = true;
        foreach (var drop in DropSaveLoader.instance.dropSaveDatabase[mapName].database)
        {
            Vector3 pos = new Vector3(drop.x,drop.y,drop.z);

            DropItem dropItem = SpawnDrop(pos,drop.itemId,drop.num);
            DropItemDic[dropItem] = drop;
        }
        ini=false;  
    }
    public DropItem SpawnDrop(Vector3 position, int itemId,int num)
    {
        DropItem drop = dropPool.Get();
        drop.transform.position = position;
        drop.SetItem(itemId,num);
        string mapName = mapManager.GetComponent<MapLoader>().mapName;
        if (!ini)
        {
            DropSaveData dropSaveData = new DropSaveData();
            dropSaveData.itemId = itemId;
            dropSaveData.x = position.x; dropSaveData.y = position.y; dropSaveData.z = position.z;
            dropSaveData.num = num;
            DropItemDic[drop] = dropSaveData;
            DropSaveLoader.instance.Add(mapName, dropSaveData);
        }

        drop.gameObject.SetActive(true);
        return drop;
    }
    public void CollectDrop(DropItem drop)
    {
        string mapName = mapManager.GetComponent<MapLoader>().mapName;
        DropSaveLoader.instance.dropSaveDatabase[mapName].database.Remove(DropItemDic[drop]);
        DropItemDic.Remove(drop);
        dropPool.Release(drop);
    }
    public void PlayerDropItem(Vector3 pos,int id,int num)
    {
        DropItem drop = SpawnDrop(pos, id, num);
        TryGetPlayerTranseventSO.RaiseEvent();
        int flipx = 1;
        if (player.GetComponent<SpriteRenderer>().flipX)
        {
            flipx = -1;
        }
        else
        {
            flipx = 1;
        }
        Vector3 offset = new Vector3(flipx*2f,flipx*2f ,0f);
        StartCoroutine(ThrowDrop(drop, pos, offset, 0.8f));

    }
    public void EntityDropItem(Vector3 pos,Entity entity)
    {
        foreach(var loots in entity.data.loots)
        {
            if (loots.stage == entity.stage)
            {
                for(int i = 0; i < loots.num;i++)
                {
                    DropItem drop = SpawnDrop(pos, loots.itemId, 1);
                    drop.pickupDuration = 1f;
                    Vector3 offset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
                    StartCoroutine(ThrowDrop(drop, pos,offset, 0.8f));
                }
            }
        }
    }
    IEnumerator ThrowDrop(DropItem drop, Vector3 startPos, Vector3 targetOffset, float duration)//生成掉落物的抛物线
    {
        float time = 0f;
        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            float t = time / duration;
            float x = Mathf.Lerp(startPos.x, startPos.x + targetOffset.x, t);

            float height = 2f;
            float y = startPos.y + (4 * height * t * (1 - t));

            drop.transform.position = new Vector3(x, y, startPos.z);

            yield return null;
        }
    }
    private void GetPlayerTransform(Transform trans)
    {
        player = trans;
    }
}
