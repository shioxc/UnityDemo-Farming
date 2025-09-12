using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GoodSlot : MonoBehaviour,IPointerDownHandler,IPointerUpHandler,IPointerEnterHandler,IPointerExitHandler
{
    private Goods good;
    public float initialduration = 0.5f;
    public float repeatDuration = 0.1f;

    private bool isBuying;
    private float timer = 0f;

    public Image background;
    public TMP_Text goodName;
    public TMP_Text priceText;
    public Image icon;
    private void Update()
    {
        timer -= Time.unscaledDeltaTime;
        if(timer<=0f&&isBuying)
        {
            Buy();
        }
    }
    private void OnDisable()
    {
        Destroy(this.gameObject);
    }
    public void Initialized(Goods _good)
    {
        good = _good;
        Item item = ItemLoader.GetItemById(good.itemId);
        goodName.text = $"{item.name}x{good.num}";
        priceText.text = $"{good.price}";
        SpriteCache.GetSprite(item.iconKey, sprite =>
        {
            if(icon != null)
            {
                icon.sprite = sprite;
            }
        });
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        timer = initialduration;
        isBuying = true;
        Buy();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isBuying=false;
    }
    private void Buy()
    {
        if (GeneralDataLoader.instance.database.database.gold < good.price) return;
        if(InventoryLoader.instance.CheckBag(good.itemId,good.num))
        {
            GeneralDataLoader.instance.database.database.gold-=good.price;
            InventoryLoader.instance.AddItem(good.itemId,good.num);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        background.color = new Color(0.8f,0.8f,0.8f,1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        background.color = Color.white;
    }
}
