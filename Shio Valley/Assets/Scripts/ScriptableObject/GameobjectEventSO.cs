using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/GameObjectEventSO")]
public class GameobjectEventSO : ScriptableObject
{
    public UnityAction<GameObject> OnEventRaised;
    public void RaiseEvent(GameObject obj)
    {
        OnEventRaised?.Invoke(obj); 
    }
}
