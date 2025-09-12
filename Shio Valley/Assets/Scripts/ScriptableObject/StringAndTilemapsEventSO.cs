using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Event/StringAndTilemapsEventSO")]
public class StringAndTilemapsEventSO : ScriptableObject
{
    public UnityAction<string, Tilemap[]> OnEvnetRaised;
    public void RaiseEvent(string mapName, Tilemap[] tilemaps)
    {
        OnEvnetRaised?.Invoke(mapName, tilemaps);
    }
}
