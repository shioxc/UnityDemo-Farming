using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName ="Event/Vector3IntIntEventSO")]
public class Vector3IntIntEventSO : ScriptableObject
{
    public UnityAction<Vector3, int, int> OnEventRaised;
    public void RaiseEvent(Vector3 pos,int value1,int value2)
    {
        OnEventRaised?.Invoke(pos,value1,value2);
    }
}
