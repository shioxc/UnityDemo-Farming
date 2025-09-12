using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName = "Event/Vector3AndEntityEventSO")]
public class Vector3AndEntityEventSO : ScriptableObject
{
    public UnityAction<Vector3, Entity> OnEventRaised;
    public void RaiseEvent(Vector3 pos,Entity entity)
    {
        OnEventRaised?.Invoke(pos, entity);
    }
}
