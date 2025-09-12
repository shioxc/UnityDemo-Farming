using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BedEntity : Entity,IAxeInteractable,IPickaxeInteractable,IBreakable,IInteractable
{
    public VoidEventSO OnSleepEventSO;
    public override void Initialized(EntitySaveData data)
    {
        base.Initialized(data);
        LoadFromData(data);
    }

    public override EntitySaveData ToSaveData()
    {
        return new EntitySaveData
        {
            entityId = entityId,
            dataId = data.id,
            type = type,
            stage = stage,
            lastDay = lastDay,
            cellpos = new VectorIntData(cellPos),
        };
    }
    public override void LoadFromData(EntitySaveData data)
    {
        SetStage(data.stage);
    }

    public void InteractWithPickaxe(int damage)
    {
        Break();
    }
    public void InteractWithAxe(int damage)
    {
        Break();
    }
    public void Break()
    {
        EntityDropItemEventSO.RaiseEvent(cellPos,this);
        DeleteEntity();
    }
    public void Interact(PlayerController player, SlotUI slot, TileData tileData, Vector3Int tilePos)
    {
        OnSleepEventSO.RaiseEvent();
    }
}
