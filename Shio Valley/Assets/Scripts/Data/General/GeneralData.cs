using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GeneralData
{
    public int totalDay;//������
    public int playTime;//ʱ�� ����
    public int gold;//���
    public int day;//1~28
    public int season;//0~3
    public int date;//1~7
    public float x,y,z;
}
[System.Serializable]
public class GeneralDatabase
{
    public GeneralData database;
}
