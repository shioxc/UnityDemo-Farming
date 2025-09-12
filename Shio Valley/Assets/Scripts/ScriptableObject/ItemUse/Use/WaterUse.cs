using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ItemUses/Water")]
public class WaterUse : ItemUseSO
{
    public override void Excute(PlayerController player, SlotUI slot, TileData tileData, Vector3Int tilePos, List<Entity> entityList)
    {
        foreach(var entity in entityList)
        {
            IWaterable waterable = entity.GetComponent<IWaterable>();
            if (waterable != null)
            {
                waterable.Watered();
            }
        }
    }
    public override void ExcuteAnimate(PlayerController player)
    {
        player.GetComponent<PlayerAnimation>().SetUseAnimation("UseWater");
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
