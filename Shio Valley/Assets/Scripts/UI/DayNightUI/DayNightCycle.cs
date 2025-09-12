using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayNightCycle : MonoBehaviour
{
    Image sky;
    private bool sleep;
    public VoidEventSO OnSleepEventSO;
    private void Awake()
    {
        sky = GetComponent<Image>();
    }
    private void OnEnable()
    {
        OnSleepEventSO.onEventRaised += Fade;
    }
    private void OnDisable()
    {
        OnSleepEventSO.onEventRaised -= Fade;
    }
    private void Update()
    {
        if(!sleep)
        {
            SkyChange();
        }
    }
    private void Fade()
    {
        sky.color = sky.color = new Color(0f, 0f, 0f, 0.9f);
    }
    private void SkyChange()
    {
        float curHour = TimeManager.instance.hour;
        float curMinute = TimeManager.instance.minute;
        if (6 <= curHour && curHour < 12)
        {
            float _alpha = (720 - curHour * 60 - curMinute) / 12000;
            if (_alpha < 0) _alpha = 0f;
            sky.color = new Color(1f, 1f, 1f, _alpha);
        }
        else if (curHour >= 12 && curHour < 15)
        {
            sky.color = new Color(1f, 1f, 1f, 0f);
        }
        else if (curHour >= 15 && curHour < 18)
        {
            float _alpha = (curHour * 60 + curMinute - 900) / 360;
            sky.color = new Color(0.4f, 0f, 0f, _alpha);
        }
        else if (curHour >= 18 && curHour < 21)
        {
            float _alpha = 0.5f + (curHour * 60 + curMinute - 1080) / 450;
            float r = 0.4f - (curHour * 60 + curMinute - 1080) / 450;
            sky.color = new Color(r, 0f, 0f, _alpha);
        }
        else if (curHour >= 21 || curHour < 3)
        {
            sky.color = new Color(0f, 0f, 0f, 0.9f);
        }
        else if (curHour >= 3 && curHour < 6)
        {
            Color start = new Color(0f, 0f, 0f, 0.9f);

            Color end = new Color(1f, 1f, 1f, 0.03f);

            float t = (curHour * 60 + curMinute - 120) / 240;
            sky.color = Color.Lerp(start, end, t);
        }
    }
}
