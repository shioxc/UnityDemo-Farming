using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeEntity : Entity,IGrowable,IAxeInteractable,IHurtable,IBreakable
{
    public int maxHp;
    public int hp;
    public int duration;//当前阶段已过天数
    public List<GameObject> col;
    public override void Initialized(EntitySaveData data)
    {
        base.Initialized(data);
        LoadFromData(data);
    }
    public override void SetStage(int _stage)
    {
        col[stage].SetActive(false);
        base.SetStage(_stage);
        col[stage].SetActive(true);
        return;
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
                {"hp",hp },
                {"duration",duration }
            }
        };
    }
    public override void LoadFromData(EntitySaveData data)
    {
        if (data.extra.TryGetValue("hp", out var _hp))
        {
            long t = (long)_hp;
            hp = (int)t;
        }
        else
        {
            hp = maxHp;
        }
        if(data.extra.TryGetValue("duration",out var _duration))
        {
            duration = Convert.ToInt32(_duration); 
        }
        SetStage(data.stage);
    }
    public override void EntityUpdate()
    {
        if (lastDay >= GeneralDataLoader.instance.database.database.totalDay) return;
        grow();
        base.EntityUpdate();//最后
    }

    public void grow()
    {
        int nowDay = GeneralDataLoader.instance.database.database.totalDay;
        int growthDays = 0;
        int fullYears = (nowDay / 112) - (lastDay / 112) - 1;
        if (fullYears >= 0)
        {    
            growthDays += fullYears * 84;
            int startYearDay = lastDay % 112;
            growthDays += Math.Max(0, 84 - startYearDay);
            int endYearDay = nowDay % 112;
            growthDays += Math.Min(endYearDay, 84);
        }
        else
        {
            growthDays += Math.Max(0,Math.Min(84, nowDay % 112) - lastDay % 112);
        }

        duration += growthDays;
        for (int i = stage;i<data.stages.Count;i++)
        {
            if(data.stages[i].duration <= duration && i!=data.stages.Count-1)
            {
                duration -= data.stages[i].duration;
            }
            else
            {
                if (i != stage)
                {
                    hp = maxHp;
                    SetStage(i);
                }
                break;
            }
        }
    }

    public void InteractWithAxe(int damage)
    {
        TakeDamage(damage);
    }

    public void TakeDamage(int damage)
    {
        hp -= damage*(data.stages.Count-stage);
        StartCoroutine(Fade());
        if (hp <= 0)
        {
            Break();
        }
    }

    public void Break()
    {
        EntityDropItemEventSO.RaiseEvent(transform.position,GetComponent<Entity>());
        DeleteEntity();
    }
    private IEnumerator Fade()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color startColor = new Color(1f, 1f, 1f, 0.6f);
        Color endColor = sr.color;

        float duration = 0.5f;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            sr.color = Color.Lerp(startColor, endColor, t / duration);
            yield return null;
        }

        sr.color = Color.white;
    }
}
