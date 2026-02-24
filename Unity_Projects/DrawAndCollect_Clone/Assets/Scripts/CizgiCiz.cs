using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CizgiCiz : MonoBehaviour
{
    //Cizilecek cizgi icin kullanilacak prefab(Onceden hazirlanmis bir GameObject).
    public GameObject linePrefab;

    //O an cizilen cizgiyi temsil edecek GameObject referansi.
    public GameObject cizgi;

    //Cizgiyi gorsel olarak ekranda gosterecek LineRenderer bileseni.
    public LineRenderer lineRenderer;

    //Cizgiye carpisma eklemek icin EdgeCollider2D bileseni. Fiziksel etkilesim saglar.
    public EdgeCollider2D edgeCollider;

    //Cizgi boyunca kaydedilen noktalarin listesi. Cizgiyi olustururken ve guncellerken kullanilir.
    public List<Vector2> parmakPozisyonListesi;
    public List<GameObject> cizgiler;
    bool cizmekMumkunmu;
    [SerializeField] private AudioSettings sesSistemi;
    [SerializeField] private TextMeshProUGUI cizmeHakkiText;

    int cizmeHakki;

    private void Start()
    {
        cizmekMumkunmu = false;
        cizmeHakki = 3;
        cizmeHakkiText.text = cizmeHakki.ToString(); 
    }

    void Update()
    {
        if(cizmekMumkunmu == true && Time.timeScale != 0 && cizmeHakki != 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                CizgiOlustur();
            }
            if (Input.GetMouseButton(0))
            {
                if (lineRenderer)
                {
                    // Son eklenen noktadan 0.1 birimden fazla uzaksa, yeni nokta ekle.
                    // [^1] listenin son elemanini ifade eder (C# 8.0 ile gelen indeksleme).
                    Vector2 parmakPozisyonu = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    if (Vector2.Distance(parmakPozisyonu, parmakPozisyonListesi[^1]) > .1f)

                    {
                        CizgiyiGuncelle(parmakPozisyonu);
                    }
                }
            }
        }
        if(cizgiler.Count != 0 && cizmeHakki != 0)
        {
            if(Input.GetMouseButtonUp(0))
            {
                cizmeHakki--;
                cizmeHakkiText.text = cizmeHakki.ToString();
            }
        }
    }
    void CizgiOlustur()
    {
        //Prefab'den yeni bir cizgi olusturur. posizyon(0, 0), donus yok.
        cizgi = Instantiate(linePrefab, Vector2.zero, Quaternion.identity);

        cizgiler.Add(cizgi);

        //Yeni cizgiden LineRenderer bilesenini alir.
        lineRenderer = cizgi.GetComponent<LineRenderer>();

        //Yeni cizgiden EdgeCollider2D bilesenini alir.
        edgeCollider = cizgi.GetComponent<EdgeCollider2D>();

        //Onceki cizgilerdeki noktalari temizler.
        parmakPozisyonListesi.Clear();

        //Cizgi baslamak icin ayni pozisyonu iki kez ekler.
        parmakPozisyonListesi.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        //Cizgi iki noktadan olusmali.
        parmakPozisyonListesi.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));  

        //LineRenderer'a baslangic ve bitis noktalarini ayarlar.
        lineRenderer.SetPosition(0, parmakPozisyonListesi[0]);
        lineRenderer.SetPosition(1, parmakPozisyonListesi[1]);

        //EdgeCollider'i baslangic noktalarina gore ayarlar.
        edgeCollider.points = parmakPozisyonListesi.ToArray(); 
    }
    void CizgiyiGuncelle(Vector2 GelenParmakPozisyonu)
    {
        //Yeni parmak pozisyonunu listeye ekler.
        parmakPozisyonListesi.Add(GelenParmakPozisyonu);

        //LineRenderer'in toplam nokta sayisini bir arttirir.
        lineRenderer.positionCount++;

        //LineRenderer'in son noktasini gunceller.
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, GelenParmakPozisyonu);

        //EdgeCollider'i yeni noktalarla tekrar ayarlar.
        edgeCollider.points = parmakPozisyonListesi.ToArray(); 
    }
    public void DevamEt()
    {
        foreach (var item in cizgiler)
        {
            Destroy(item.gameObject);
        }
        cizgiler.Clear();
        cizmeHakki = 3;
        cizmeHakkiText.text = cizmeHakki.ToString();
    }
    public void CizmeyiDurdur()
    {
        cizmekMumkunmu = false;
    }
    public void CizmeyiBaslat()
    {
        cizmeHakki = 3;
        cizmekMumkunmu = true;
    }
}
