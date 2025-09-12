using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPicker : MonoBehaviour
{
    public void PickupItem(int itemId,int num)
    {
        InventoryLoader.instance.AddItem(itemId, num);
    }
}
