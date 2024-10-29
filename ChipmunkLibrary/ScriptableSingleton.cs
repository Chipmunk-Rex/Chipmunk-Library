using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


namespace Chipmunk.Library
{
    public class ScriptableSingleton<T> : ScriptableObject where T : ScriptableObject
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    T[] results = Resources.FindObjectsOfTypeAll<T>();

                    if (results.Length == 0)
                    {
                        Debug.LogWarning("SingletonSO<" + typeof(T).Name + "> is missing");
                        _instance = Resources.Load<T>($"ScriptableObject/{typeof(T).Name}");

                        if (_instance == null)
                        {
                            Debug.LogWarning($"SingletonSO<" + typeof(T).Name + "> not found in Resources. Creating a new instance.");
#if UNITY_EDITOR
                        _instance = CreateAndSaveInstance();
#else
                            _instance = CreateInstance<T>();
#endif
                        }
                    }
                    if (results.Length > 1)
                    {
                        Debug.LogError("Singleton<" + typeof(T).Name + "> has more than 1 instance");
                        return null;
                    }
                    if (_instance == null)
                        _instance = results[0];
                }
                return _instance;
            }
            private set
            {
                if (_instance != null)
                {
                    Debug.LogError("Singleton<" + typeof(T).Name + "> is already set \n maybe you have to destroy it first");
                    return;
                }
                _instance = value;
            }
        }
        protected virtual void OnEnable()
        {
            Initialize();
        }
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public void Initialize()
        {
            if (Instance != null)
            {
                Debug.LogWarning("Singleton<" + typeof(T).Name + "> is already set \n maybe you have to destroy it first");
            }
            Instance = this as T;
        }


#if UNITY_EDITOR
    private static T CreateAndSaveInstance()
    {
        T instance = CreateInstance<T>();
        string directoryPath = "Assets/Resources/ScriptableObject";
        string assetPath = $"{directoryPath}/{typeof(T).Name}.asset";

        // 디렉토리가 존재하지 않으면 생성합니다.
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        AssetDatabase.CreateAsset(instance, assetPath);
        AssetDatabase.SaveAssets();
        return instance;
    }
#endif
    }
}