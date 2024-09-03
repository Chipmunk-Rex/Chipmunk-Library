using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class LightPool
{
    private Stack<GameObject> _pool;
    [SerializeField] private Transform _parentTrm;
    [SerializeField] public IPoolable poolable;
    [HideInInspector]
    public string PoolName
    {
        get
        {
            string name;
            if (poolable == null)
            {
                name = prefab.name;
            }
            else
            {
                name = poolable.PoolName;
            }
            return name;
        }
    }
    [SerializeField] public GameObject prefab;
    [SerializeField] private int count;
    public void Initailize(Transform parentTrm)
    {
        _parentTrm = parentTrm;
        poolable = prefab.GetComponent<IPoolable>();
        _pool = new Stack<GameObject>(count);

        for (int i = 0; i < count; i++)
        {
            GameObject gameObj = GameObject.Instantiate(prefab, _parentTrm);
            gameObj.SetActive(false);
            if (poolable != null)
                gameObj.name = this.poolable.PoolName;
            else
                gameObj.name = prefab.name;
            _pool.Push(gameObj);
        }
    }
    public GameObject Pop()
    {
        GameObject item = null;
        if (_pool.Count == 0)
        {
            GameObject gameObj = GameObject.Instantiate(prefab, _parentTrm);
            gameObj.name = poolable.PoolName;
        }
        else
        {
            item = _pool.Pop();
            item.SetActive(true);
        }
        return item;
    }
    public void Push(GameObject item)
    {
        item.SetActive(false);
        _pool.Push(item);
    }
}

public class LightPoolManager : MonoSingleton<LightPoolManager>
{

    [SerializeField] private List<LightPool> _poolableList = new();
    [SerializeField] private Dictionary<string, LightPool> _pools = new();
    protected override void Awake()
    {
        foreach (LightPool pool in _poolableList)
        {
            pool.Initailize(transform);
            if (pool.poolable != null)
                _pools.Add(pool.poolable.PoolName, pool);
            else
                _pools.Add(pool.prefab.name, pool);
        }
    }
    public GameObject Pop(string itemName)
    {
        if (_pools.ContainsKey(itemName))
        {
            GameObject item = _pools[itemName].Pop();
            if (item.TryGetComponent(out IPoolable poolable))
                poolable.ResetItem();
            return item;
        }
        Debug.LogError($"There is no pool {itemName}");
        return null;
    }
    public GameObject Pop(GameObject Prefab)
    {
        if (_pools.ContainsKey(Prefab.name))
        {
            GameObject item = _pools[Prefab.name].Pop();
            if (item.TryGetComponent(out IPoolable poolable))
                poolable.ResetItem();
            return item;
        }
        Debug.LogError($"There is no pool {Prefab.name}");
        return null;
    }

    public void Push(GameObject item)
    {
        if (item.TryGetComponent(out IPoolable poolable))
        {
            if (_pools.ContainsKey(poolable.PoolName))
            {
                _pools[poolable.PoolName].Push(item);
                return;
            }

            Debug.LogError($"There is no pool {poolable.PoolName}");
        }
        else
        {
            if (_pools.ContainsKey(item.name))
            {
                _pools[item.name].Push(item);
                return;
            }

            Debug.LogError($"There is no pool {item.name}");
        }
    }
}
