using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KameraTakibi : MonoBehaviour
{
    public Transform karakter;
    public float kameraTakipHizi;
    private void Update()
    {
       if(karakter.position.y > transform.position.y)
        {
            Vector3 hedefPozisyon = new Vector3(transform.position.x, karakter.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, hedefPozisyon, kameraTakipHizi);
        }
    }
}
