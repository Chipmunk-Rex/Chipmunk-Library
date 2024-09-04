using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkMonoSingleton<T> : NetworkBehaviour where T : NetworkBehaviour
{
    protected static T _instace = null;
    private static bool IsDestroyed = false;
    public static T Instance{
        get{
            if(IsDestroyed)
                _instace = null;
            if(_instace == null){
                    _instace = GameObject.FindAnyObjectByType<T>();
                if(_instace == null)
                    throw new Exception("NetworkMonoSingleton : Cannot Find Instance");
                else
                IsDestroyed = false;
            }
                return _instace;
        }
    }

    private void OnDisable(){
        IsDestroyed = true;
    }
}
