using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour
{
    public Item item;
    public int num;
    public int index;//µÚindex¸ñ
    private void Update()
    {
        if(num == 0)
        {
            item = null;
            transform.Find("ItemIcon").gameObject.SetActive(false);
            transform.Find("Num").gameObject.SetActive(false);
        }
    }
}
