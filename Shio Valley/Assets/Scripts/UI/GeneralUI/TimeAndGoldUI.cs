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
        dateDic[1] = "һ";
        dateDic[2] = "��";
        dateDic[3] = "��";
        dateDic[4] = "��";
        dateDic[5] = "��";
        dateDic[6] = "��";
        dateDic[7] = "��";
        SeasonDic[0] = "����";
        SeasonDic[1] = "�ļ�";
        SeasonDic[2] = "�＾";
        SeasonDic[3] = "����";
    }
    private void Update()
    {
        date.text = $"{SeasonDic[GeneralDataLoader.instance.database.database.season]} {GeneralDataLoader.instance.database.database.day}�� ����{dateDic[GeneralDataLoader.instance.database.database.date]}";
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
