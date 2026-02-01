using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Bird : MonoBehaviour
{
    private Rigidbody2D birdRb;
    private SpriteRenderer birdSr;
    public float ziplamaGucu;
    public AudioSource kanatSesi;
    public AudioSource basariliGecisSesi;
    public AudioSource kaybetmeSesi;
    public int skor;
    public int rekor;
    public TMP_Text skorText;
    public TMP_Text rekorText;
    public GameObject gameOverPaneli;
    public GameObject startPaneli;
    void Start()
    {
        birdRb = GetComponent<Rigidbody2D>();
        birdSr = GetComponent<SpriteRenderer>();
        skor = 0;
        gameOverPaneli.SetActive(false);
        startPaneli.SetActive(true);
        Time.timeScale = 0;
        birdSr.enabled = false;
        skorText.text = "Score: 0";
        rekor = PlayerPrefs.GetInt("Rekor", 0);
        rekorText.text = "High Score: " + rekor;
    }
    void Update()
    {
        if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            birdSr.enabled = true;
            birdRb.velocity = Vector2.up * ziplamaGucu;
            kanatSesi.Play();
            
        }
        skorText.text = "Score: " + skor;
    }
    private void OnCollisionEnter2D(Collision2D temas)
    {
       if(temas.gameObject.tag == "Pipe" || temas.gameObject.tag == "Zemin")
        {
            kanatSesi.enabled = false;
            Time.timeScale = 0;
            kaybetmeSesi.Play();
            gameOverPaneli.SetActive(true);
        }
    }
    private void OnTriggerEnter2D(Collider2D temas)
    {
        if(temas.gameObject.tag == "Skor")
        {
            skor++;
            basariliGecisSesi.Play();
            if(skor > rekor)
            {
                rekor = skor;
                PlayerPrefs.SetInt("Rekor", rekor);
                rekorText.text = "High Score: " + rekor;
            }

        }
    }
    public void StartGame()
    {
        startPaneli.SetActive(false);
        Time.timeScale = 1;
    }
    public void PlayAgain()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
