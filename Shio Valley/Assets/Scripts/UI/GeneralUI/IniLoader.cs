using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class IniLoader : MonoBehaviour
{
    public List<string> menu;
    public StringListEventSO LoadEventSO;

    private void Start()
    {
        LoadEventSO.RaiseEvent(menu);
    }
}
