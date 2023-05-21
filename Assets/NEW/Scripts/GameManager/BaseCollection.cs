using System.Collections.Generic;
using UnityEngine;

public class BaseCollection<T,T1> : MonoBehaviour
{
    public Dictionary<T, T1> Dict = new Dictionary<T, T1>();



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
    }

    public void Remove(T key)
    {
        if (!Dict.ContainsKey(key))
        {
            return;
        }

        Dict.Remove(key);
    }
}
