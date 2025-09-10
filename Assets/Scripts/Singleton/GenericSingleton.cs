using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

// Classe generica Singleton
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    private static bool isApplicationQuitting = false;
    public virtual bool IsDestroyedOnLoad() => false;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                FindAnyObjectByType(typeof(T));
                if (instance == null && !isApplicationQuitting)
                {
                    GameObject gameObj = new GameObject(typeof(T).Name + "_Singleton");
                    instance = gameObj.AddComponent<T>();
                    Debug.Log($"Generating new Singleton: {gameObj.name}");
                }
            }
            return instance;
        }
    }

    public virtual void Awake()
    {
        if (instance == null)
        {
            instance = GetComponent<T>();
            if (!IsDestroyedOnLoad())
            {
                DontDestroyOnLoad(gameObject);
                Debug.Log("Creating Singleton: " + gameObject.name);
            }
        }
        else if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    private void OnApplicationQuit()
    {
        isApplicationQuitting = true;
    }
}