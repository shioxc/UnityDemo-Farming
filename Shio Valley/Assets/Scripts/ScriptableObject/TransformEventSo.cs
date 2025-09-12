using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName ="Event/TransformEventSO")]
public class TransformEventSo : ScriptableObject
{
    public UnityAction<Transform> OnEventRaised;
    public void RaiseEvent(Transform transform)
    {
        OnEventRaised?.Invoke(transform);
    }
}
