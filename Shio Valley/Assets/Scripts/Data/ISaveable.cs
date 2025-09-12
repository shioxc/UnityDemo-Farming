using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable
{
    void OnSave(string saveId);
    void OnLoad(string saveId);
}
