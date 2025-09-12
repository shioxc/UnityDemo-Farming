using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Component
{
    private readonly T prefab;
    private readonly Queue<T> pool = new Queue<T>();
    private readonly Transform content;

    public ObjectPool(T prefab, int initialSize,Transform _content)
    {
        this.prefab = prefab;
        content = _content;
        Expand(initialSize);
    }
    private void Expand(int count)
    {
        for (int i = 0; i < count; i++)
        {
            T obj = GameObject.Instantiate(prefab,content);
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public T Get()
    {
        if (pool.Count == 0)
        {
            Expand(10);
        }

        T obj = pool.Dequeue();
        return obj;
    }
    public void Release(T obj)
    {
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }
}
