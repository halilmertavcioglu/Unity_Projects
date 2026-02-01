using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CemberDonus : MonoBehaviour
{
    public float cember_donus_hizi;
    void Update()
    {
        transform.Rotate(0, 0, cember_donus_hizi* Time.deltaTime);
    }
}
