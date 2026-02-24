using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Top : MonoBehaviour
{
    public Dictionary<string, HingeJoint2D> HingeKontrol = new Dictionary<string, HingeJoint2D>(); /*Topa calisma aninda eklenen eklemlerin(jointlerin)
    referanslarini kaybetmemek ve onlara hizlica isimleriyle ulasabilmek icin "hafiza" olusturur.*/

    [SerializeField] private AudioManager audioManager;
    public void SonZinciriBagla(Rigidbody2D SonZincir, string HingeAdi)
    {
        HingeJoint2D joint = gameObject.AddComponent<HingeJoint2D>(); //Top nesnesine calisma aninda bir HingeJoint2D(eklem) bileseni ekler.
        HingeKontrol.Add(HingeAdi, joint); //Yeni olusturulan eklemi disaridan gelen isimle(HingeAdi) sozluge kaydeder.Boylece ismiyle geri cagirabilir.
        joint.autoConfigureConnectedAnchor = false; //Baglanti noktasinin Unity tarafindan otomatik belirlenmesini kapatir.
        joint.connectedBody = SonZincir; //Bu eklemin hangi nesneye baglanacagini belirler(Gelen son zincir halkasi).
        joint.anchor = Vector2.zero; //Eklemin "top" uzerindeki tutunma noktasini belirler(Vector2.zero = tam merkezi).
        joint.connectedAnchor = new Vector2(0f, -.2f); /*Eklemin "SonZincir" uzerindeki tutunma noktasini belirler.
        Zincirin tam merkezinden "zincirIleOlanMesafe" kadar asagiya baglar. */
    }
    private void OnCollisionEnter2D(Collision2D temas)
    {
        if(temas.gameObject.CompareTag("HedefObje"))
        {
            audioManager.ObjeyeCarpmaSesiCal();
        }
    }
}