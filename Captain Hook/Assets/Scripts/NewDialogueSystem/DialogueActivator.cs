using UnityEngine;

public class DialogueActivator : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueObject dialogueObject;

    public void UpdateDialogueObject(DialogueObject dialogueObject)
    {
        this.dialogueObject = dialogueObject;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && other.TryGetComponent(out PlayerMovement playerMovement))
        {
            playerMovement.Interactable = this;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerMovement playerMovement))
        {
            if(playerMovement.Interactable is DialogueActivator dialogueActivator && dialogueActivator == this)
            {
                playerMovement.Interactable = null;
            }
        }
    }

    public void Interact(PlayerMovement playerMovement)
    {
        foreach(DialogueResponseEvents responseEvents in GetComponents<DialogueResponseEvents>())
        {
            if(responseEvents.DialogueObject == dialogueObject)
            {
                playerMovement.DialogueUI.AddResponseEvents(responseEvents.Events);
                break;
            }
        }

        playerMovement.DialogueUI.ShowDialogue(dialogueObject);
    }
}
