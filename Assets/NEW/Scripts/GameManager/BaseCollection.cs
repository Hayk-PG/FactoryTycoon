using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseCollection<T,T1> : MonoBehaviour
{
    public Dictionary<T, T1> Dict = new Dictionary<T, T1>();

    public event Action<T, T1> OnCollectionAdd;



    protected virtual void Awake()
    {
        InitializeCollection();
    }

    protected virtual void InitializeCollection()
    {

    }

    public virtual void Add(T key, T1 value)
    {
        if (Dict.ContainsKey(key))
        {
            return;
        }

        Dict.Add(key, value);
        OnCollectionAdd?.Invoke(key, value);
    }

    public virtual void Remove(T key, T1 value = default)
    {
        if (!Dict.ContainsKey(key))
        {
            return;
        }

        Dict.Remove(key);
    }
}
