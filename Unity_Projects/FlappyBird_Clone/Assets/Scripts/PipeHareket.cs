using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeHareket : MonoBehaviour
{
    public float pipeHiz;


    void Update()
    {
        transform.position += Vector3.left * pipeHiz * Time.deltaTime; 
    }
}
