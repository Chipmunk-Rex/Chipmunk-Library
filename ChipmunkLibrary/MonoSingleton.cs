using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    [Header("MonoSingleton")]
    [Tooltip("Dont Destroy on Scene Change")]
    [SerializeField] bool isDontDestroy = false;
    [Space(1)]
    protected static T _instace = null;
    private static bool IsDestroyed = false;
    public static T Instance
    {
        get
        {
            if (IsDestroyed)
                _instace = null;
            if (_instace == null)
            {
                _instace = GameObject.FindAnyObjectByType<T>();
                if (_instace == null)
                    throw new Exception("바보같은! 싱글톤이 없어!");
                else
                    IsDestroyed = false;
            }
            return _instace;
        }
    }
    protected virtual void Awake()
    {
        if (_instace == null)
        {
            _instace = this as T;
            if (isDontDestroy)
                DontDestroyOnLoad(gameObject);
        }
        else if (_instace != this)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    private void OnDisable()
    {
        IsDestroyed = true;
    }
}
