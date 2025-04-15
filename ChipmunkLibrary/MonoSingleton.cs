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
                    throw new Exception($"MonoSingleton : Cannot Find Singleton Instance");
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
        }
        else if (_instace != this)
        {
            Destroy(gameObject);
        }

        
        if (isDontDestroy)
            DontDestroyOnLoad(gameObject);
    }
    private void OnDisable()
    {
        IsDestroyed = true;
    }
}
