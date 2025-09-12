using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemToEntity
{
    public int itemId;
    public int dataId;
}

[CreateAssetMenu(menuName ="ItemToEntityDic")]
public class ItemIdToEntityDataIdDic : ScriptableObject
{
    public List<ItemToEntity> itemToEntityDic;
    public Dictionary<int, int> lookup;

    public void Init()
    {
        lookup = new Dictionary<int, int>();
        foreach (var item in itemToEntityDic)
        {
            lookup[item.itemId] = item.dataId;
        }
    }
}
