using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yonetici : MonoBehaviour
{
    public GameObject altigen, top, baslangicYazisi;
    public Transform baslangicPozisyonu;
    private bool basla = false;
    void Start()
    {
        altigen.GetComponent<Donus>().enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            top.transform.position = baslangicPozisyonu.transform.position;
            altigen.GetComponent<Donus>().enabled = true;
            baslangicYazisi.SetActive(false);
            basla = true;

        }
    }
}
