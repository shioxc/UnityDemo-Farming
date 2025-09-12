using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{
    public int id;
    public string name;
    public int price;
    public string iconKey;
    public ItemEnum type;
    public string description;
    public bool canUse;
    public bool canCustomized;
    public bool canEat;
    public int maxStack;
    public List<bool> canUsedinseason;
}

[Serializable]
public class ItemDatabase
{
    public List<Item> items;
}
