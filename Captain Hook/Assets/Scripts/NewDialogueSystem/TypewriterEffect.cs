using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using TextFX;

public class TypewriterEffect : MonoBehaviour
{
    [SerializeField] private float typewriterSpeed = 50f;

    public bool IsRunning { get; private set; }

    private readonly List<Punctuation> punctuations = new List<Punctuation>()
    {
        new Punctuation(new HashSet<char>() {'.', '!', '?'}, 0.6f),
        new Punctuation(new HashSet<char>() {',', ';', ':'}, 0.3f)
    };

    private Coroutine typingCoroutine;

    //
    private TextEffects textEffects;
    private void Start()
    {
        textEffects = GetComponent<TextEffects>();
    }
    //

    public void Run(string textToType, TMP_Text textLabel)
    {
        typingCoroutine = StartCoroutine(TypeText(textToType, textLabel));
    }

    public void Stop()
    {
        StopCoroutine(typingCoroutine);
        IsRunning = false;
    }

    private IEnumerator TypeText(string textToType, TMP_Text textLabel)
    {
        //
        int charactersLeft = textToType.Length;
        //
        IsRunning = true;
        textLabel.text = string.Empty;

        float t = 0;
        int charIndex = 0;
        while(charIndex < textToType.Length)
        {
            int lastCharIndex = charIndex;

            t += Time.deltaTime * typewriterSpeed;

            charIndex = Mathf.FloorToInt(t);
            charIndex = Mathf.Clamp(charIndex, 0, textToType.Length);

            for(int i = lastCharIndex; i < charIndex; i++)
            {
                bool isLast = i >= textToType.Length - 1;

                //
                if (charactersLeft > 2 && IsTag(textToType[i]) && IsTag(textToType[i + 1]))
                {
                    /* var of type TextEffects = */
                    Debug.Log("RAN");
                    textEffects.effectType = WhichTag(textToType, i);
                }
                //

                textLabel.text = textToType.Substring(0, i + 1);



                if(IsPunctuation(textToType[i], out float waitTime) && !isLast && !IsPunctuation(textToType[i + 1], out _))
                {
                    yield return new WaitForSeconds(waitTime);
                }
                //
                charactersLeft--;
                //Debug.Log("Characters Left: " + charactersLeft);
                //
            }

            yield return null;
        }

        IsRunning = false;
    }
    //
    private bool IsTag(char character)
    {
        if(character == '%') { return true; }
        return false;
    }

    private EffectType WhichTag(string textToType, int currentIndex)
    {
        Debug.Log(textToType[currentIndex + 2]);
        switch(textToType[currentIndex + 2])
        {
            case ('W'):
                return EffectType.Wave;
            case ('S'):
                return EffectType.Shake;
            default:
                return EffectType.None;
        }
    }
    //
    

    private bool IsPunctuation(char character, out float waitTime)
    {
        foreach(Punctuation punctuationCategory in punctuations)
        {
            if(punctuationCategory.Punctuations.Contains(character))
            {
                waitTime = punctuationCategory.WaitTime;
                return true;
            }
        }

        waitTime = default;
        return false;
    }

    private readonly struct Punctuation
    {
        public readonly HashSet<char> Punctuations;
        public readonly float WaitTime;

        public Punctuation(HashSet<char> punctuations, float waitTime)
        {
            Punctuations = punctuations;
            WaitTime = waitTime;
        }
    }
}
