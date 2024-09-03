using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
[Serializable]
public struct SerializableDictionaryItem<TKey, TValue>
{
    public TKey key;
    public TValue value;
    public SerializableDictionaryItem(TKey key, TValue value)
    {
        this.key = key;
        this.value = value;
    }
}

[Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField]
    private List<SerializableDictionaryItem<TKey, TValue>> serializableItems = new();
    [SerializeField]
    // private List<TKey> keys = new List<TKey>();
    // [SerializeField]
    // private List<TValue> values = new List<TValue>();

    /// <summary>
    /// 데이터 저장시 실행
    /// </summary>
    public void OnBeforeSerialize()
    {
        serializableItems.Clear();
        foreach (KeyValuePair<TKey, TValue> pair in this)
        {
            serializableItems.Add(new SerializableDictionaryItem<TKey, TValue>(pair.Key, pair.Value));
        }

        // keys.Clear();
        // values.Clear();
        // foreach (KeyValuePair<TKey, TValue> pair in this)
        // {
        //     keys.Add(pair.Key);
        //     values.Add(pair.Value);
        // }
    }   
    /// <summary>
    /// 데이터 불러올 때 실행
    /// </summary>
    public void OnAfterDeserialize()
    {
        this.Clear();
        for (int i = 0; i < serializableItems.Count; i++)
        {
            if (this.ContainsKey(serializableItems[i].key))
            {
                if (serializableItems[i].key is Enum)
                    foreach (TKey key in Enum.GetValues(typeof(TKey)).Cast<TKey>().ToList())
                    {
                        if (!this.ContainsKey(key))
                        {
                            serializableItems[i] = new SerializableDictionaryItem<TKey, TValue>(key, serializableItems[i].value);
                            break;
                        }
                    }
                else
                {
                    serializableItems.RemoveAt(serializableItems.Count - 1);
                    serializableItems.Add(new SerializableDictionaryItem<TKey, TValue>(default(TKey), default(TValue)));
                }
                Debug.LogError($"SerializeDictionary : 중복된 키를 기본 키로 변경하였습니다");
            }

            if (this.ContainsKey(serializableItems[i].key))
                Debug.LogError($"SerializeDictionary : 중복된 키가 입력되어 값을 제거했습니다 \n 중복된  : " + serializableItems[i].key.ToString());
            else
                this.Add(serializableItems[i].key, serializableItems[i].value);
        }
        if (this.Count > serializableItems.Count)
        {
            serializableItems.Add(new SerializableDictionaryItem<TKey, TValue>(default(TKey), default(TValue)));
        }
        else if (serializableItems.Count > this.Count)
        {
            serializableItems.RemoveAt(serializableItems.Count - 1);
        }

        // this.Clear();
        // if (keys.Count > values.Count)
        // {
        //     keys[keys.Count - 1] = default(TKey);
        //     values.Add(default(TValue));
        // }
        // if (values.Count > keys.Count)
        // {
        //     values.RemoveAt(values.Count - 1);
        //     // values[values.Count-1] = default(TValue);
        //     // keys.Add(default(TKey));
        //     // Debug.Log("실행");
        // }


        // if (keys.Count != values.Count)
        // {
        //     throw new System.Exception(String.Format("이게 무슨 멍청한 짓인가요..."));
        // }
    }

}
