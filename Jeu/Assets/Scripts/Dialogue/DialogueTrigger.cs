using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField]
    DialogueSystem dialogueSystem;

    private bool oneTimeTrigger = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.name.Equals("Head") || collision.gameObject.CompareTag("Player")) && !oneTimeTrigger)
        {
            dialogueSystem.ShowDialogue();
            oneTimeTrigger = true;
        }
    }
}
