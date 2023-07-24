using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayerScript : MonoBehaviour
{
    public AudioSource src ;
    public AudioClip FL_On, FL_Off;

    public void FlashOn()
    {
        src.clip = FL_On;
        src.Play();
    }
    public void FlashOff()
    {
        src.clip = FL_Off;
        src.Play();
    }
}
