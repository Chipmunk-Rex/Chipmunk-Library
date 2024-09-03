using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class SerializeableNotifyValue<T>
{
    /// <summary>
    /// 값이 변경될때 호출되는 이벤트.
    /// 반환 : (T prev, T next)
    /// </summary>
    [SerializeField] public UnityEvent<T, T> OnvalueChanged;
    [SerializeField] private T _value;
    public T Value
    {
        get
        {
            return _value;
        }
        set
        {
            T before = _value;
            _value = value;
            if ((before == null && value != null) || !before.Equals(_value))
                OnvalueChanged?.Invoke(before, _value);
        }
    }
    public SerializeableNotifyValue()
    {
        _value = default(T);
    }
    public SerializeableNotifyValue(T value)
    {
        _value = value;
    }
}
