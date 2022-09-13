using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator anim;
    public string state="DoorIdle";
    private bool scientistTalk = true;
    // Start is called before the first frame update
    void Start()
    {
        anim = transform.GetChild(0).gameObject.GetComponent<Animator>();
        GameMaster.Instance.isSolved = false;
    }

    // Update is called once per frame
    void Update()
    {
        int i=0;
        foreach (Hole hole in FindObjectsOfType<Hole>())
        {
            if(!hole.isActive) {i=i+1;}
        }
        if(i==0) {
            if(state=="DoorIdle") {
                anim.Play("DoorDown");
                SoundManager.Instance.Play("Door");
            }
            state="DoorIdleDown";
            
        } else {
            if(state=="DoorIdleDown") {
                anim.Play("DoorUp");
                SoundManager.Instance.Play("Door");
                scientistTalk=true;
            }
            state="DoorIdle";

        }


        if(state=="DoorIdleDown" && scientistTalk==true) {
            scientistTalk = false;
            StartCoroutine(GameMaster.Instance.Scientist());
            GameMaster.Instance.isSolved = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.name == "player" && state=="DoorIdleDown") {
            StartCoroutine(GameMaster.Instance.LevelUp());
        }
    }
}
