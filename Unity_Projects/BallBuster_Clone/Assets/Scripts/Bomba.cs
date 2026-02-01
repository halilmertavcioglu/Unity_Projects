using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;

public class Bomba : MonoBehaviour
{
    [SerializeField] private int sayi;
    [SerializeField] TextMeshProUGUI sayiText;
    [SerializeField] GameManagerr gameManager;

    List<Collider2D> colliders = new List<Collider2D>(); //OverlapBox'un buldugu Collider2D'lerin bilgisini saklamak icin kullanilir.

    private void Start()
    {
        sayiText.text = sayi.ToString(); //Bombanin ustundeki yaziyi ayarla.
        gameObject.tag = sayi.ToString(); // Bombayi, ayni sayidaki toplarla etkilesime girecek sekilde etiketle.
    }
    private void OnCollisionEnter2D(Collision2D temas)
    {
        if(temas.gameObject.CompareTag(sayi.ToString())) //Eger carpan objenin etiketi bombaninkiyle ayniysa patlama gerceklestir.
        {
            GucUygula();
        }
    }

    void GucUygula() //Guc uygulanacak alani kontol eden fonksiyon. Patlama alani taranir ve etkilencek objeler belirlenir.
    {
        var temasFiltresi2D = new ContactFilter2D //Hangi objelerin algilanacagini belirleyen filtre olusturur.
        {
            useTriggers = true //Trigger olan collider'lar da algilansin mi? (evet). 
        };

        //Physics2D.OverlapBox(pozisyon, patlamaBoyutu, aci, filtre, sonucListesi);
        Physics2D.OverlapBox(transform.position, transform.localScale * 2, 20f, temasFiltresi2D, colliders); /*Patlamanin etki alanini tanimlar ve bu alanda kalan,
        filtreden gecen objeleri "colliders" listesine ekler.*/

        gameManager.PatlamaEfekt(transform.position); //Gorsel patlama efektini calistirir.
        gameObject.SetActive(false); //Bambayi sahneden kaldirir.

        foreach(Collider2D item in colliders) //colliders adli listedeki tum Collider2D'leri tek tek gezer.
        {
            if(item.gameObject.tag == "Kutu") //Eger bu obje bir kutuysa
            {
                item.GetComponent<Kutu>().EfektOynat(); //Kutuyu parcala ve efekti oynat.
            }
            else if(item.gameObject.GetComponent<Rigidbody2D>() != null) /*Objede fizik bilesenine(Rigidbody2D) sahipse(Top vb)
            Objeyi patlamanin siddetiyle yukari ve ileri firlat.*/
            { 
                item.gameObject.GetComponent<Rigidbody2D>().AddForce(90 * new Vector2(0, 6), ForceMode2D.Force);
            }
        }

        /*foreach(Collider2D item in colliders) //colliders adli listedeki tum Collider2D'leri tek tek gezer.
        {
            Rigidbody2D rb = item.GetComponent<Rigidbody2D>(); //O collider'a bagli Rigidbody var mi diye bakar.

            if(rb != null) //Rigidbody yoksa oyun cokmesin diye kuvvet uygulanmaz. 
            {
                rb.AddForce(Vector2.up * 540); //Yukari yonlu siddet uygular.
            }
        }*/
    }
}