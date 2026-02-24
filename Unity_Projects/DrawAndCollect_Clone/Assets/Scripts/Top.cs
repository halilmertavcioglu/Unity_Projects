using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Top : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private AudioSettings sesSistemi;
    private void OnTriggerEnter2D(Collider2D temas)
    {
        if(temas.gameObject.CompareTag("TopGirdi"))
        {
            gameObject.SetActive(false);

            if(gameObject.CompareTag("Bomba"))
            {
                gameManager.OyunBitti();
            }
            else
            {
                gameManager.DevamEt(transform.position);
                sesSistemi.KovaSokmaSesiCal();
            }
            
        }
        else if (temas.gameObject.CompareTag("OyunBitti"))
        {
            gameObject.SetActive(false);

            if (gameObject.CompareTag("Bomba"))
            {
                gameManager.DevamEt(transform.position);
                sesSistemi.KovaSokmaSesiCal();
            }
            else
            {
                gameManager.OyunBitti();
            }
        }
    }
}