using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ItemUses/Pickaxe")]
public class PickaxeUse : ItemUseSO
{
    public override void ExcuteAnimate(PlayerController player)
    {
        player.GetComponent<PlayerAnimation>().SetUseAnimation("UsePickaxe");
        if (player.curTilePos.x < player.transform.position.x)
        {
            player.sr.flipX = true;
        }
        else
        {
            player.sr.flipX = false;
        }
    }
    public override void Excute(PlayerController player,SlotUI slot,TileData tileData,Vector3Int pos,List<Entity> entityList) 
    {
        for (int i = entityList.Count - 1; i >= 0; i--)
        {
            var entity = entityList[i];
            IPickaxeInteractable pickaxeInteractable = entity.GetComponent<IPickaxeInteractable>();
            if (pickaxeInteractable != null)
            {
                pickaxeInteractable.InteractWithPickaxe(20);
                break;
            }
        }
    }
}
