using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName ="Event/IntAndIntEventSO")]
public class IntAndIntEventSO : ScriptableObject
{
    public UnityAction<int, int> OnEventRaised;
    public void RaiseEvent(int t0,int t1)
    {
        OnEventRaised.Invoke(t0,t1); 
    }
}
