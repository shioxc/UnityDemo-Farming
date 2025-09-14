using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoilEntity : Entity,IWaterable,IPickaxeInteractable,IBreakable
{
    public bool isWater;//trueÎª1£¬falseÎª0
    public bool keepWater;
    public bool isPlanted;
    public int duration;
    public override void Initialized(EntitySaveData saveData)
    {
        base.Initialized(saveData);
        LoadFromData(saveData);
    }
    public override void EntityUpdate()
    {
        if (lastDay >= GeneralDataLoader.instance.database.database.totalDay) return;
        KeepWater();
        base.EntityUpdate();
    }
    public override void SetStage(int _stage)
    {
        base.SetStage(_stage);
        if(stage == 1)
        {
            sr.color = new Color(0.7f, 0.7f, 0.7f, 1f);
        }
        else
        {
            sr.color = new Color(1f,1f, 1f, 1f);
        }
    }

    public override EntitySaveData ToSaveData()
    {
        return new EntitySaveData
        {
            entityId = entityId,
            dataId = data.id,
            type = type,
            cellpos = new VectorIntData(cellPos),
            lastDay = lastDay,
            stage = stage,
            extra = new Dictionary<string, object>
            {
                {"isWater",isWater},
                {"isPlanted",isPlanted },
                {"duration",duration },
                {"keepWater",keepWater }
            }
        };
    }
    public override void LoadFromData(EntitySaveData data)
    {

        if(data.extra.TryGetValue("isWater",out var _isWater))
        {
            isWater = Convert.ToBoolean(_isWater);
        }
        if (data.extra.TryGetValue("isPlanted", out var _isPlanted))
        {
            isPlanted = Convert.ToBoolean(_isPlanted);
        }
        if (data.extra.TryGetValue("duration", out var _duration))
        {
            duration = Convert.ToInt32(_duration);
        }
        if(data.extra.TryGetValue("keepWater",out var _keepWater))
        {
            keepWater = Convert.ToBoolean(_keepWater);
        }
        SetStage(data.stage);
    }
    public void KeepWater()
    {
        if(keepWater)
        {
            Watered();
        }
        else
        {
            Dry();
        }
    }
    public void Watered()
    {
        isWater = true;
        SetStage(1);
    }

    public void Dry()
    {
        if (isWater&&!keepWater)
        {
            isWater = false;
            SetStage(0);
        }
        else if(!isWater&&!isPlanted)
        {
            duration++;
            if(duration>=2)
            {
                Break();
            }
            return;
        }
        duration = 0;
    }

    public void InteractWithPickaxe(int damage)
    {
        Break();
    }

    public void Break()
    {
        EntityDropItemEventSO.RaiseEvent(cellPos, this);
        DeleteEntity();
    }
}
