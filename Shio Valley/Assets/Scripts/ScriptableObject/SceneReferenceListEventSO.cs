using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

[CreateAssetMenu(menuName ="Event/SceneReferenceListEventSO")]
public class SceneReferenceListEventSO :ScriptableObject
{
    public UnityAction<List<AssetReference>> onEventRaised;
    public void RaiseEvent(List<AssetReference> refs)
    {
        onEventRaised?.Invoke(refs); 
    }
}
