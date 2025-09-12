using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

public class SceneLoader : MonoBehaviour
{
    public bool first;
    public Image fadeImage;
    public float fadeDuration = 1f;
    public StringListEventSO LoadSceneEventSO;
    public StringListEventSO UnloadSceneEventSO;
    public SceneMappingSO SceneMappingSO;
    public List<string> SceneToUnload;
    private Dictionary<string, AsyncOperationHandle<SceneInstance>> currentHandle = new Dictionary<string, AsyncOperationHandle<SceneInstance>>();
    public VoidEventSO AfterLoadSceneEventSO;
    private void Awake()
    {
        first = true;
        DontDestroyOnLoad(gameObject);
    }
    private void OnEnable()
    {
        LoadSceneEventSO.onEventRaised += LoadNewScene;
        UnloadSceneEventSO.onEventRaised += GetSceneToUnload;
    }
    private void OnDisable()
    {
        LoadSceneEventSO.onEventRaised -= LoadNewScene;
        UnloadSceneEventSO.onEventRaised -= GetSceneToUnload;
    }
    public async void LoadNewScene(List<string> scenes)
    {
        if(!first)
        await FadeIn();
        await DataManager.instance.WaitAll();
        foreach (var scene in SceneToUnload)
        {
            await UnloadScene(scene);
        }
        SceneToUnload = null;
        foreach (var scene in scenes)
        {
            await LoadSceneAsync(scene);
        }
        AfterLoadSceneEventSO.RaiseEvent();
        if(!first)
        await FadeOut();
        first = false;
    }
    private async Task LoadSceneAsync(string s)
    {
        AssetReference sceneToLoad = SceneMappingSO.GetSceneRef(s);
        if(currentHandle.ContainsKey(s))
        {
            return;
        }
        currentHandle[s] = sceneToLoad.LoadSceneAsync(LoadSceneMode.Additive, true);
        await currentHandle[s].Task;
        currentHandle[s].Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log($"场景 {handle.Result.Scene.name} 加载成功");
            }
        };
    }
    public void GetSceneToUnload(List<string> scenes)
    {
        SceneToUnload = scenes;
    }
    public async Task UnloadScene(string sceneToUnload)
    {
        if(currentHandle.ContainsKey(sceneToUnload))
        {
            await Addressables.UnloadSceneAsync(currentHandle[sceneToUnload]).Task;
            currentHandle.Remove(sceneToUnload);
        }
        else
        {
            Debug.Log("null");
        }
    }

    private async Task FadeIn()
    {
        float t = 0;
        Color c = fadeImage.color;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Clamp01(t / fadeDuration);
            fadeImage.color = c;
            await Task.Yield();
        }
    }

    private async Task FadeOut()
    {
        float t = 0;
        Color c = fadeImage.color;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = 1f - Mathf.Clamp01(t / fadeDuration);
            fadeImage.color = c;
            await Task.Yield();
        }
    }
}
