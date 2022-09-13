using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMaster : MonoBehaviour
{
    public int playerstartingx;
    public int playerstartingy;
    private GameObject player;

    private DialogueManager dm;

    public int levelID;

    private bool sendDrone = false;





    


    // Start is called before the first frame update
    void Start()
    {
        
        

        

        dm = FindObjectOfType<DialogueManager>();
        player = FindObjectOfType<Player>().gameObject;
        playerstartingx = 0;
        playerstartingy = -7;

        if(levelID==0) {
            playerstartingx = 8;
            playerstartingy = -5;
        }

        // ERASE LATER
        //if(levelID==15) {
        //    playerstartingx=0;
        //    playerstartingy=76;
        //}

        
        
    }


    // Update is called once per frame
    void Update()
    {
        if(levelID==15 && !GameMaster.Instance.isCamFollowing) {
            GameMaster.Instance.isCamFollowing = true;
            GameMaster.Instance.cm.transform.parent = player.transform;
            GameMaster.Instance.cm.transform.localPosition = new Vector3(0, 1.6f, -5f);
        } 
        if(levelID!=15 && GameMaster.Instance.isCamFollowing) {
            GameMaster.Instance.isCamFollowing = false;
            GameMaster.Instance.cm.transform.parent = null;
            GameMaster.Instance.cm.transform.position = new Vector3(0, -0.4f, -5f);
        }

        if(!dm.dialogueOn && sendDrone) {
            StartCoroutine(GameMaster.Instance.Scientist());
            sendDrone=false;
        }

        if(Input.GetKeyDown(KeyCode.R) && !dm.dialogueOn && GameMaster.Instance.isPlayingGame) {
            StartCoroutine(RestartLevel());
        }

        if(levelID==15) {
            player.GetComponent<Player>().CheckEnding();
        }

    }

    public IEnumerator RestartLevel() {

        GameMaster.Instance.isSolved = false;
        if(levelID!=15) {
            GameMaster.Instance.labOn = false;
        
            StartCoroutine(GameMaster.Instance.CloseLab());
        } else {
            GameMaster.Instance.endingSquare.GetComponent<SpriteRenderer>().color = new Color(0,0,0,1f);
            FindObjectOfType<Player>().isEnding = false;
        }
        
        
        
        StartCoroutine(GameMaster.Instance.Fading());
        yield return new WaitForSeconds(0.8f);
        foreach (Stone stone in FindObjectsOfType<Stone>())
        {
            stone.isMoving = false;
            stone.transform.position = new Vector3(stone.startingx,stone.startingy,0);
            stone.anim.Play("Stone");
            stone.isReady = false;
        }
        if(levelID==15) {
            Taker taker = FindObjectOfType<Taker>();
            taker.transform.position = new Vector3(taker.startingx,taker.startingy,0);
        }
        player.transform.position = new Vector3(playerstartingx,playerstartingy,0);
        
        if(levelID==0) {
            player.transform.GetChild(0).gameObject.GetComponent<Animator>().Play("IdleLeft");
        } else {
            player.transform.GetChild(0).gameObject.GetComponent<Animator>().Play("IdleUp");
        }

        StartCoroutine(GameMaster.Instance.Fading(false));
        GameMaster.Instance.stoned = false;
    }

    public IEnumerator PlayDialogue() {
        yield return new WaitForSeconds(1f);
        
        if(levelID==15) {
            dm.dialogueOn=false;
            sendDrone=true;
        } else {
            gameObject.GetComponent<DialogueTrigger>().TriggerDialogue();
        }
    }

    public void PlayMusic() {
        
        if(GameMaster.Instance.soundOn) {

            if(levelID==0) {
                SoundManager.Instance.Play("1");
            } else if(levelID==1) {
                SoundManager.Instance.Play("2");
            } else if(levelID==2) {
                SoundManager.Instance.Play("3");
            } else if(levelID==3) {
                SoundManager.Instance.Play("4");
            } else if(levelID==4) {
                SoundManager.Instance.Play("5");
            } else if(levelID==5) {
                SoundManager.Instance.Play("6");
            } else if(levelID==6) {
                SoundManager.Instance.Play("7");
            } else if(levelID==7) {
                SoundManager.Instance.Play("8");
            } else if(levelID==8) {
                SoundManager.Instance.Play("9");
            } else if(levelID==9) {
                SoundManager.Instance.Play("10");
            } else if(levelID==10) {
                SoundManager.Instance.Play("11");
            } else if(levelID==11) {
                SoundManager.Instance.Play("12");
            } else if(levelID==12) {
                SoundManager.Instance.Play("13");
            } else if(levelID==13) {
                SoundManager.Instance.Play("14");
            } else if(levelID==14) {
                SoundManager.Instance.Play("15");
            } else if(levelID==15) {
                SoundManager.Instance.Play("16");
            }

        }
        
        
    }


}
