using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ItemUses/Bed")]
public class BedUse : ItemUseSO
{
    public CreateEntityEventSO createEntityEventSO;
    public override void Excute(PlayerController player, SlotUI slot, TileData tileData, Vector3Int tilePos, List<Entity> entityList)
    {
        if (entityList.Count > 0||!tileData.canPlace) return;
        EntitySaveData entity = new EntitySaveData
        {
            entityId = 200,
            dataId = 200,
            cellpos = new VectorIntData(tilePos),
            lastDay = GeneralDataLoader.instance.database.database.totalDay,
            stage = 0,
        };
        InventoryLoader.instance.ReduceItem(slot.index, 1);
        createEntityEventSO.RaiseEvent(tilePos, 200, entity);
    }
    public override void ExcuteAnimate(PlayerController player)
    {
        player.OnUseItemAnimationEvent();
        if (player.curTilePos.x < player.transform.position.x)
        {
            player.sr.flipX = true;
        }
        else
        {
            player.sr.flipX = false;
        }
    }
}
