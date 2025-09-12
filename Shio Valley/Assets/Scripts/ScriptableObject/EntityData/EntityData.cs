using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class EntityStagesData
{
    public Sprite sprite;
    public int duration;//阶段天数  
}
[System.Serializable]
public class EntityLoot
{
    public int stage;
    public int itemId;
    public int num;
}
[CreateAssetMenu(menuName ="Entity/EntityDataSO")]
public class EntityData : ScriptableObject
{
    public int id;
    public string entityName;
    public int row;//行
    public int col;//列
    public List<EntityStagesData> stages;
    public List<EntityLoot> loots;
}
