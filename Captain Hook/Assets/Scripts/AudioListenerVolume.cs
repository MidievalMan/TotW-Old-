using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioListenerVolume : MonoBehaviour
{
    public float volume;
    void Start()
    {
        AudioListener.volume = volume;
    }
    
}
