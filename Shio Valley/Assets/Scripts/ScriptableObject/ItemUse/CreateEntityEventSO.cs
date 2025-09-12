using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName ="ItemUses/CreateEntityEventSO")]
public class CreateEntityEventSO : ScriptableObject
{
    public UnityAction<Vector3Int, int, EntitySaveData> OnEventRaised;
    public void RaiseEvent(Vector3Int pos,int prefabId,EntitySaveData data)
    {
        OnEventRaised?.Invoke(pos, prefabId, data);
    }
}
