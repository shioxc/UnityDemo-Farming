using System.Collections.Generic;
using UnityEngine;
public static class ItemUseManager
{
    private static ItemUseDicSO dic;
    private static ItemIdToEntityDataIdDic entityDataDic;
    private static EntityListEventChannelSO GetEntityListEventSO;
    private static List<Entity> entityList;
    public static void Initialize(ItemUseDicSO dicSO,EntityListEventChannelSO entityListEventChannelSO,ItemIdToEntityDataIdDic entityDicSO)//在DataManager中的Awake调用进行初始化
    {
        dic = dicSO;
        entityDataDic = entityDicSO;
        GetEntityListEventSO = entityListEventChannelSO;
        dic.Init();
        entityDataDic.Init();
    }
    public static void UseItem(PlayerController player,SlotUI slot,TileData tileData,Vector3Int tilePos)
    {
        if (slot.item == null) return;
        var logic = dic?.GetUseLogic(slot.item.type);
        if (logic != null)
        {
            GetEntityListEventSO.RaiseEvent(tilePos,GetEntityList);
            logic.Excute(player, slot,tileData,tilePos,entityList);
        }
    }
    public static void UseAnimate(PlayerController player,SlotUI slot)
    {
        if (slot.item == null) return;
        var logic = dic?.GetUseLogic(slot.item.type);
        if (logic != null)
        {
            logic.ExcuteAnimate(player);
        }
    }
    public static int GetEntityDataId(int itemId)
    {
        if(entityDataDic == null) return 0;
        entityDataDic.lookup.TryGetValue(itemId, out var dataId);
        return dataId;
    }
    private static void GetEntityList(List<Entity> entities)
    {
        entityList = entities;
    }
    public static void Interact(PlayerController player,SlotUI slot, TileData tileData, Vector3Int tilePos)
    {
        GetEntityListEventSO.RaiseEvent(tilePos, GetEntityList);
        foreach (var entity in entityList)
        {
            IInteractable interactable = entity.GetComponent<IInteractable>();
            if(interactable != null)
            {
                interactable.Interact(player, slot, tileData, tilePos);
                break;
            }
        }
    }
}
