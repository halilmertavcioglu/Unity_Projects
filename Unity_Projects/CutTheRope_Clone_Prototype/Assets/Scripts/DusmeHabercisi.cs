using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DusmeHabercisi : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    private void OnTriggerEnter2D(Collider2D temas)
    {
        if(temas.CompareTag("HedefObje"))
        {
            gameManager.HedefObjeDustu();
        }
        else if(temas.CompareTag("Top"))
        {
            gameManager.TopDustu();
        }
    }
}
