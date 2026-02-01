using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TopKontrol : MonoBehaviour
{
    private Rigidbody2D topRb;
    public float ziplamaGucu;
    private SpriteRenderer topSr;
    public Color[] renkler;
    //public Color renk1, renk2, renk3, renk4;
    private int skor = 0;
    public TMP_Text skorYazisi;
    void Start()
    {
        topRb = GetComponent<Rigidbody2D>();
        topSr = GetComponent<SpriteRenderer>();
        RenkDegistir();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            topRb.velocity = Vector2.up * ziplamaGucu;
        }
    }

    private void OnTriggerEnter2D(Collider2D temas)
    {
        if(temas.gameObject.tag == "Kenar" && temas.GetComponent<KnrObjeRenkleri>().renk != topSr.color)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if(temas.gameObject.tag == "RenkDegistirici")
        {
            RenkDegistir();
            Destroy(temas.gameObject);
        }

        if(temas.gameObject.tag == "Yildiz")
        {
            skor = skor + 1;
            skorYazisi.text ="Score: "+skor.ToString();
            Destroy(temas.gameObject);
        }

        if(temas.gameObject.tag == "Bayrak")
        {
            int levelKodu = SceneManager.GetActiveScene().buildIndex;
            levelKodu++;
            SceneManager.LoadScene(levelKodu);
        }
    }

    private void RenkDegistir()
    {
        int rastgeleSayi = Random.Range(0, renkler.Length);

        topSr.color = renkler[rastgeleSayi];
        
        /*switch(rastgeleSayi)
        {
            case 1:
                topSr.color = renk1;
                break;
            case 2:
                topSr.color = renk2;
                break;
            case 3:
                topSr.color = renk3;
                break;
            case 4:
                topSr.color = renk4;
                break;
        }*/
    }
}
