using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;

    public float xaxis;
    public float yaxis;
    Rigidbody2D rb;

    public bool isEnding=false;

    private Animator anim;
    private DialogueManager dm;
    private string currentAnim = "IdleDown";
    public string isFacing = "Down";
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = transform.GetChild(0).gameObject.GetComponent<Animator>();
        dm = FindObjectOfType<DialogueManager>();

        Debug.Log("Screen Width : " + Screen.width);
        Debug.Log("Screen Height : " + Screen.height);
    }

    // Update is called once per frame
    void Update()
    {

        if(GameMaster.Instance.isPlayingGame && (Input.GetKeyDown(KeyCode.Z)||Input.GetKeyDown(KeyCode.Return)) && !dm.dialogueOn && !GameMaster.Instance.isSolved) {
            Kick();
        }

        if(GameMaster.Instance.labOn && !dm.dialogueOn) {
            int playerx = Mathf.RoundToInt(transform.position.x);
            int playery = Mathf.RoundToInt(transform.position.y);

            if(playerx<=-6 && playery<10) {
                StartCoroutine(GameMaster.Instance.CloseLab());
            }
        }

        
        
    }

    void FixedUpdate() {

        if(!dm.dialogueOn && GameMaster.Instance.isPlayingGame && !GameMaster.Instance.stoned) {
            xaxis = Input.GetAxisRaw("Horizontal");
            yaxis = Input.GetAxisRaw("Vertical");
        } else {
            xaxis = 0;
            yaxis = 0;
        }
        
        if(GameMaster.Instance.isPlayingGame) {
            if(xaxis==0 && yaxis<0) {
                ChangeAnimation("WalkDown");
                isFacing = "Down";
            } else if (xaxis==0 && yaxis>0) {
                ChangeAnimation("WalkUp");
                isFacing = "Up";
            } else if (xaxis>0 && yaxis==0) {
                ChangeAnimation("WalkRight");
                isFacing = "Right";
            } else if (xaxis<0 && yaxis==0) {
                ChangeAnimation("WalkLeft");
                isFacing = "Left";
            } else if (xaxis<0 && yaxis>0) {
                ChangeAnimation("WalkLeft");
                isFacing = "Left";
            } else if (xaxis<0 && yaxis<0) {
                ChangeAnimation("WalkLeft");
                isFacing = "Left";
            } else if (xaxis>0 && yaxis>0) {
                ChangeAnimation("WalkRight");
                isFacing = "Right";
                } else if (xaxis>0 && yaxis<0) {
                ChangeAnimation("WalkRight");
                isFacing = "Right";
            } else if (xaxis==0 && yaxis==0) {
                if (isFacing == "Down") {
                    ChangeAnimation("IdleDown");
                } else if (isFacing == "Left") {
                    ChangeAnimation("IdleLeft");
                } else if (isFacing == "Right") {
                    ChangeAnimation("IdleRight");
                } else if (isFacing == "Up") {
                    ChangeAnimation("IdleUp");
                }
                
            }
        }
        

        
        rb.velocity = speed*Vector2.ClampMagnitude(new Vector2(xaxis,yaxis), 0.3f);
    }

    public void ChangeAnimation(string state) {
        if (currentAnim == state) return;
        anim.Play(state);
        currentAnim = state;
    }


    private void Kick() {
        foreach(Stone stone in FindObjectsOfType<Stone>()) {

            

            string kickTo = "";

            int stonex = Mathf.RoundToInt(stone.transform.position.x);
            int stoney = Mathf.RoundToInt(stone.transform.position.y);
            int playerx = Mathf.RoundToInt(transform.position.x);
            int playery = Mathf.RoundToInt(transform.position.y);

            bool kickRight = (stoney - playery == 0) && (stonex - playerx == 1) && (isFacing == "Right");
            bool kickLeft = (stoney - playery == 0) && (stonex - playerx == -1) && (isFacing == "Left");
            bool kickUp = (stoney - playery == 1) && (stonex - playerx == 0) && (isFacing == "Up");
            bool kickDown = (stoney - playery == -1) && (stonex - playerx == 0) && (isFacing == "Down");

            if(kickRight) {kickTo = "Right";}
            if(kickLeft) {kickTo = "Left";}
            if(kickUp) {kickTo = "Up";}
            if(kickDown) {kickTo = "Down";}

            stone.Move(kickTo);

            if(stone.endingStone && kickTo == "Left") {
                
                StartCoroutine(GameMaster.Instance.RevealEnding());
            }
        }
    }

    public void CheckEnding() {
        float playerx = transform.position.x;
        float playery = transform.position.y;

        if(playerx<=-9 && playery>=60 && !isEnding) {
            isEnding=true;
            GameMaster.Instance.Ending("Good");
        }
        if(playery>=79 && !isEnding) {
            isEnding=true;
            GameMaster.Instance.Ending("Bad");
        }

    }

}
