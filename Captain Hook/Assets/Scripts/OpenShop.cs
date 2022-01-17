using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenShop : MonoBehaviour
{
    public GameObject shopUI;
    public PlayerMovement playerMovement;

    public DialogueTrigger dialogueTrigger;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Input.GetKey(KeyCode.W))
        {
            shopUI.SetActive(true);
            if (playerMovement.GetMasterControl())
            {
                SoundManager.PlaySound(SoundManager.Sound.EnterDoor);
            }
            playerMovement.SetMasterControl(false);

            dialogueTrigger.TriggerShopDialogue();
        }

    }
}
