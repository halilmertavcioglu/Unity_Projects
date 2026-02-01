using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

//temas.relativeVelocity.y <= 0f --> Aþaðý doðru düþüyorsa veya durmuþsa kuvvet uygula demektir.
public class Platform : MonoBehaviour
{
    public float ziplamaGucu;
    private Camera kamera;

    private void Start()
    {
        kamera = Camera.main;
    }
    private void Update()
    {
        float mesafe = kamera.transform.position.y - transform.position.y;

        if (mesafe >= 5.15f)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + 18f);
        }
    }
    private void OnCollisionEnter2D(Collision2D temas)
    {
        Rigidbody2D karakterRb;
        if(temas.gameObject.tag == "Karakter" && temas.relativeVelocity.y <= 0f)
        {
            karakterRb = temas.gameObject.GetComponent<Rigidbody2D>();
            karakterRb.velocity = new Vector2(karakterRb.velocity.x, ziplamaGucu);
        }
    }
}
