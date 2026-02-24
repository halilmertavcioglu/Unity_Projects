using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("---Genel Listeler-Objeler---")]
    [SerializeField] private GameObject[] ipMerkezleri;
    [SerializeField] private ParticleSystem kesmeEfekt;

    [Header("---Level Ayarlari---")]
    [SerializeField] private int leveldekiTopSayisi;
    [SerializeField] private int devrilmesiGerekenObjeSayisi;

    [Header("---UI Objeleri---")]
    [SerializeField] private GameObject[] paneller;
    [SerializeField] private TextMeshProUGUI[] textler;

    [Header("---Ses Ayarlari---")]
    [SerializeField] private AudioManager audioManager;

    [Header("---Oynanis Ayarlari---")]
    [SerializeField] private float kesildiktenSonraSure = 5f;
    private bool sureBasladiMi = false;


    private void Start()
    {
        sureBasladiMi = false;
    }
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero); /*Fare imlecinin
            ekrandaki koordinatlarini, oyun dunyasindaki 2D koordinatlara cevirir ve oraya hayali isin(raycast) atar.*/

            if(hit.collider != null)
            {
                audioManager.IpKesmeSesiCal();
                if (hit.collider.CompareTag("Merkez_1"))
                    ZincirIslemleri(hit, "Merkez_1");
                else if (hit.collider.CompareTag("Merkez_2"))
                    ZincirIslemleri(hit, "Merkez_2");
                else if (hit.collider.CompareTag("Merkez_3"))
                    ZincirIslemleri(hit, "Merkez_3");
                else if (hit.collider.CompareTag("Merkez_4"))
                    ZincirIslemleri(hit, "Merkez_4");
                else if (hit.collider.CompareTag("Merkez_5"))
                    ZincirIslemleri(hit, "Merkez_5");
            }
        }
        if(sureBasladiMi)
        {
            kesildiktenSonraSure -= Time.deltaTime;
            if(kesildiktenSonraSure <= 0)
            {
                Kaybettin();
                sureBasladiMi = false;
            }
        }
    }
    void ZincirIslemleri(RaycastHit2D hit, string HingeAdi)
    {
        hit.collider.gameObject.SetActive(false);
        kesmeEfekt.transform.position = hit.collider.gameObject.transform.position;
        kesmeEfekt.Play();
        foreach (var item in ipMerkezleri)
        {
            if (item.GetComponent<ip_yonetimi>().HingeAdi == HingeAdi)
            {
                foreach (var item2 in item.GetComponent<ip_yonetimi>().baglantiHavuzu)
                {
                    item2.SetActive(false);
                }
            }
        }
        if(!sureBasladiMi)
        {
            kesildiktenSonraSure = 5f;
            sureBasladiMi = true;
        }
    }
    public void TopDustu()
    {
        leveldekiTopSayisi--;

        if (leveldekiTopSayisi == 0)
        {
            if (devrilmesiGerekenObjeSayisi > 0)
                Kaybettin();
            else if(devrilmesiGerekenObjeSayisi == 0)
                Kazandin();
        }
        else
        {
            if (devrilmesiGerekenObjeSayisi == 0)
                Kazandin();
        }
    }
    public void HedefObjeDustu()
    {
        devrilmesiGerekenObjeSayisi--;
        if (leveldekiTopSayisi == 0 && devrilmesiGerekenObjeSayisi == 0)
            Kazandin();
        else if (leveldekiTopSayisi == 0 && devrilmesiGerekenObjeSayisi > 0)
            Kaybettin();
    }
    void Kazandin()
    {
        audioManager.KazanmaSesiCal();
        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
        textler[0].text = "LEVEL : " + SceneManager.GetActiveScene().name;
        paneller[0].SetActive(true);
        Time.timeScale = 0;
    }
    void Kaybettin()
    {
        textler[1].text = "LEVEL : " + SceneManager.GetActiveScene().name;
        audioManager.KaybetmeSesiCal();
        paneller[1].SetActive(true);
        Time.timeScale = 0;
    }
    public void ButonIslemleri(string Deger)
    {
        switch(Deger)
        {
            case "Tekrar":
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                Time.timeScale = 1;
                break;

            case "SonrakiLevel":
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                Time.timeScale = 1;
                break;
        }
    }
}
