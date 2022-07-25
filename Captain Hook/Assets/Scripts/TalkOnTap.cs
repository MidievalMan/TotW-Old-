using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TalkOnTap : MonoBehaviour
{
    private static bool triggerDialogue = false;

    private DialogueTrigger dialogueTrigger;
    private int totalNumSentences;
    private int remainingSentences;

    public GameObject arrow;
    private TMP_Text text;
    public bool needShopTextReference;

    private void Start()
    {
        dialogueTrigger = GetComponent<DialogueTrigger>();
        totalNumSentences = dialogueTrigger.dialogue.sentences.Length;
        remainingSentences = totalNumSentences;

        if (needShopTextReference)
        {
            text = GameObject.FindWithTag("ShopText").GetComponent<TMP_Text>();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            triggerDialogue = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            arrow.SetActive(true);
            //text.gameObject.SetActive(true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("Player"))
        {            
            if (triggerDialogue)
            {
                if (remainingSentences == totalNumSentences)
                {
                    dialogueTrigger.TriggerDialogue();
                    remainingSentences--;
                }
                else
                {
                    dialogueTrigger.TriggerNextSentence();
                    remainingSentences--;
                }

                if(remainingSentences <= -1)
                {
                    remainingSentences = totalNumSentences;
                }
            }
            triggerDialogue = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        arrow.SetActive(false);
        //text.gameObject.SetActive(false);
    }
}
