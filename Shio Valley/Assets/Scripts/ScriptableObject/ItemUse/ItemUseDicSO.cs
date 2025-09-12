using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemUseSO : ScriptableObject
{
    public abstract void Excute(PlayerController player,SlotUI slot,TileData tileData,Vector3Int tilePos,List<Entity> entityList);
    public abstract void ExcuteAnimate(PlayerController player);
}
[System.Serializable]
public class ItemUseDic
{
    public ItemEnum itemType;
    public ItemUseSO useLogic;
}
[CreateAssetMenu(menuName = "ItemUses/DicManager")]
public class ItemUseDicSO : ScriptableObject
{
    public List<ItemUseDic> dics;
    private Dictionary<ItemEnum, ItemUseSO> lookup;
    public void Init()
    {
        lookup = new Dictionary<ItemEnum, ItemUseSO>();
        foreach(var dic in dics)
        {
            lookup[dic.itemType] = dic.useLogic;
        }
    }

    public ItemUseSO GetUseLogic(ItemEnum itemType)
    {
        if (lookup == null) Init();
        lookup.TryGetValue(itemType, out var logic);
        return logic;
    }
}
