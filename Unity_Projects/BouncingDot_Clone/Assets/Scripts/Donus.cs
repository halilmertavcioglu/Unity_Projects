using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Donus : MonoBehaviour
{
    private float beklemeSuresi = 0.05f;
    void Update()
    {
        StartCoroutine(AltigenDonus()); 
    }

    IEnumerator AltigenDonus()
    {
        Vector2 farePozisyonu = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, 0));

        if (Input.GetMouseButtonDown(0) && farePozisyonu.x > 0) //saða döndürmek için
        {
            transform.Rotate(0, 0, -30);
            yield return new WaitForSeconds(beklemeSuresi); //Daha kademeli bir dönüþ yapmak için 
            transform.Rotate(0, 0, -30);
        }
        else if (Input.GetMouseButtonDown(0) && farePozisyonu.x < 0) //sola döndürmek için
        {
            transform.Rotate(0, 0, 30);
            yield return new WaitForSeconds(beklemeSuresi); //Daha kademeli bir dönüþ yapmak için
            transform.Rotate(0, 0, 30);
        }
    }
}
