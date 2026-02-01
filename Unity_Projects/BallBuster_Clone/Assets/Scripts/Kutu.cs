using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kutu : MonoBehaviour
{
    [SerializeField] GameManagerr gameManager;

    public void EfektOynat()
    {
        gameManager.KutuParcalamaEfekt(transform.position);
        gameObject.SetActive(false);
    }
}
