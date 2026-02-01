using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KareDonusHizi : MonoBehaviour
{
    public float kare_donus_hizi;
    void Update()
    {
        transform.Rotate(0, 0, kare_donus_hizi * Time.deltaTime);
    }
}
