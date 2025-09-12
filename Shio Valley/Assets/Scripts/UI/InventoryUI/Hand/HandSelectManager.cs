using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandSelectManager : MonoBehaviour
{
    public GameObject selectedSlot;
    [SerializeField] private GameObject profile;
    public GameObject[] curSlots;

    public int curIndex;
    private void Start()
    {
        curIndex = 0;
    }

    private void Update()
    {
        ScrollSelect();
    }
    private void OnEnable()
    {
        SelectSlot(transform.GetChild(curIndex).gameObject);
    }

    public void SelectSlot(GameObject slot)
    {
        UnSelectSlot();
        selectedSlot = slot;
        Item item = selectedSlot.GetComponent<SlotUI>()?.item;
        slot.transform.Find("SelectedTip")?.gameObject.SetActive(true);
        curIndex = selectedSlot.GetComponent<SlotUI>().index;
    }
    public void UnSelectSlot()
    {
        if (selectedSlot == null) return;
        selectedSlot.transform.Find("SelectedTip")?.gameObject.SetActive(false);
        selectedSlot = null;
    }

    public void CheckProfile(GameObject slot)
    {
        Item item = slot.GetComponent<SlotUI>()?.item;
        if (item != null)
        {
            profile.GetComponent<ProfileTip>()?.SetProfile(item);
            profile.SetActive(true);
            profile.GetComponent<ProfileTip>()?.RefreshUI();
        }
    }
    public void DisableProfile()
    {
        profile.SetActive(false); 
    }

    private void ScrollSelect()
    {
        float scroll = Mouse.current.scroll.ReadValue().y;

        if (scroll > 0) // 向上滚
        {
            curIndex--;
        }
        else if (scroll < 0) // 向下滚
        {
            curIndex++;
        }
        if (curIndex > curSlots.Length - 1) curIndex = 0;
        if (curIndex < 0) curIndex = curSlots.Length - 1;
        SelectSlot(curSlots[curIndex]);
    }
}
