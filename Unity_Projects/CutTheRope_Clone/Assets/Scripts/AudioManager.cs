using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    [Header("---Hoperlorler (Audio Sources)---")]
    [SerializeField] private AudioSource sesKaynagi;
    [SerializeField] private AudioSource anlikSesler;

    [Header("---Ses Dosyalari (Audio Clips)---")]
    [SerializeField] private AudioClip oyunGenelMuzigi;
    [SerializeField] private AudioClip objeyeCarpmaSesi;
    [SerializeField] private AudioClip kazanmaSesi;
    [SerializeField] private AudioClip kaybetmeSesi;
    [SerializeField] private AudioClip ipKesmeSesi;

    private void Start()
    {
        if(sesKaynagi != null && oyunGenelMuzigi != null)
        {
            sesKaynagi.clip = oyunGenelMuzigi;
            sesKaynagi.loop = true;
            sesKaynagi.Play();
        }
    }
    public void ObjeyeCarpmaSesiCal()
    {
        anlikSesler.PlayOneShot(objeyeCarpmaSesi);
    }
    public void KazanmaSesiCal()
    {
        anlikSesler.PlayOneShot(kazanmaSesi);
    }
    public void KaybetmeSesiCal()
    {
        anlikSesler.PlayOneShot(kaybetmeSesi);
    }
    public void IpKesmeSesiCal()
    {
        anlikSesler.PlayOneShot(ipKesmeSesi);
    }
}