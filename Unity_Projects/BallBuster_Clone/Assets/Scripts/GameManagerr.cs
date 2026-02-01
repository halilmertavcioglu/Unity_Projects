using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

[Serializable]
public class Hedefler //Bu kisim sadece veri tutar.
{
    public Sprite hedefGorsel;
    public string hedefTuru;
    public int hedefinDegeri;
    public GameObject gorevTamam;
}

[Serializable]
public class HedeflerUI //Bu kisim sadece UI kontrol eder.
{
    public GameObject gorev;
    public Image hedefGorsel;
    public TextMeshProUGUI hedefinDegeriText;
    public GameObject gorevTamam;
}

public class GameManagerr : MonoBehaviour
{
    [Header("--Level Ayarlari--")]
    public Sprite[] spriteObjeleri; //Toplarin gorselini degistirmek icin sprite havuzu
    [SerializeField] private GameObject[] toplar; //Onceden sahnede/prefab hazirlanmis top havuzu
    [SerializeField] private TextMeshProUGUI kalanTopSayisiText;
    int kalanTopSayisi; //Oyunda kac top kaldigini tutan degisken
    int havuzIndex; //Siradaki kullanilacak topun index'i

    [Header("--Atis Sistemi--")]
    [SerializeField] private GameObject topAtici; //Topu firlatan obje
    [SerializeField] private GameObject aticiNamlusu; //Topun atis oncesinde durdugu yer
    [SerializeField] private GameObject gelecekTop; //Namludaki toptan sonra gelecek top
    GameObject seciliTop;

    [Header("---Ses Ayarlari---")]
    [SerializeField] private AudioSource sesKaynagi;
    [SerializeField] private AudioSource oyunMuzigi;
    [SerializeField] private AudioClip bombaPatlamaSesi;
    [SerializeField] private AudioClip topBirlesmeSesi;
    [SerializeField] private AudioClip kazanmaSesi;
    [SerializeField] private AudioClip kaybetmeSesi;
    [SerializeField] private AudioClip gorevTamamlamaSesi;

    [Header("--Diger Ayarlar--")]
    [SerializeField] private ParticleSystem patlamaEfekti;
    [SerializeField] private ParticleSystem[] kutuKirilmaEfektleri;
    int kutuKirilmaIndex;

    [Header("--Gorev Islemleri--")]
    [SerializeField] private List<Hedefler> Hedefler; //Level’daki görevlerin oyun içi verilerini tutar (hedef türü, miktar vb.).
    [SerializeField] private List<HedeflerUI> HedeflerUI; //Gorevlerin UI gosterimini yapar(Kac adet gorev varsa).
    int topDegeri, kutuDegeri, toplamGorevSayisi;
    bool kutuHedefiVarMi;
    public bool topHedefiVarMi;
    public bool topGoreviTamamlandiMi = false;

