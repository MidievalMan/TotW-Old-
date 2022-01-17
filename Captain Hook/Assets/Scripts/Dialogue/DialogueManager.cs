using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DialogueTypeEnum;
using System;

public class DialogueManager : MonoBehaviour
{

    private Queue<string> sentences;
    PlayerMovement player;
    GrapplingHook grapplingHook;

    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public TMP_Text shopText;
    public Image[] textBoxes;


    private int typeOfDialogue;
    private DialogueType dialogueType;
    private float enterPauseLength = 0.5f;

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        grapplingHook = FindObjectOfType<GrapplingHook>();

        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue, DialogueType type, float speed)
    {
        player.SetMasterControl(false);
        dialogueType = type;

        if(dialogueType == DialogueType.Thought || dialogueType == DialogueType.Sign || dialogueType == DialogueType.NPC) // not in shop
        {
            dialogueText.gameObject.SetActive(true);

            if(type == DialogueType.Thought)
            {
                //new Color(13, 6, 42, 255); ideal color
                dialogueText.color = Color.black;
                textBoxes[0].gameObject.SetActive(true);
            }
            else if(type == DialogueType.Sign)
            {
                //new Color(13, 6, 42, 255); ideal color
                dialogueText.color = Color.black;
                textBoxes[1].gameObject.SetActive(true);
            }
            else if (type == DialogueType.NPC)
            {
                //new Color(254, 215, 207, 255); ideal color
                dialogueText.color = Color.white;
                textBoxes[2].gameObject.SetActive(true);
            }

        } else // in shop
        {
            shopText.gameObject.SetActive(true);
        }

        nameText.gameObject.SetActive(true);
        nameText.SetText(dialogue.name);

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence(speed); // first sentence
    }

    public void DisplayNextSentence(float speed)
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence, speed));
    }

    IEnumerator TypeSentence(string sentence, float speed)
    {

        dialogueText.text = "";
        shopText.text = "";
        SoundManager.PlaySound(SoundManager.Sound.Sign);
        foreach (char letter in sentence.ToCharArray())
        {

            dialogueText.text += letter;
            shopText.text += letter;

            if (letter == '\n')
            {
                yield return new WaitForSeconds(enterPauseLength);
            }
            else
            {
                yield return new WaitForSeconds(speed);
            }

        }
    }

    public void EndDialogue()
    {

        // might have to stop all coroutines here to stop text from appearing after exiting shop
        nameText.gameObject.SetActive(false);
        dialogueText.gameObject.SetActive(false);
        shopText.gameObject.SetActive(false);

        foreach (Image textBox in textBoxes)
        {
            textBox.gameObject.SetActive(false);
        }

        player.SetMasterControl(true);
    }

}
