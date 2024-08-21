using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public struct PoolPair{
    public GameObject poolable;
    public int count;
}
[CreateAssetMenu(menuName = "SO/Pool/List")]
public class PoolListSO : ScriptableObject
{
    public List<PoolItemSO> list;

}
