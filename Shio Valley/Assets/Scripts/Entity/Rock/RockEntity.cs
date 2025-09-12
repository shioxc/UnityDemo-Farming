using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockEntity : Entity,IPickaxeInteractable,IHurtable,IBreakable
{
    public int hp;

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
            extra = new Dictionary<string, object>
            {
                {"hp",hp }
            }
        };
    }
    public override void LoadFromData(EntitySaveData data)
    {

        if(data.extra.TryGetValue("hp",out var _hp))
        {
            hp = Convert.ToInt32(_hp);
        }
        SetStage(data.stage);
    }

    public void InteractWithPickaxe(int damage)
    {
        TakeDamage(damage);
    }

    public void TakeDamage(int damage)
    {
        hp-= damage;
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
