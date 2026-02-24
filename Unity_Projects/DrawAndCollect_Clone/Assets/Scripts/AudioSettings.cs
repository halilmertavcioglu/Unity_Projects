using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AudioSettings : MonoBehaviour
{
    [Header("---Hoperlorler (Audio Sources)---")]
    [SerializeField] private AudioSource sesKaynagi;
    [SerializeField] private AudioSource anlikSesler;

    [Header("---Ses Dosyalari (Audio Clips)---")]
    [SerializeField] private AudioClip oyunGenelMuzigi;
    [SerializeField] private AudioClip oyunBitisSesi;
    [SerializeField] private AudioClip kovayaSokmaSesi;
    [SerializeField] private AudioClip topSekmeSesi;

    private void Start()
    {
        if(sesKaynagi != null && oyunGenelMuzigi != null)
        {
            sesKaynagi.clip = oyunGenelMuzigi;
            sesKaynagi.loop = true;
            sesKaynagi.Play();
        }
    }
    public void TopSekmeSesiCal()
    {
        anlikSesler.PlayOneShot(topSekmeSesi);
    }
    public void KovaSokmaSesiCal()
    {
        anlikSesler.PlayOneShot(kovayaSokmaSesi);
    }
    public void OyunBitisSesiCal()
    {
        anlikSesler.PlayOneShot(oyunBitisSesi);
    }
    public void MuzigiDurdur()
    {
        sesKaynagi.Stop();
    }
}