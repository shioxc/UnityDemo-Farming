using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public class TimeAndGoldUI : MonoBehaviour
{
    public TMP_Text date;
    public TMP_Text time;
    public TMP_Text gold;

    private Dictionary<int,string> dateDic = new Dictionary<int,string>();
    private Dictionary<int,string> SeasonDic = new Dictionary<int,string>();
    int oldMinute = 0;
    private void Awake()
    {
        dateDic[1] = "Ò»";
        dateDic[2] = "¶þ";
        dateDic[3] = "Èý";
        dateDic[4] = "ËÄ";
        dateDic[5] = "Îå";
        dateDic[6] = "Áù";
        dateDic[7] = "Æß";
        SeasonDic[0] = "´º¼¾";
        SeasonDic[1] = "ÏÄ¼¾";
        SeasonDic[2] = "Çï¼¾";
        SeasonDic[3] = "¶¬¼¾";
    }
    private void Update()
    {
        date.text = $"{SeasonDic[GeneralDataLoader.instance.database.database.season]} {GeneralDataLoader.instance.database.database.day}ÈÕ ÐÇÆÚ{dateDic[GeneralDataLoader.instance.database.database.date]}";
        int hour = (int)TimeManager.instance.hour;
        int minute = (int)TimeManager.instance.minute;
        if(Math.Abs(minute - oldMinute) >= 10)
        {
            oldMinute = minute;
        }
        time.text = $"{hour}:{oldMinute:D2}";
        gold.text = $"{GeneralDataLoader.instance.database.database.gold}";
    }
}
