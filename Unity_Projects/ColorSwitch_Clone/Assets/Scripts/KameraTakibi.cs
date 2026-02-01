using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KameraTakibi : MonoBehaviour
{
    public Transform top;
    void Update()
    {
        if(top.position.y > transform.position.y) // topun y pozisyonu, kameranýn y pozisyonundan büyük ise
        {
            transform.position = new Vector3(transform.position.x, top.position.y, transform.position.z);
        }
    }
}
