using UnityEngine;

/// <summary>
/// Base Class for every Singleton Component, mainly used for controllers
/// </summary>
public abstract class MonoBehaviourSingleton<T> : MonoBehaviour where T : Component
{
    /// <summary>
    /// Static variable of the type 
    /// </summary>
    private static T _instance;
    /// <summary>
    /// Singleton accessor
    /// </summary>
    public static T Instance
    {
        get
        {
            if (_instance)
            {
                return _instance;
            }
            else
            {
                var instances = FindObjectsOfType<T>();
                if (instances.Length > 0)
                {
                    _instance = instances[0];
                }
                if (instances.Length > 1)
                {
                    Debug.LogError("There are more than one instances of " + typeof(T).Name);
                }
                if (!_instance)
                {
                    _instance = CreateDefaultInstance();
                }
                return _instance;
            }
        }
    }

    /// <summary>
    /// Initialices all the singleton components
    /// </summary>
    public abstract void InitializeSingleton();

    /// <summary>
    /// Executes at the begining of the scene, initialices the singleton or
    /// prevents a duplicate
    /// </summary>
    public virtual void Awake()
    {
        if (_instance == null || _instance==this)
        {
            _instance = this as T;
            InitializeSingleton();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    /// <summary>
    /// Instantiate a default singleton instance from a prefab
    /// </summary>
    /// <returns></returns>
    private static T CreateDefaultInstance()
    {
        string typeName = typeof(T).Name;
        GameObject singletonPrefab = Resources.Load(typeName) as GameObject;
        GameObject singletonInstance = null;
        if (singletonPrefab)
        {
            singletonInstance = Instantiate(singletonPrefab);
        }
        else
        {
            singletonInstance = new GameObject(typeName+"-Runtime");
            singletonInstance.AddComponent<T>();
        }
        T singleton = singletonInstance.GetComponent<T>();
        MonoBehaviourSingleton<T> genericSingleton = singleton as MonoBehaviourSingleton<T>;
        genericSingleton?.InitializeSingleton();
        return singleton;
    }

    /// <summary>
    /// Sets the singleton as persistent in other scenes
    /// </summary>
    protected void SetAsPersistentSingleton()
    {
        //transform.parent = null;
        DontDestroyOnLoad(gameObject);
    }
}