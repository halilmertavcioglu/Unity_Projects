using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notlar : MonoBehaviour
{
    /*
        public class Donus : MonoBehaviour
       {
        private float beklemeSuresi = 0.05f;
        void Update()
        {
            StartCoroutine(AltigenDonus()); 
        }

        IEnumerator AltigenDonus()
        {
            Vector2 farePozisyonu = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, 0));

            if (Input.GetMouseButtonDown(0) && farePozisyonu.x > 0) //sağa döndürmek için
            {
                transform.Rotate(0, 0, -30);
                yield return new WaitForSeconds(beklemeSuresi); //Daha kademeli bir dönüş yapmak için 
                transform.Rotate(0, 0, -30);
            }
            else if (Input.GetMouseButtonDown(0) && farePozisyonu.x < 0) //sola döndürmek için
            {
                transform.Rotate(0, 0, 30);
                yield return new WaitForSeconds(beklemeSuresi); //Daha kademeli bir dönüş yapmak için
                transform.Rotate(0, 0, 30);
            }
        }
       }
    !!!!!Vector2 farePozisyonu = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, 0));!!!!!


    1- ScreenToWorldPoint() --> Ekrandaki(piksel cinsinden) bir konumu oyun dünyasındaki gerçek konuma(Unity'de 2D/3D koordinatları) çevirir.


    2- Vector2 --> Unity'nin 2 boyutlu vektör sınıfıdır.


    3- Input.mousePosition.x --> Fare imlecinin ekrandaki x eksenindeki konumunu verir.(Altıgeni döndürme işlemi için yapılır.)


    4- new Vector2 --> Farenin ekrandaki X konumunu alıp, sahnedeki gerçek dünya konumuna dönüştürmek için kullanılan bir ifadedir.
    Input.mousePosition.x kullanılmasının amacı, farenin ekrandaki yatay (X ekseni) konumunu almak içindir.
    */

    //!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!
    //!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!
    //!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!
    //!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!
    //!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!
    //!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!
    //!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!
    //!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!


    /*
    public class Top : MonoBehaviour
    {
        private Rigidbody2D topRb;
        private SpriteRenderer topSr;
        public float ziplamaGucu;
        void Start()
     {
            topRb = GetComponent<Rigidbody2D>();
            topSr = GetComponent<SpriteRenderer>();
        }
    
        private void OnCollisionEnter2D(Collision2D temas)
     {
            if (temas.gameObject.tag == "Kenar")
            {
                topRb.AddForce(Vector2.up * ziplamaGucu, ForceMode2D.Impulse);
            }
        }
    }
    

    1- GetComponent<> ---> Topun fiziksel hareketlerine ve görünümüne koddan erişmemizi sağlar.


    2- private void OnCollisionEnter2D(Collision2D temas) ---> Unity'nin özel bir fizik olayı fonksiyonudur.
    Bu fonksiyon, 2D fizik motorunda çarpışma olduğu anda otomatik çalışır. 


    3- if (temas.gameObject.tag == "Kenar") --> Çarpılan nesnenin tag’ini kontrol eder.
    temas --> Çarpışma bilgisidir.
    temas.gameObject --> Çarptığı nesnenin kendisidir.
    .tag --> O nesnenin etiketini verir.

    "Eğer topun çarptığı nesnenin etiketi ‘Kenar’ ise, aşağıdaki işlemi yap." demektir.


    4- topRb.AddForce(Vector2.up * ziplamaGucu, ForceMode2D.Impulse); --> Topa yukarı yönde zıplama kuvveti uygular.

    .AddForce() --> RigidBody2D'ye kuvvet uygular.
    Vector2.up --> Yukarı yönü temsil eder.
    *ziplamaGucu --> Uygulanan kuvvetin büyüklüğünü belirler.
    ForceMode2D.Impulse --> Çarpışma için uygulanacak kuvvetin "ani" uygulanacağını belirtir.(Sürekli değil tek seferde güçlü vuruş)

    private void OnTriggerEnter2D(Collider2D TemasT) --> Bir nesne, “Is Trigger” kutusu işaretli bir alana girdiğinde çalışan metottur.
    Yani iki nesne temas ettiğinde ama çarpışmadan, sadece tetikleme olduğunda çalışır.

    //!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!
    //!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!
    //!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!
    //!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!
    //!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!
    //!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!
    //!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!
    //!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!*!


    if (topSr.color == temas.gameObject.GetComponent<SpriteRenderer>().color)
    {

    }
    else if(topSr.color != temas.gameObject.GetComponent<SpriteRenderer>().color)
    {
           SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    Kodun Mantığı;
    1 SceneManager.GetActiveScene() → şu anki sahneyi bul
    2 .buildIndex → o sahnenin numarasını al
    3 SceneManager.LoadScene(...) → o numaralı sahneyi tekrar yükle

    1- temas.gameObject.GetComponent<SpriteRenderer>().color --> “Temas edilen nesnenin SpriteRenderer bileşenindeki renk değerini al.” demektir.

    2- SceneManager --> Unity'de sahneleri yönetmek için kullanılır. 
    Kullanırken "using Unity.Engine.SceneManagement" kütüphanesinin ekli olması gerekir.

    3- SceneManager.GetActiveScene() --> "Şu anda aktif olan sahnenin bilgisini getir." demektir. 

    4- .buildIndex --> Bu scene sahnesinin özelliğidir.

    5- SceneManager.LoadScene() --> Bu fonksiyonun içine, sahne adı ve build index yazılır ve o sahneyi yükler.

    6- r, g, b nedir?

    Unity’de renk (Color) dediğimiz şey aslında 3 (bazen 4) sayının birleşimidir:

    r → red (kırmızı oranı)
    g → green (yeşil oranı)
    b → blue (mavi oranı)

    Diğer renkler, bu 3 rengin türevi olduğu için r, g, b kullanılır.

    PlayerPrefs --> PlayerPrefs, Unity’nin küçük verileri (ayarlar, skorlar, vb.) cihazda saklamak için sunduğu basit bir sınıftır.

    PlayerPrefs.SetInt(SetFloat/SetString) --> Tam sayı değerlerini kaydeder.

    PlayerPrefs.GetInt(GetFloat/GetString) --> Kaydedilen tam sayı değerini geri alarak yeniden paylaşır.

    PlayerPrefs.HasKey("") --> Belirtilen anahtarın önceden kaydedilip kaydedilmediğine bakar.

    */
}
