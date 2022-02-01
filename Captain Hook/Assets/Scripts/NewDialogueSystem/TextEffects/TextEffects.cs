using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TextFX;

public class TextEffects : MonoBehaviour
{
    public EffectType effectType = EffectType.None;

    private void Update()
    {
        switch(effectType)
        {
            case EffectType.None:
                //execute nothing here
                //Debug.Log("NONE");
                break;
            case EffectType.Wave:
                //do wave stuff here
                Debug.Log("WAVE");
                break;
            case EffectType.Shake:
                //do shake stuff here
                Debug.Log("SHAKE");
                break;
            default:
                Debug.LogError("Reached Default case in TextEffects Class");
                break;
        }
    }
}