    void Start()
    {
        kalanTopSayisi = toplar.Length; //Top sayisini havuz uzunluguna esitler.
        TopGetir(true); //Ilk  topu ve gelecek topu sahneye getirir.
        toplamGorevSayisi = Hedefler.Count;
        oyunMuzigi.Play();
        
        for (int i = 0; i < Hedefler.Count; i++) //Hedefler.Count --> Hedefler listesinde kac adet gorev oldugunu verir.
        {
            HedeflerUI[i].gorev.SetActive(true); //Gorevin UI panelini ekranda aktif eder.
            HedeflerUI[i].hedefGorsel.sprite = Hedefler[i].hedefGorsel; //Gorevin oyun ici gorselini UI'a atar.
            HedeflerUI[i].hedefinDegeriText.text = Hedefler[i].hedefinDegeri.ToString(); //Gorevde istenen sayýyý yazýya cevirip UI'da gosterir.
            if(Hedefler[i].hedefTuru == "Top")
            {
                topHedefiVarMi = true;
                topDegeri = Hedefler[i].hedefinDegeri;
            }
            else if(Hedefler[i].hedefTuru == "Kutu")
            {
                kutuHedefiVarMi = true;
                kutuDegeri = Hedefler[i].hedefinDegeri;
            }
        }
    }

    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //Mouse pozisyonundan sahneye isin(ray) gönderir.
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);// Isinin(ray) 2D objeye carpip carpmadigini kontrol eder.

            if(hit.collider != null) //Bir objeye carptiysa
            {
                if(hit.collider.gameObject.CompareTag("OyunZemini")) //Carpilan obje "OyunZemini" tagina sahipse
                {
                    Vector2 MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); //Mouse'nin dunya koordinatlarini alir.

                    topAtici.transform.position = Vector2.MoveTowards(topAtici.transform.position, new Vector2(MousePosition.x,
                      topAtici.transform.position.y), 30 * Time.deltaTime); //Sadece x ekseninde, mouse'ye dogru yumusak bir sekilde hareket ettirir.
                }
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            seciliTop.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic; /*Mouse'un sol tik birakildiginda, topun RigidBody2D'sini 
            kinematic'ten dynamic'e cevirerek asagü düsüsü saglanir.*/
            seciliTop.transform.parent = null; //"Child" olan topu, aticidan ayirir.
            seciliTop.GetComponent<Top>().BirincilDurumDegistir();
            TopGetir(false);
        }
    }
    void TopGetir(bool IlkKurulum)
    {
        if(IlkKurulum) //Bu kisim atistan onceki kurulum
        {
            toplar[havuzIndex].transform.SetParent(topAtici.transform); //Top atilmadan once aticiya bagliyoruz.
            toplar[havuzIndex].transform.position = aticiNamlusu.transform.position; //Sahnede sag üstteki siradaki topu, atis noktasina getirmek. 
            toplar[havuzIndex].SetActive(true); //Atilacak topu gorunur hale getirir
            seciliTop = toplar[havuzIndex]; //Su an atilacak topu secili top olarak kaydeder.

            havuzIndex++;

            toplar[havuzIndex].transform.position = gelecekTop.transform.position; //Siradaki topu "gelecekTop" alanina yerlestirir.
            toplar[havuzIndex].SetActive(true); //"gelecekTop" alanindaki topu gorunur yapar.
            kalanTopSayisiText.text = kalanTopSayisi.ToString(); //Kalan top sayisini gunceller.
        }
        else //Bu kisim her atistan sonra calisacak bolum
        {
            if(toplar.Length != 0) //Havuzda top varsa devam et(Guvenlik kontrolu)
            {
                toplar[havuzIndex].transform.SetParent(topAtici.transform);//Yeni topu tekrar aticiya baglar.
                toplar[havuzIndex].transform.position = aticiNamlusu.transform.position; //Yeni topu, namlu ucuna getirir.
                toplar[havuzIndex].SetActive(true); //Namlu ucundaki topu aktif eder.
                seciliTop = toplar[havuzIndex]; //Atilacak topu secili top olarak kaydeder.

                kalanTopSayisi--; //Havuzdaki top sayisini her iþlemde birer birer azaltiyoruz.
                kalanTopSayisiText.text = kalanTopSayisi.ToString(); //UI'da gunceller.

                if(havuzIndex != toplar.Length-1) //Eger son topa henuz gelinmediyse
                {
                    havuzIndex++; //Siradaki topa gec.
                    toplar[havuzIndex].transform.position = gelecekTop.transform.position; //Gelecek topu yerlestir.
                    toplar[havuzIndex].SetActive(true); //Gelecek topu gorunur yap.
                }
            }

            if(kalanTopSayisi == 0) //Atilacak top kalmadiysa
            {
                Invoke("GorevleriKontrolEt", 2.5f);
            }
        } 
    }

    public void PatlamaEfekt(Vector2 Pozisyon) //Patlama efektini verilen (x,y) konumunda oynatir.
    {
        patlamaEfekti.gameObject.transform.position = Pozisyon; //Patlama efektini verilen pozisyona tasir.
        patlamaEfekti.gameObject.SetActive(true); //Patlama efektini aktif hale getirir.
        patlamaEfekti.Play(); //Patlama efektini oynatir.
        sesKaynagi.PlayOneShot(bombaPatlamaSesi);
    }

    public void KutuParcalamaEfekt(Vector2 Pozisyon) //Kutu kirilma efektini verilen (x,y) konumunda oynatir ve gorev durumunu gunceller.
    {
        kutuKirilmaEfektleri[kutuKirilmaIndex].gameObject.transform.position = Pozisyon; //Kutu kirilma efektini verilen pozisyona tasir.
        kutuKirilmaEfektleri[kutuKirilmaIndex].gameObject.SetActive(true); //Kirilma efektini aktif eder.
        patlamaEfekti.Play(); //Kirilma efektini oynatir.

        if(kutuHedefiVarMi) //Level'da kutu hedefi varsa gorev sayisini kontrol eder.
        {
            kutuDegeri--; //Kutu kirildikca hedeflenen kutu sayisini azaltir.
            if(kutuDegeri == 0)
            {
                HedeflerUI[1].gorevTamam.SetActive(true); //Kutu hedefi tamamlandiysa UI'da "Gorev Tamam" isaretini aktif eder.
                toplamGorevSayisi--; //Tamamlanan gorevi cikartir ve toplam gorev sayisini azaltir.
                sesKaynagi.PlayOneShot(gorevTamamlamaSesi);

                if (toplamGorevSayisi == 0)
                {
                    Kazandin();
                }
            }
        }

        if(kutuKirilmaIndex == kutuKirilmaEfektleri.Length-1) /*Son kirilma efektine ulasildi mi kontrol eder.
        Efektler sirasiyla oynatilir, sona gelinince basa sarar.*/
        {
            kutuKirilmaIndex = 0;
        }
        else
        {
            kutuKirilmaIndex++;
        }
    }

    public void GorevSayiKontrolu(int sayi) //Top hedefi tamamlandi mi kontrol eder.
    {
        if(topGoreviTamamlandiMi == false && sayi == topDegeri)
        {
            topGoreviTamamlandiMi = true;
            HedeflerUI[0].gorevTamam.SetActive(true); //Top hedefi tamamlandiysa UI'da "Gorev Tamam" isaretini aktif eder.
            sesKaynagi.PlayOneShot(gorevTamamlamaSesi);

            toplamGorevSayisi--; //Toplam gorev sayisini azaltir.
            if (toplamGorevSayisi == 0)
            {
                Kazandin();
            }
        }
    }

    public void Kazandin()
    {
        Debug.Log("Kazandin");
        sesKaynagi.PlayOneShot(kazanmaSesi);
    }

    public void Kaybettin()
    {
        Debug.Log("Kaybettin");
        sesKaynagi.PlayOneShot(kaybetmeSesi);
    }

    public void GorevleriKontrolEt()
    {
        if (toplamGorevSayisi == 0)
            Kazandin();
        else
            Kaybettin();
    }
    public void TopBirlesmeSesi()
    {
        sesKaynagi.PlayOneShot(topBirlesmeSesi);
    }
}
