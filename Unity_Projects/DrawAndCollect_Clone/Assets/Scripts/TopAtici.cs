using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopAtici : MonoBehaviour
{
    [SerializeField] private GameObject[] toplar;
    [SerializeField] private GameObject topAtarMerkezi;
    [SerializeField] private GameObject kova;
    [SerializeField] private GameObject[] kovaSpawnNoktalari;
    [SerializeField] private AudioSettings sesSistemi;
    int aktifTopIndex;
    int randomKovaIndex;
    bool kilit;

    public void OyunBaslasin()
    {
        StartCoroutine(TopAtisSistemi());
    }
    IEnumerator TopAtisSistemi()
    {
        while (true)
        {
            if (!kilit)
            {
                yield return new WaitForSeconds(0.5f);

                //Aktif topu spawn noktasina tasiyoruz ve aktif ediyoruz.
                toplar[aktifTopIndex].transform.position = topAtarMerkezi.transform.position;
                toplar[aktifTopIndex].SetActive(true);

                //Quaternion.AngleAxis ile Z ekseni etrafýnda bir rotasyon oluþturuyoruz.
                //Bu rotasyonu Vector3.right ile çarparak sað yönü belirtilen açý kadar döndürmüþ oluyoruz.
                //Böylece top için rastgele bir atýþ yönü elde ediyorum.
                Vector3 pos = Quaternion.AngleAxis(AciVer(70f, 110f), Vector3.forward) * Vector3.right;

                //Rigidbody2D'ye kuvvet uygulanarak topu firlatiyoruz.
                toplar[aktifTopIndex].GetComponent<Rigidbody2D>().AddForce(750 * pos);

                if (aktifTopIndex != toplar.Length - 1)
                    aktifTopIndex++;
                else
                    aktifTopIndex = 0;

                yield return new WaitForSeconds(0.5f);
                randomKovaIndex = Random.Range(0, kovaSpawnNoktalari.Length - 1);
                kova.transform.position = kovaSpawnNoktalari[randomKovaIndex].transform.position;
                kova.SetActive(true);
                kilit = true;
                Invoke("TopuKontrolEt", 5f);
            }
            else
            {
                yield return null;
            }
        }
    }
    public void DevamEt()
    {
        kilit = false;
        kova.SetActive(false);
        CancelInvoke();
    }
    float AciVer(float deger1, float deger2)
    {
        return Random.Range(deger1, deger2);
    }
    void TopuKontrolEt()
    {
        if (kilit)
            GetComponent<GameManager>().OyunBitti();
    }
}