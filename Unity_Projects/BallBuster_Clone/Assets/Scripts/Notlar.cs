using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notlar : MonoBehaviour
{
    /*
     
     if(Input.GetMouseButton(0))
     {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if(hit.collider != null)
        {
            if(hit.collider.gameObject.tag == "OyunZemini")
            {
                Vector2 MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
     
                topAtici.transform.position = Vector2.MoveTowards(topAtici.transform.position, new Vector2(MousePosition.x,
                  topAtici.transform.position.y), 30 * Time.deltaTime);
            }
        }
     }
     
    KODUNUN ACIKLAMASI
    
    Oyuncu mouse ile ekrana tikladiginda, top atan karakteri(topAtici) parmagini takip edecek sekilde saga sola hareket ettirmektir.
    Bunu yaparken kafasina gore degil, oyun alaninda ekledigimiz Box Collider sinirlari kadar ve yatay eksende hareket yapar.

    ----------------------------------------------------------------------------------------------------------------------------------------------------------------

    Ray ray = ... : Kameradan mouse'nin oldugu yere doğru sanal bir isin(ray) olusturuyor. 

    ----------------------------------------------------------------------------------------------------------------------------------------------------------------

    RaycastHit2D hit = ... : Bu isin(ray) oyun dunyasinda(2D fizikte) bir şeye çarpti mi diye bakiliyor.

    ----------------------------------------------------------------------------------------------------------------------------------------------------------------

    topAtici.transform.position = Vector2.MoveTowards(topAtici.transform.position, new Vector2(MousePosition.x,topAtici.transform.position.y), 30 * Time.deltaTime);
    Mouse yukarida olsa bile Y yuksekligi asla degismez. Sadece ray uzerinde kayar gibi saga-sola hareket eder.

    ----------------------------------------------------------------------------------------------------------------------------------------------------------------
    MoveTowards(mevcutPozisyon,hedefPozisyon,hiz): Bir objeyi, belirlenen hedefe dogru, adim adim yaklastirir.

    1️- topAtici.transform.position (mevcutPozisyon): Top atici su an nerede?

    2️- new Vector2(MousePosition.x, topAtici.transform.position.y) (hedefPozisyon):

    X -> Mouse'nin oldugu yere git.
    Y -> sabit(degismiyor)
    Boylece sadece saga-sola gitmesini sagliyoruz.

    3- 30 * Time.deltaTime (hiz): FPS artsa da azalsa da ayni hizda kalir.
     
     */
}
