using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using DialogueTypeEnum;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public DialogueType dialogueType;
    public float dialogueSpeed = 0.002f;

    public GameObject[] gameObjectsToActivate;
    public GameObject[] gameObjectsToDeactivate;

    public void ActivateOrDeactivateGOs()
    {
        for (int i = 0; i < gameObjectsToActivate.Length; i++)
        {
            gameObjectsToActivate[i].SetActive(true);
        }
        for (int i = 0; i < gameObjectsToDeactivate.Length; i++)
        {
            gameObjectsToDeactivate[i].SetActive(false);
        }
    }


    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, dialogueType, dialogueSpeed);
        ActivateOrDeactivateGOs();
    }

    public void TriggerNextSentence()
    {
        FindObjectOfType<DialogueManager>().DisplayNextSentence(dialogueSpeed);
    }

    public void TriggerShopDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, dialogueType, dialogueSpeed);
    }
}
