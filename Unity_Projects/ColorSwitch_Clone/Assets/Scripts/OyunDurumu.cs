using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OyunDurumu : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 0; // oyun baþlamadýysa top sabit
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Time.timeScale = 1.0f; // oyun baþlatýnca normal hýzýna döndü.
            this.enabled = false; // oyun baþlatýldýktan sonra sistemi yormamak için oyun hýz durumunu kapatmaya yarar.
        }
    }
}
