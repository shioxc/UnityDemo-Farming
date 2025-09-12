using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName ="ItemUses/Shovel")]
public class ShovelUse : ItemUseSO
{
    public CreateEntityEventSO createEntityEventSO;
    public override void ExcuteAnimate(PlayerController player)
    {
        player.GetComponent<PlayerAnimation>().SetUseAnimation("UseShovel");
        if (player.curTilePos.x < player.transform.position.x)
        {
            player.sr.flipX = true;
        }
        else
        {
            player.sr.flipX = false;
        }
    }
    public override void Excute(PlayerController player, SlotUI slot, TileData tileData, Vector3Int tilePos, List<Entity> entityList)
    {
        
        if (entityList.Count > 0||tileData==null||!tileData.canPlant) return;
        EntitySaveData entity = new EntitySaveData
        {
            entityId = 3,
            dataId = 3,
            cellpos = new VectorIntData(tilePos),
            lastDay = GeneralDataLoader.instance.database.database.totalDay,
            stage = 0,
            extra = new Dictionary<string, object>
            {
                {"isWater",false},
                {"isPlanted",false },
                {"duration",0 },
                {"keepWater",false }
            }
        };
        createEntityEventSO.RaiseEvent(tilePos,3,entity);
    }
}
