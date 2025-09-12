using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Inventory
{
    public int itemId;
    public int storeNum;
}
[Serializable]
public class InventoryDatabase
{
    public List<Inventory> inventories;
}
