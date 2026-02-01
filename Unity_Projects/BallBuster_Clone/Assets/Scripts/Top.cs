using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.CompilerServices;

public class Top : MonoBehaviour
{
    [SerializeField] private int sayi;
    [SerializeField] TextMeshProUGUI sayiText;
    [SerializeField] GameManagerr gameManager;
    [SerializeField] ParticleSystem birlesmeEfekti;
    [SerializeField] SpriteRenderer _Renderer;
    [SerializeField] private bool varsayilanTop;
    bool birincil;
    void Start()
    {
        sayiText.text = sayi.ToString(); //Topun baslangictaki sayi degerini yaziya aktarir. 
        gameObject.tag = sayi.ToString(); //Carpisma kontrolu için objenin tagini, sayisi ile ayni yapar.

        if (varsayilanTop)
            birincil = true; //Baslangicta VarsayilanToplar birlesmeye hazir olsun.
    }
    public void DurumuAyarla()
    {
        birincil = true; //Topu tekrar birleþebilir hale getiren yardimci fonksiyon.
    }
    public void BirincilDurumDegistir()
    {
        Invoke("DurumuAyarla", 0.1f);//Bu fonksiyon, topun firlatildiktan sonra belli bir sure sonra birleþebilir olmasini saglar.
    }
    private void OnCollisionEnter2D(Collision2D temas)
    {
        BirlesmeKontrolu(temas);// Bu fonksiyon ilk carpisma oldugunda birlesme olup olmadigini kontrol eder.
    }
    private void OnCollisionStay2D(Collision2D temas)
    {
        BirlesmeKontrolu(temas); //Ayni sayidaki toplar birbirine deðmeye devam ediyorsa, birlesme islemini zorla/garantiye al.
    }

    private void BirlesmeKontrolu(Collision2D temas)
    {
        if (temas.gameObject.GetComponent<Bomba>() != null) return; //Carpisilan obje bir bomba ise, birlesme islemini durdur, bomba patlasin.
        if(temas.gameObject.CompareTag(sayi.ToString())&&birincil)
        {
            birlesmeEfekti.Play(); //carpismada birlesme particle efektini oynatir.
            temas.gameObject.SetActive(false); //carpismis oldugumuz objeyi "false" diyerek pasiflestiriyoruz.
            sayi *= 2; //iki obje birlestiginde degerini ikiye katliyoruz.
            gameObject.tag = sayi.ToString(); // birlesen objeye yeni tag veriyoruz.
            sayiText.text = sayi.ToString(); // en son sayisal deger ne olduysa topa yazmasini sagliyoruz. 
            gameManager.TopBirlesmeSesi();

            switch (sayi)
            {
                /*case 2:
                    _Renderer.sprite = gameManager.spriteObjeleri[0];
                    break;*/

                case 4:
                    _Renderer.sprite = gameManager.spriteObjeleri[1];
                    break;

                case 8:
                    _Renderer.sprite = gameManager.spriteObjeleri[2];
                    break;

                case 16:
                    _Renderer.sprite = gameManager.spriteObjeleri[3];
                    break;

                case 32:
                    _Renderer.sprite = gameManager.spriteObjeleri[4];
                    break;

                case 64:
                    _Renderer.sprite = gameManager.spriteObjeleri[5];
                    break;

                case 128:
                    _Renderer.sprite = gameManager.spriteObjeleri[6];
                    break;

                case 256:
                    _Renderer.sprite = gameManager.spriteObjeleri[7];
                    break;

                case 512:
                case 1024:
                case 2048:
                    _Renderer.sprite = gameManager.spriteObjeleri[8];
                    break;
            }

            /*Top her buyudugunde, bu yeni sayinin bolumu bitirmek için gerekli olan "hedef sayi" 
            olup olmadigini kontrol etmektir. Her bolumde top birlestirme gorevi olmayabilir.
            Eger "topHedefiVarMi" degeri false ise, bos yere kontrol yapip islemciyi yormaz. */
            if (gameManager.topHedefiVarMi) 
            {
                gameManager.GorevSayiKontrolu(sayi); //Yeni olusan sayiyi hedefle karsilastirmasi icin GameManager'a gonder.
            }
            birincil = false; //Birlesme bittiginde birden fazla eslesmeyi onlemek icin yetkiyi kapat.
            Invoke(nameof(DurumuAyarla), 0.1f); //0.1 saniye sonra topu yeni birleþmeler için tekrar hazýr et
        }
    }
}
