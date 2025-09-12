using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityDataLoader : MonoBehaviour
{
    private void Start()
    {
        var task = EntityDatabase.InitializeDic();
        DataManager.instance.AddTask(task);
        Destroy(gameObject);
    }
}
