using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class TimeManager : MonoBehaviour
{
    public PlayerController player;
    public float hour;
    public float minute;

    public float timeSpeed;//timeSpeed分钟24小时,默认15
    public VoidEventSO NextDayEventSO;
    public Image fadeImage;
    public float fadeDuration = 0.2f;

    public bool test;
    private float testSpeed=24f;
    private bool isNextDayRunning = false;
    public VoidEventSO AfterDayChangedEventSO;
    public VoidEventSO EntityUpdateEventSO;
    public VoidEventSO OnSleepEventSO;

    public static TimeManager instance { get;private set;}

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        instance.hour = hour;
        instance.minute = minute;
        timeSpeed = 24f / timeSpeed;
    }
    private void OnEnable()
    {
        OnSleepEventSO.onEventRaised += SleepToNextDay;
    }
    private void OnDisable()
    {
        OnSleepEventSO.onEventRaised -= SleepToNextDay;
    }
    private void Update()
    {
        if (test)
        {
            instance.minute += Time.deltaTime * testSpeed;//半分钟1天
        }
        else instance.minute += Time.deltaTime * timeSpeed;
        if(instance.minute >= 60)
        {
            instance.minute -= 60;
            instance.hour++;
        }
        if (instance.hour >= 23)
        {
            instance.hour = 0;
        }
        if (!isNextDayRunning && instance.hour >= 2 && instance.hour < 6)
        {
            StartCoroutine(NextDay());
        }
    }

    private void SleepToNextDay()
    {
        player.inputControl.Disable();
        IniPlayer();
        StartCoroutine(NextDay());
    }

    private void IniPlayer()
    {
        PlayerController controller = player.GetComponent<PlayerController>();
        controller.canUse = true;
    }
    

    private IEnumerator NextDay()
    {
        isNextDayRunning = true;

        player.inputControl.Disable();
        Time.timeScale = 0f;

        yield return StartCoroutine(FadeIn());

        NextDayEventSO.RaiseEvent();
        instance.hour = 6f;
        instance.minute = 0f;
        EntityUpdateEventSO.RaiseEvent();
        AfterDayChangedEventSO.RaiseEvent();

        yield return StartCoroutine(FadeOut());

        player.inputControl.Enable();
        Time.timeScale = 1f;

        isNextDayRunning = false;
    }
    private IEnumerator FadeIn()
    {
        float t = 0;
        Color c = fadeImage.color;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            c.a = Mathf.Clamp01(t / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }
    }

    private IEnumerator FadeOut()
    {
        float t = 0;
        Color c = fadeImage.color;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            c.a = 1f - Mathf.Clamp01(t / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }
    }
}
