using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MercuryDictionary<TKey,TValue>:IEnumerable
{
    private Dictionary<TKey, TValue> _data = new Dictionary<TKey, TValue>();

    public IEnumerator<KeyValuePair<TKey,TValue>> GetEnumerator()
    {
        return _data.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _data.GetEnumerator();
    }

    public bool IsRegistered(TKey key)
    {
        return _data.ContainsKey(key);
    }

    public void Register(TKey key,TValue value)
    {
        if (IsRegistered(key)) return;
        _data.Add(key, value);
    }

    public void UnRegister(TKey key)
    {
        if (!IsRegistered(key)) return;
        _data.Remove(key);
    }

    public TValue GetValue(TKey key)
    {
        return IsRegistered(key) ? _data[key] : default(TValue);
    }

    public void Clear() => _data.Clear();
}
