using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public static class SpriteCache
{
    private static Dictionary<string, Sprite> spriteCache = new Dictionary<string, Sprite>();
    public static void GetSprite(string key, System.Action<Sprite> callback)
    {
        if (spriteCache.TryGetValue(key, out Sprite cachedSprite))
        {
            callback?.Invoke(cachedSprite);
            return;
        }

        Addressables.LoadAssetAsync<Sprite>(key).Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                Sprite sprite = handle.Result;
                spriteCache[key] = sprite;
                callback?.Invoke(sprite);
            }
            else
            {
                Debug.LogWarning($"Failed to load Sprite: {key}");
                callback?.Invoke(null);
            }
            Addressables.Release(handle);
        };
    }
}
