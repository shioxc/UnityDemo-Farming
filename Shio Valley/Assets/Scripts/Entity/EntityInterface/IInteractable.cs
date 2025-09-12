using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void Interact(PlayerController player, SlotUI slot, TileData tileData, Vector3Int tilePos);
}
