using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSpawn : MonoBehaviour
{
    public float pipeSpawnlama;
    public float sayac = 0;
    public GameObject pipe;
    public float rastgeleYukseklik;
    void Start()
    {
        GameObject yeniPipe = Instantiate(pipe);
        yeniPipe.transform.position = transform.position + new Vector3(0, Random.Range(-rastgeleYukseklik, rastgeleYukseklik), 0);
        Destroy(yeniPipe, 10);
    }

    void Update()
    {
        if(sayac > pipeSpawnlama)
        {
            GameObject yeniPipe = Instantiate(pipe);
            yeniPipe.transform.position = transform.position + new Vector3(0, Random.Range(-rastgeleYukseklik,rastgeleYukseklik), 0);
            Destroy(yeniPipe, 10);
            sayac = 0;
        }
        sayac += Time.deltaTime;
    }
}
