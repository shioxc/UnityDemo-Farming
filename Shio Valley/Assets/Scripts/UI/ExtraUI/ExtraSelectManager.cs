using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraSelectManager : MonoBehaviour
{
    public GameObject profile;
    private void OnDisable()
    {
        UnSelectSlot();
    }
    public void SelectSlot(int id)
    {
        Item item = ItemLoader.GetItemById(id);
        if (item != null)
        {
            profile.GetComponent<ProfileTip>()?.SetProfile(item);
            profile.SetActive(true);
            profile.GetComponent<ProfileTip>()?.RefreshUI();
        }
    }
    public void UnSelectSlot()
    {
        profile.SetActive(false);
    }
}
