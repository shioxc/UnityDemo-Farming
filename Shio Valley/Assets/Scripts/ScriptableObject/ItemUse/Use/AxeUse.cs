using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[CreateAssetMenu(menuName = "ItemUses/Axe")]
public class AxeUse : ItemUseSO
{
    public override void ExcuteAnimate(PlayerController player)
    {
        player.GetComponent<PlayerAnimation>().SetUseAnimation("UseAxe");
        if (player.curTilePos.x < player.transform.position.x)
        {
            player.sr.flipX = true;
        }
        else
        {
            player.sr.flipX = false;
        }
    }
    public override void Excute(PlayerController player, SlotUI slot, TileData tileData,Vector3Int tilePos,List<Entity> entityList)
    {
        for(int i = entityList.Count-1;i>=0;i--)
        {
            var entity = entityList[i];
            IAxeInteractable axeInteractable = entity.GetComponent<IAxeInteractable>();
            if(axeInteractable != null )
            {
                axeInteractable.InteractWithAxe(20);
                break;
            }
        }
    }
}
