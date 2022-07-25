using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TextFX;
using TMPro;

public class TextEffects : MonoBehaviour
{
    public EffectType effectType = EffectType.None;

    public void ChooseTextEffect(EffectType effectType, TMP_Text textLabel, string textToType, int startIndexForEffect)
    {
        this.effectType = effectType;
        switch (effectType)
        {
            case EffectType.None:

                //execute nothing here
                //Debug.Log("NONE");
                break;
            case EffectType.Wave:

                StartCoroutine(Wave(textLabel, textToType, startIndexForEffect));

                break;
            case EffectType.Shake:

                Shake();

                break;
            default:
                Debug.LogError("Reached Default case in TextEffects Class");
                break;
        }
    }

    private IEnumerator Wave(TMP_Text textLabel, string textToType, int startIndexForEffect)
    {
        
        Debug.Log("WAVE");
        yield return null;
    }

    private void Shake()
    {
        Debug.Log("SHAKE");
    }
}
