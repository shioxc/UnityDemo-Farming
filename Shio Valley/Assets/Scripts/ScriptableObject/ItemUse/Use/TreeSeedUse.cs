using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

[CreateAssetMenu(menuName ="ItemUses/TreeSeed")]
public class TreeSeedUse : ItemUseSO
{
    public CreateEntityEventSO createEntityEventSO;
    public override void ExcuteAnimate(PlayerController player)
    {
        player.OnUseItemAnimationEvent();
    }
    public override void Excute(PlayerController player, SlotUI slot, TileData tileData, Vector3Int tilePos, List<Entity> entityList)
    {
        if (!tileData.canPlant) return;
        if ((entityList.Count == 1 && entityList[0].GetComponent<SoilEntity>()!=null)||entityList.Count == 0)
        {
            if(entityList.Count ==1 )
            {
                entityList[0].GetComponent<SoilEntity>().Break();
            }
            EntitySaveData saveData = new EntitySaveData
            {
                entityId = 1,
                dataId = ItemUseManager.GetEntityDataId(slot.item.id),
                cellpos = new VectorIntData(tilePos),
                lastDay = GeneralDataLoader.instance.database.database.totalDay,
                stage = 0,
                extra = new Dictionary<string, object>
                {
                    {"duration",0 }
                }
            };
            InventoryLoader.instance.ReduceItem(slot.index, 1);
            createEntityEventSO.RaiseEvent(tilePos, 1, saveData);
        }
    }
}
