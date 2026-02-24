using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("---TOP VE TEKNIK OZELLIKLER---")]
    [SerializeField] private TopAtici topAtar;
    [SerializeField] private CizgiCiz cizgiCiz;

    [Header("---GENEL OBJELER---")]
    [SerializeField] private ParticleSystem kovayaGirme;
    [SerializeField] private ParticleSystem bestScoreGecis;
    [SerializeField] private AudioSettings sesSistemi;

    [Header("---TOP VE TEKNIK OZELLIKLER---")]
    [SerializeField] private GameObject[] paneller;
    [SerializeField] private TextMeshProUGUI[] scoreTextleri;

    bool oyunBittiMi = false;

    int girenTopSayisi;
    void Start()
    {
        //Time.timeScale = 0;
        if(PlayerPrefs.HasKey("BestScore"))
        {
            scoreTextleri[0].text = PlayerPrefs.GetInt("BestScore").ToString();
            scoreTextleri[1].text = PlayerPrefs.GetInt("BestScore").ToString();
        }
        else
        {
            PlayerPrefs.SetInt("BestScore", 0);
            scoreTextleri[0].text = "0";
            scoreTextleri[1].text = "0";
        }
    }
    public void DevamEt(Vector2 Pos)
    {
        kovayaGirme.transform.position = Pos;
        kovayaGirme.gameObject.SetActive(true);
        kovayaGirme.Play();

        girenTopSayisi++;
        scoreTextleri[3].text = girenTopSayisi.ToString();

        topAtar.DevamEt();
        cizgiCiz.DevamEt();
    }
    public void OyunBitti()
    {
        if (oyunBittiMi) return;
        oyunBittiMi = true;

        Debug.Log("KAYBETTIN");
        sesSistemi.MuzigiDurdur();
        sesSistemi.OyunBitisSesiCal();
        paneller[1].SetActive(true);
        paneller[2].SetActive(false);

        scoreTextleri[1].text = PlayerPrefs.GetInt("BestScore").ToString();
        scoreTextleri[2].text =girenTopSayisi.ToString();

        if (girenTopSayisi > PlayerPrefs.GetInt("BestScore"))
        {
            PlayerPrefs.SetInt("BestScore", girenTopSayisi);
            bestScoreGecis.Play();
            
        }
        cizgiCiz.CizmeyiDurdur();
    }
    public void OyunBaslasin()
    {
        paneller[0].SetActive(false);
        topAtar.OyunBaslasin();
        cizgiCiz.CizmeyiBaslat();
        paneller[2].SetActive(true);
    }
    public void TekrarOyna()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
