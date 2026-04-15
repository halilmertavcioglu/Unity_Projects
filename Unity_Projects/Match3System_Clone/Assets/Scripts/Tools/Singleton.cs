using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ensures only one instance of a class exists and provides easy access to it.
/// </summary>
public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    #region Variables

    [Header("Singleton Reference")]
    private static T instance;

    /// <summary>
    /// Static access to the single instance of this class.
    /// </summary>
    public static T Instance 
    {
        get 
        {
            if (instance == null)
                Debug.LogError("No Instance of " + typeof(T) + " exists in the scene");

            return instance;
        }
    }

    #endregion

    /// <summary>
    /// Sets up the instance and prevents duplicates.
    /// </summary>
    protected void Awake()
    {
        if(instance == null)
        {
            instance = this as T;
            Init();
        }

        else
        {
            Debug.LogWarning("An instance of " + typeof(T) + " already exists in the scene. Self-destructing.");
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Clears the static reference when the object is destroyed.
    /// </summary>
    protected void OnDestroy()
    {
        if (this == instance)
            instance = null; 
    }

    /// <summary>
    /// Optional: Extra setup logic for child classes.
    /// </summary>
    protected virtual void Init() { } 
}
