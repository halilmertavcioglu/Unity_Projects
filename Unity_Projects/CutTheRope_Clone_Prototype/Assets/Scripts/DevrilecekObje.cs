using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevrilecekObje : MonoBehaviour
{
    [SerializeField] private AudioSource ses;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Top"))
            ses.Play();
    }
}
