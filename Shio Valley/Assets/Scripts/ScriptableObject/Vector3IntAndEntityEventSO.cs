using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/Vector3IntAndEntityEventSO")]
public class Vector3IntAndEntityEventSO : ScriptableObject
{
    public UnityAction<Vector3Int, Entity> OnEventRaised;
    public void RaiseEvent(Vector3Int pos,Entity entity)
    {
        OnEventRaised?.Invoke(pos, entity);
    }
}
