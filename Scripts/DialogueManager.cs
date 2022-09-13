using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences;


    public TMPro.TextMeshProUGUI nameText;
    public TMPro.TextMeshProUGUI dialogueText;
    public Image faceImage;

    public bool dialogueOn = true;

    public int countDown = 0;

    public Animator anim;

    public float textSpeed = 0.02f;

    public bool isOver = false;

    public bool dialogueWait = false;
    public bool triggerNext = false;
    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();

    }

    // Update is called once per frame
    void Update()
    {
        if((Input.GetKeyDown(KeyCode.Z)||Input.GetKeyDown(KeyCode.Return)) && dialogueOn) {
            DisplayNextSentence();
        }
    }

    public void StartDialogue(Dialogue dialogue, int chosenSentence = -1) {
        
        anim.SetBool("on", true);
        dialogueOn = true;
        isOver = false;
        
        nameText.text = dialogue.name;
        faceImage.sprite = dialogue.portrait;

        sentences.Clear();

        if(chosenSentence>-1) {
            sentences.Enqueue(dialogue.sentences[chosenSentence]);
        } else {
            foreach (string sentence  in dialogue.sentences)
            {
                sentences.Enqueue(sentence);
            }
        }

        

        

        DisplayNextSentence();


    }

    public void DisplayNextSentence() {
        if(sentences.Count==0) {
            EndDialogue();

            return;
        }

        string sentence = sentences.Dequeue();
        
        StopAllCoroutines();
        StartCoroutine(Type(sentence));
    }

    IEnumerator Type(string sentence) {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;

            if(countDown==0) {
                
                if(!isOver) {SoundManager.Instance.Play("Letter");}
                
                countDown = Random.Range(4,7);
            } else {
                countDown -= 1;
            }

            
            
            
            yield return new WaitForSeconds(textSpeed);
        }
    }

    public void EndDialogue() {
        anim.SetBool("on", false);
        dialogueOn = false;
        isOver = true;
        if(dialogueWait) {
            triggerNext = true;
            dialogueWait = false;
        }

        //GameMaster.Instance.Interaction(false);
    }
}
