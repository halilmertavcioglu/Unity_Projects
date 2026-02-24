using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ip_yonetimi : MonoBehaviour
{
    [SerializeField] private Rigidbody2D ilkKanca; //Topun tavana veya sabit bir noktaya baglantigi ilk fiziksel govde(kanca). 
    [SerializeField] private Top top; //Ipin ucuna takilacak top nesnesinin script referansi.
    [SerializeField] private int baglantiSayisi = 5; //Baslangic ve bitis arasindaki halka/eklem sayisi;
    public GameObject[] baglantiHavuzu;
    public string HingeAdi; //Topun baglandigi ozel ipe verilecek isim(Dictionary icin anahtar).
    void Start()
    {
        IpOlustur();
    }
    void IpOlustur()
    {
        Rigidbody2D oncekiBaglanti = ilkKanca; //Zincirleme islemi icin o anki halkayi bir sonrakine baglayacak olan gecici degisken.
        //Baslangicta bu, en tepedeki sabit kancadir.

        for (int i = 0; i < baglantiSayisi; i++)
        {
            baglantiHavuzu[i].SetActive(true); //Baglanti havuzundaki i. siradaki baglantiyi gorunur hale getirir.
            HingeJoint2D joint = baglantiHavuzu[i].GetComponent<HingeJoint2D>(); // Olusturulan halkanin uzerindeki eklem(joint) bilesenini al.
            joint.connectedBody = oncekiBaglanti; // Bu halkayi, bir onceki halkaya fiziksel olarak bagla.
            // O an olusturulan ip parcasini, kendisinden onceki halkaya fiziksel olarak dugumler.

            if (i < baglantiSayisi - 1) //Eger olusturulan halka, listenin son halkasi degilse
            {
                oncekiBaglanti = baglantiHavuzu[i].GetComponent<Rigidbody2D>(); /*Bir onceki halkanin buna baglanabilmesi icin "oncekiBaglanti"yi
                                                                       bu halka olarak guncelle.*/
            }
            else //son halkasi ise
            {
                top.SonZinciriBagla(baglantiHavuzu[i].GetComponent<Rigidbody2D>(), HingeAdi); /*Topu son halkaya baglamak icin Top scriptindeki
                fonksiyonu, halka bilgisi ve ip ismiyle cagir.*/
            }
        }
    }
}
