using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName ="Event/StringListEventSO")]
public class StringListEventSO : ScriptableObject
{
    public UnityAction<List<string>> onEventRaised;
    public void RaiseEvent(List<string> list)
    {
        onEventRaised?.Invoke(list); 
    }
}
