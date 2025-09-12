using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    private SpriteRenderer icon;
    public int itemId;
    public int num;
    public float pickupDuration;
    public float speed;
    public float range;
    private Transform player;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }
    private void Update()
    {
        if(pickupDuration >0)
        {
            pickupDuration -= Time.unscaledDeltaTime;
        }
        else
        {
            if (player == null) return;
            float distance = Vector3.Distance(player.position,transform.position);
            if (distance <= range)
            {
                if (InventoryLoader.instance.CheckBag(itemId,num))
                {
                    attract(distance);
                }
            }
        }
        if (!this.gameObject.activeSelf) return;
        DropManager dropManager = transform.parent.GetComponent<DropManager>();
        string mapName = dropManager.mapManager.GetComponent<MapLoader>().mapName;
        var dropSaveData = dropManager.DropItemDic[this];
        var dropdata = DropSaveLoader.instance.dropSaveDatabase[mapName].database.Find(data => data == dropSaveData );
        if (dropdata != null)
        {
            dropdata.x = transform.position.x;
            dropdata.y = transform.position.y;
            dropdata.z = transform.position.z;
        }
    }
    public void SetItem(int id,int _num)
    {
        itemId = id;
        num = _num;
        icon = GetComponent<SpriteRenderer>();
        SpriteCache.GetSprite(ItemLoader.GetItemById(itemId).iconKey, sprite => {
            if (sprite != null)
            {
                icon.sprite = sprite;
            }
        });
    }
    private void attract(float distance)
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.unscaledDeltaTime);
        if(distance<=0.2f)
        {
            OnPickup();
        }
    }
    private void OnPickup()
    {
        transform.parent.GetComponent<DropManager>().CollectDrop(this);
        player.gameObject.GetComponent<ItemPicker>().PickupItem(itemId,num);
    }
}
