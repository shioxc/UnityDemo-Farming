using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileTip : MonoBehaviour
{
    public void SetProfile(Item item)
    {
        if (item == null) return;
        TMP_Text itemName;
        TMP_Text description;
        TMP_Text price;
        itemName = transform.Find("middle").Find("ItemName").GetComponent<TMP_Text>();
        description = transform.Find("middle").Find("Description").GetComponent<TMP_Text>();
        price = transform.Find("middle").Find("Price").Find("PriceText").GetComponent<TMP_Text>();
        itemName.text = item.name;
        description.text = item.description;
        price.text = $"{item.price}";
    }
    public void RefreshUI()
    {
        StartCoroutine(RefreshLater());
       
    }
    IEnumerator RefreshLater()
    {
        yield return null;
        TMP_Text[] texts = transform.Find("middle").GetComponentsInChildren<TMP_Text>();
        foreach (var t in texts)
        {
            t.ForceMeshUpdate();
        }
        GameObject middle = transform.Find("middle").gameObject;
        RectTransform left = transform.Find("left").gameObject.GetComponent<RectTransform>();
        RectTransform right = transform.Find("right").gameObject.GetComponent<RectTransform>();
        Vector2 size = left.sizeDelta;
        RectTransform middleRect = middle.GetComponent<RectTransform>();

        LayoutRebuilder.ForceRebuildLayoutImmediate(middle.GetComponent<RectTransform>());
        float newHeight = 0;
        if (middleRect != null)
        {
            newHeight = middleRect.rect.height;
        }
        VerticalLayoutGroup layout = middle.GetComponent<VerticalLayoutGroup>();
        int pad = (int)((newHeight - layout.padding.top - layout.padding.bottom) * 2 / 9);
        layout.padding.top = pad;
        layout.padding.bottom = pad;

        LayoutRebuilder.ForceRebuildLayoutImmediate(middle.GetComponent<RectTransform>());
        newHeight = middleRect.rect.height;
        size.y = newHeight;
        left.sizeDelta = size;
        size = right.sizeDelta;
        size.y = newHeight;
        right.sizeDelta = size;

        LayoutRebuilder.ForceRebuildLayoutImmediate(middle.GetComponent<RectTransform>());
        yield return null;
        LayoutRebuilder.ForceRebuildLayoutImmediate(gameObject.GetComponent<RectTransform>());
    }
}
