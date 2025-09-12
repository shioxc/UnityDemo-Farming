using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public int entityId;
    public int stage;//0~
    public int lastDay;
    public EntityEnum type;
    public EntityData data;
    public Vector3Int cellPos;
    public List<EntityLoot> loots;

    public SpriteRenderer sr;
    public VoidEventSO EntityUpdateEventSO;
    public Vector3IntAndEntityEventSO DeleteEntityEventSO;
    public Vector3AndEntityEventSO EntityDropItemEventSO;
    private void OnEnable()
    {
        EntityUpdateEventSO.onEventRaised += EntityUpdate;
    }
    private void OnDisable()
    {
        EntityUpdateEventSO.onEventRaised -= EntityUpdate;
    }
    public virtual void EntityUpdate()
    {
        LastDayChange();
    }

    public void LastDayChange()
    {
        lastDay = GeneralDataLoader.instance.database.database.totalDay;
    }
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    
    public virtual void SetStage(int _stage)
    {
        stage = Mathf.Clamp(_stage, 0, data.stages.Count-1);
        sr.sprite = data.stages[stage].sprite;
        loots.Clear();
        foreach(var loot in data.loots)
        {
            if(loot.stage == _stage)
            {
                loots.Add(loot); 
            }
        }
    }
    public virtual void Initialized(EntitySaveData data)
    {
        entityId = data.entityId;
        cellPos = data.cellpos.ToVector3Int();
        lastDay = data.lastDay;
    }
    public abstract EntitySaveData ToSaveData();
    public abstract void LoadFromData(EntitySaveData data);
    public void DeleteEntity()
    {
        DeleteEntityEventSO.RaiseEvent(cellPos,this);
        Destroy(gameObject);
    }
}
