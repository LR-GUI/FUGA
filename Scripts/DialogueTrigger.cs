using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    // Put this into the object wanting to fire the dialogue
    public Dialogue dialogue;
    
    public void TriggerDialogue(int chosenSentence=-1) {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, chosenSentence);
    }
}
