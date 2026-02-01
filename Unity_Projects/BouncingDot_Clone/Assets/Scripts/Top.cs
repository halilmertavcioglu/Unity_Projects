using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Top : MonoBehaviour
{
    private Rigidbody2D topRb;
    private SpriteRenderer topSr;
    public float ziplamaGucu;
    public Color renk1, renk2, renk3, renk4, renk5, renk6;
    public TextMeshProUGUI skorYazisi, rekorYazisi;
    private int skor, rekor;
    void Start()
    {
        topRb = GetComponent<Rigidbody2D>();
        topSr = GetComponent<SpriteRenderer>();

        if(PlayerPrefs.HasKey("rekor"))
        {
            rekor = PlayerPrefs.GetInt("rekor");
            rekorYazisi.text = "High Score: " + rekor.ToString();
        }
        else
        {
            rekor = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D temas)
    {
        if(temas.gameObject.tag == "Zemin")
        {
            topRb.AddForce(Vector2.up * ziplamaGucu/2, ForceMode2D.Impulse);
        }

        if (temas.gameObject.tag == "Kenar")
        {
            topRb.AddForce(Vector2.up * ziplamaGucu, ForceMode2D.Impulse);

            if (ToleransliEslestirme(topSr.color, temas.gameObject.GetComponent<SpriteRenderer>().color))
            {
                GetComponent<AudioSource>().Play();
                skor = skor + 1;
                skorYazisi.text = "Score: " +skor.ToString();

                if(skor > rekor)
                {
                    rekor = skor;
                    rekorYazisi.text = "High Score: "+ rekor.ToString();
                    PlayerPrefs.SetInt("rekor", rekor);
                }
            }
            else if(topSr.color != temas.gameObject.GetComponent<SpriteRenderer>().color)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D TemasT)
    {
        if(TemasT.gameObject.tag == "RenkDegistirici")
        {
            RenkDegistir();
        }
    }

    private void RenkDegistir()
    {
        int rastgeleSayi = Random.Range(1,7);

        switch(rastgeleSayi)
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

            case 5:
                topSr.color = renk5;
                break;

            case 6:
                topSr.color = renk6;
                break;

        }
    }
    bool ToleransliEslestirme(Color renk1, Color renk2)
    {
        if (Mathf.Abs(renk1.r - renk2.r) < 0.05f && Mathf.Abs(renk1.g - renk2.g) < 0.05f && Mathf.Abs(renk1.b - renk2.b) < 0.05f)
        {
            return true; // Renkler neredeyse ayný
        }
        else
        {
            return false; // Farklý
        }
    }
}