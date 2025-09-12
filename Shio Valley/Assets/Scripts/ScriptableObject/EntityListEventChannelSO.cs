using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName ="Event/EntityListEventChannelSO")]
public class EntityListEventChannelSO : ScriptableObject
{
    public UnityAction<Vector3Int,UnityAction<List<Entity>>> OnRequest;
    public void RaiseEvent(Vector3Int pos, UnityAction<List<Entity>> callback)
    {
        OnRequest?.Invoke(pos, callback); 
    }
}
