using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class InitialLoad : MonoBehaviour
{
    public AssetReference IniScene;
    private void Awake()
    {
        Addressables.LoadSceneAsync(IniScene);
    }
}
