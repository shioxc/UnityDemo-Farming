using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.Mathematics;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;

public class CropEntity : Entity,IGrowable,IWaterable,IAxeInteractable,IPickaxeInteractable,IBreakable,IInteractable
{
    public int duration;
    public bool isWater;
    public bool keepWater;
    public List<bool> season;

    public override void Initialized(EntitySaveData data)
    {
        base.Initialized(data);
        LoadFromData(data);
    }
    public override void EntityUpdate()
    {
        if (lastDay >= GeneralDataLoader.instance.database.database.totalDay) return;
        grow();
        KeepWater();

        base.EntityUpdate();
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
            extra = new Dictionary<string, object>
            {
                {"duration",duration },
                {"isWater",isWater },
                {"keepWater",keepWater },
                {"spring",season[0]},
                {"summer",season[1]},
                {"autumn",season[2]},
                {"winter",season[3]}
            }
        };
    }
    public override void LoadFromData(EntitySaveData saveData)
    {
        data = EntityDatabase.GetEntityData(saveData.dataId);
        for (int i = 0; i < 4; i++)
            season.Add(false); 
        if(saveData.extra.TryGetValue("duration",out var _duration))
        {
            duration = Convert.ToInt32(_duration);
        }
        if (saveData.extra.TryGetValue("isWater", out var _isWater))
        {
            isWater = Convert.ToBoolean(_isWater);
        }
        if (saveData.extra.TryGetValue("keepWater", out var _keepWater))
        {
            keepWater = Convert.ToBoolean(_keepWater);
        }
        if(saveData.extra.TryGetValue("spring",out var _spring))
        {
            season[0]=Convert.ToBoolean(_spring);
        }
        if (saveData.extra.TryGetValue("summer", out var _summer))
        {
            season[1] = Convert.ToBoolean(_summer);
        }
        if (saveData.extra.TryGetValue("autumn", out var _autumn))
        {
            season[2] = Convert.ToBoolean(_autumn);
        }
        if (saveData.extra.TryGetValue("winter", out var _winter))
        {
            season[3] = Convert.ToBoolean(_winter);
        }

        SetStage(saveData.stage);
    }
    public void grow()
    {
        int nowDay = GeneralDataLoader.instance.database.database.totalDay;
        int _season;
        int day = lastDay;
        while(day<=nowDay)
        {
            _season = (day % 72 - 1) / 28;
            if (!season[_season]) Break();
            int t = 29 - day % 72 % 28;
            day += t;
        }
        if (!isWater)return;
        
        if(!keepWater)
        {
            duration++;
            if (data.stages[stage].duration <= duration && stage != data.stages.Count - 1)
            {
                duration -= data.stages[stage].duration;
                if (stage + 1 <= data.stages.Count - 1)
                {
                    SetStage(stage + 1);
                }
            }
        }
        else
        {
            if (lastDay >= nowDay || stage == data.stages.Count - 1) return;
            duration = nowDay - lastDay + duration;
            for (int i = stage; i < data.stages.Count; i++)
            {
                if (data.stages[i].duration <= duration && i != data.stages.Count - 1)
                {
                    duration -= data.stages[i].duration;
                }
                else
                {
                    if (i != stage)
                    {
                        SetStage(i);
                    }
                    break;
                }
            }
        }
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
    public void Dry()
    {
        isWater = false;
    }
    public void Watered()
    {
        isWater = true;
    }

    public void InteractWithAxe(int damage)
    {
        Break();
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
    public void Interact(PlayerController player, SlotUI slot, TileData tileData, Vector3Int tilePos)
    {
        if(stage == data.stages.Count-1)
            Break();
    }
}
