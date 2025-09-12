using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteSaveTip : MonoBehaviour
{
    public SaveSlotUI slot;
    public StringEventSO ShowTipEventSO;

    
    public void ShowTip()
    {
        Debug.Log(slot.saveId);
        ShowTipEventSO.RaiseEvent(slot.saveId);
    }
}
