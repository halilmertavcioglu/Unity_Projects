using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class KrktrKontrol : MonoBehaviour
{

    private Rigidbody2D karakterRb;
    public float yatayHiz;
    public TextMeshProUGUI skorYazisi;
    private float skor;
    public Transform kamera;
    public GameObject gameOverPaneli;
    public GameObject playButonu;
    private bool basla = false;
    void Start()
    {
        karakterRb = GetComponent<Rigidbody2D>();
        Time.timeScale = 0;
    }

    void Update()
    {
        if (!basla) return;

        float hiz = Input.GetAxis("Horizontal") * yatayHiz;
        karakterRb.velocity = new Vector2(hiz, karakterRb.velocity.y);

        if(karakterRb.velocity.y > 0 && transform.position.y > skor)
        {
            skor = transform.position.y;
            skorYazisi.text = "Score: " +Mathf.Round(skor).ToString();
        }

        if(kamera.position.y > transform.position.y + 7f)
        {
            Time.timeScale = 0;
            gameOverPaneli.SetActive(true);


        }
    }
    public void Basla()
    {
        basla = true;
        Time.timeScale = 1;
        //playButonu.SetActive(false);
        Destroy(playButonu);
    }
    public void playAgain()
    {
        SceneManager.LoadScene(0);
    }
}
