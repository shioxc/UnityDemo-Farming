using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "SceneMapping/SceneMapping")]
public class SceneMappingSO : ScriptableObject
{
    [System.Serializable]
    public class SceneEntry
    {
        public string key;
        public AssetReference sceneRef;
    }

    public List<SceneEntry> scenes;
    private Dictionary<string, AssetReference> cache;

    public void Initialize()
    {
        cache = new Dictionary<string, AssetReference>();
        foreach (var entry in scenes)
            cache[entry.key] = entry.sceneRef;
    }

    public AssetReference GetSceneRef(string key)
    {
        if (cache == null) Initialize();
        cache.TryGetValue(key, out var sceneRef);
        return sceneRef;
    }
}