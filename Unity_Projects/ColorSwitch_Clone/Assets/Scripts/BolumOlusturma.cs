using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BolumOlusturma : MonoBehaviour
{
    public GameObject[] objeler;
    public GameObject yildiz, bayrak, renkDegistirici;
    public int objeSayisi;
    public float objeler_arasi_mesafe;

    void Start()
    {
        Vector2 pozisyon1 = new Vector2(0, 0); // çember/kare çeþitleri ve yýldýz
        Vector2 pozisyon2 = new Vector2(0, 3.5f); // renk deðiþtirici

        for (int i = 0; i < objeSayisi; i++)
        {
            int objeKodu = Random.Range(0, objeler.Length);

            Instantiate(objeler[objeKodu], pozisyon1, Quaternion.identity);
            Instantiate(yildiz, pozisyon1, Quaternion.identity);
            Instantiate(renkDegistirici, pozisyon2, Quaternion.identity);

            pozisyon1.y += objeler_arasi_mesafe;
            pozisyon2.y += objeler_arasi_mesafe;
        }
        Instantiate(bayrak, new Vector2(0, pozisyon2.y-4f), Quaternion.identity); // Bayrak oluþumu
    }
}
