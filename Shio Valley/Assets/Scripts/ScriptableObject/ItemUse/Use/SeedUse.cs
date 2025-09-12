using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

[CreateAssetMenu(menuName ="ItemUses/Seed")]
public class SeedUse : ItemUseSO
{
    public CreateEntityEventSO createEntityEventSO;
    public override void ExcuteAnimate(PlayerController player)
    {
        player.OnUseItemAnimationEvent();
    }
    public override void Excute(PlayerController player, SlotUI slot, TileData tileData, Vector3Int tilePos, List<Entity> entityList)
    {
        if(entityList.Count == 1)
        {
            Entity entity = entityList[0];
            if (entity.GetComponent<SoilEntity>() == null) return;
            SoilEntity soilEntity = entity.GetComponent<SoilEntity>();
            soilEntity.isPlanted = true;
            if (soilEntity != null && slot.item.canUsedinseason[GeneralDataLoader.instance.database.database.season])
            {
                EntitySaveData saveData = new EntitySaveData
                {
                    entityId = 4,
                    dataId = ItemUseManager.GetEntityDataId(slot.item.id),
                    stage = 0,
                    lastDay = GeneralDataLoader.instance.database.database.totalDay,
                    cellpos = new VectorIntData(tilePos),
                    extra = new Dictionary<string, object>
                    {
                        {"duration",0 },
                        {"isWater",entity.GetComponent<SoilEntity>().isWater },
                        {"keepWater",entity.GetComponent<SoilEntity>().keepWater },
                        {"spring",slot.item.canUsedinseason[0]},
                        {"summer",slot.item.canUsedinseason[1]},
                        {"autumn",slot.item.canUsedinseason[2]},
                        {"winter",slot.item.canUsedinseason[3]}
                    }
                };
                InventoryLoader.instance.ReduceItem(slot.index, 1);
                createEntityEventSO.RaiseEvent(tilePos, 4, saveData);
            }
        }
    }
}
