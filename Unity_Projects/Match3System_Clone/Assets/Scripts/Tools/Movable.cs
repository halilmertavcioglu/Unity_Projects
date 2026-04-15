using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Base class for smooth object movement using easing functions.
/// </summary>
public class Movable : MonoBehaviour
{
    #region Variables

    [Header("Movement Settings")]
    [SerializeField] private float speed = 1f;
    protected bool idle = true;

    [Header("Internal Progress")]
    private Vector3 from;
    private Vector3 to;
    private float howfar;

    public bool Idle
    {
        get
        { 
            return idle; 
        }
    }
    
    public float Speed
    {
        get
        {
            return speed;
        }
    }

    #endregion

    /// <summary>
    /// Moves the object to a specific coordinate in world space.
    /// </summary>
    public IEnumerator MoveToPosition(Vector3 targetPos)
    {
        if (speed <= 0)
            Debug.LogWarning("Speed must be a positive number.");

        from = transform.position; 
        to = targetPos;
        howfar = 0;
        idle = false;

        do
        {
            howfar += speed * Time.deltaTime;
            if (howfar > 1)
                howfar = 1;

            transform.position = Vector3.LerpUnclamped(from, to, Easing(howfar));
            yield return null; 
        }
        while (howfar != 1); 

        idle = true; 
    }

    /// <summary>
    /// Follows and moves toward a moving target object.
    /// </summary>
    public IEnumerator MoveToTransform(Transform target)
    {
        if (speed <= 0) 
            Debug.LogWarning("Speed must be a positive number.");

        from = transform.position; 
        to = target.position; 
        howfar = 0; 
        idle = false; 

        do
        {
            howfar += speed * Time.deltaTime; 

            if (howfar > 1) 
                howfar = 1;

            to = target.position;
            transform.position = Vector3.LerpUnclamped(from, to, Easing(howfar));
            yield return null; 
        }
        while (howfar != 1); 

        idle = true; 
    }

    /// <summary>
    /// Mathematical formula to add aesthetic curve/acceleration to movement.
    /// </summary>
    private float Easing(float t) 
    {
        float c1 = 1.70158f, c2 = c1 * 1.525f;

        return t < 0.5f
            ? (Mathf.Pow(t * 2, 2) * ((c2 + 1) * 2 * t - c2)) / 2
            : (Mathf.Pow(t * 2 - 2, 2) * ((c2 + 1) * (t * 2 - 2) + c2) + 2) / 2;
    }
}
