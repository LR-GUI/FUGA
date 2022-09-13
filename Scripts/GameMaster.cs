using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    public static GameMaster Instance;

    void Awake() {

        if(Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        Instance = this;
    }

    public bool isPlayingGame = false;

    public GameObject hotSquare;
    public GameObject coldSquare;
    public GameObject stripSquare;

    public menu menu;

    
    private Image fadeSquare;
    public Image bigSquare;
    public Image littleSquare;

    public GameObject endingSquare;

    public GameObject[] Stages;
    public int activeStage=0;

    public GameObject lab;
    public bool labOn=false;

    private DialogueManager dm;

    public bool isSolved = false;

    public GameObject cm;

    public bool isCamFollowing = false;
    public bool stoned = false;

    public List<bool> levelClear = new List<bool>();
    public bool unlockBadEnding = false;
    public bool unlockGoodEnding = false;
    public bool unlock = false;

    public bool loadState = false;


    public bool badEndingWalk = false;
    public bool returnFromEnding = false;

    public bool goodEndingTalk = false;

    float waiter = 0;

    public bool soundOn = true;



    // Start is called before the first frame update
    void Start()
    {
        //LoadGame();
        dm = FindObjectOfType<DialogueManager>();
        menu.gameObject.SetActive(true);
        dm.dialogueOn = true;

        for (int i = 0; i < 16; i++)
        {
            levelClear.Add(false);
        }
        
    }

    // Update is called once per frame
    void Update() {
        if(Input.GetKeyDown(KeyCode.Escape) && isPlayingGame && !dm.dialogueOn) {
            StartCoroutine(BackToSelection());
        }

        if(!loadState) {
            LoadGame();
            loadState = true;
        }

        if(goodEndingTalk) {
            unlockGoodEnding = true;
            Player player = FindObjectOfType<Player>();
            player.ChangeAnimation("WalkLeft");

            Vector3 destination = new Vector3(-14,61f,0);
            if(player.transform.position != destination) {
                player.transform.position = Vector3.MoveTowards(player.transform.position,destination,player.speed*Time.deltaTime/10);
            } else {

                player.ChangeAnimation("IdleLeft");
                GameObject.Find("robot_ending").GetComponent<DialogueTrigger>().TriggerDialogue();
                goodEndingTalk = false;
                levelClear[15] = true;
                dm.dialogueOn = true;
                returnFromEnding = true;
            }

        }

        if(badEndingWalk) {
            unlockBadEnding = true;
            Player player = FindObjectOfType<Player>();
            Vector3 destination = new Vector3(0,84f,0);
            if(player.transform.position != destination) {
                player.transform.position = Vector3.MoveTowards(player.transform.position,destination,player.speed*Time.deltaTime/10);
            } else {
                player.ChangeAnimation("IdleUp");
                GameObject.Find("scientist_ending").GetComponent<DialogueTrigger>().TriggerDialogue();
                dm.dialogueWait = true;
                waiter = 2f;
                
                badEndingWalk = false;
            }
            
        }
        
        if(dm.triggerNext) {
            if(waiter>0) {
                waiter -= Time.deltaTime;
            } else {
                GameObject.Find("Endings").GetComponent<DialogueTrigger>().TriggerDialogue();
                dm.triggerNext = false;
                waiter = 0;
                levelClear[15] = true;
                dm.dialogueOn = true;
                returnFromEnding = true;

            }
            
            
        }

        if(returnFromEnding && !dm.dialogueOn) {
            StartCoroutine(EndingFinish());
            returnFromEnding = false;
        }


        if(!unlock && levelClear[15]) {
            unlock = true;
            //post-ending
        }


    }


    public IEnumerator Fading(bool fadeOut = true, float speed = 5, bool isLab = false, bool ending=false) {
        
        if(!isLab) {
            Color objColor = new Color(0f,0f,0f,0);
            if(!fadeOut) {
                objColor = new Color(0f,0f,0f,1f);
            }
            
            if(ending) {
                objColor = new Color(1f,1f,1f,0);
                if(!fadeOut) {
                    objColor = new Color(1f,1f,1f,1f);
                }
            }
            float amount;

            if(fadeOut) {
                bigSquare.gameObject.SetActive(true);
                while(bigSquare.color.a<1) {
                    amount = objColor.a + (speed*Time.deltaTime);
                    objColor = new Color(objColor.r,objColor.g,objColor.b,amount);
                    bigSquare.color = objColor;
                    yield return null;
                }
                
            } else {

                while(bigSquare.color.a>0) {
                        amount = objColor.a - (speed*Time.deltaTime);
                        objColor = new Color(objColor.r,objColor.g,objColor.b,amount);
                        bigSquare.color = objColor;
                        
                        yield return null;
                    }
                bigSquare.gameObject.SetActive(false);
            }

        } else {
            Color objColor = littleSquare.color;
            float amount;

            if(fadeOut) {
                littleSquare.gameObject.SetActive(true);
                while(littleSquare.color.a<1) {
                    amount = objColor.a + (speed*Time.deltaTime);
                    objColor = new Color(objColor.r,objColor.g,objColor.b,amount);
                    littleSquare.color = objColor;
                    yield return null;
                }
                
            } else {
                littleSquare.gameObject.SetActive(true);
                lab.SetActive(true);
                while(littleSquare.color.a>0) {
                        amount = objColor.a - (speed*Time.deltaTime);
                        objColor = new Color(objColor.r,objColor.g,objColor.b,amount);
                        littleSquare.color = objColor;
                        yield return null;
                    }
                littleSquare.gameObject.SetActive(false); 
            }
        }

        
        

        
    }

    public IEnumerator LevelUp(bool actualLevelUp=true) {


        StartCoroutine(Fading());
        SoundManager.Instance.Stop();
        
        yield return new WaitForSeconds(2f);
        foreach (GameObject stage in Stages)
        {
            stage.SetActive(false);
        }
        if(actualLevelUp) {
            if(activeStage != 15) {
                menu.selectedPanel += 1;
            }
            levelClear[activeStage] = true;
            activeStage = activeStage+1;

            SaveSystem.SaveData(this);
            
        }
        
        
        Stages[activeStage].SetActive(true);
        
        StartCoroutine(Stages[activeStage].GetComponent<StageMaster>().RestartLevel());
        lab.SetActive(false);

        if(activeStage==8) {
            coldSquare.SetActive(true);
            hotSquare.SetActive(false);
        } else if(activeStage==6) {
            coldSquare.SetActive(false);
            hotSquare.SetActive(true);
        } else {
            coldSquare.SetActive(false);
            hotSquare.SetActive(false);
        }

        if(activeStage==11) {
            stripSquare.SetActive(true);
        } else {
            stripSquare.SetActive(false);
        }
        if(activeStage==12) {
            Camera.main.GetComponent<Animator>().Play("shake");
        } else {
            Camera.main.GetComponent<Animator>().Play("camIdle");
        }

        menu.selectionPanel.SetActive(false);
        menu.gameObject.SetActive(false);
        dm.dialogueOn=true;
        isPlayingGame = true;

        FindObjectOfType<StageMaster>().PlayMusic();
        StartCoroutine(FindObjectOfType<StageMaster>().PlayDialogue());
        


    }


    public IEnumerator Scientist() {
        dm.dialogueOn = true;
        yield return new WaitForSeconds(1f);
        StartCoroutine(Fading(false,5,true));
        
        yield return new WaitForSeconds(1f);
        lab.GetComponent<DialogueTrigger>().TriggerDialogue(activeStage);
        labOn = true;
    }

    public IEnumerator CloseLab() {
        labOn = false;
        StartCoroutine(Fading(true,5,true));       
        yield return new WaitForSeconds(1f);
        lab.SetActive(false);
    }

    public IEnumerator BackToSelection() {

        isPlayingGame = false;
        StartCoroutine(Fading());
        SoundManager.Instance.Stop();
        
        yield return new WaitForSeconds(0.8f);



        
        menu.gameObject.SetActive(true);
        menu.isInSelectionPanel = true;
        menu.selectionPanel.SetActive(true);
        menu.controlling = true;
        StartCoroutine(GameMaster.Instance.Fading(false));

        
    }

    public IEnumerator RevealEnding(float speed=5) {
        Color objColor = endingSquare.GetComponent<SpriteRenderer>().color;
        float amount;
        while(objColor.a>0) {
            amount = objColor.a - (speed*Time.deltaTime);
            objColor = new Color(objColor.r,objColor.g,objColor.b,amount);
            endingSquare.GetComponent<SpriteRenderer>().color = objColor;
            yield return null;
        }
    }

    public void Ending(string type) {
        isPlayingGame = false;
        SoundManager.Instance.Stop();
        

        if(type=="Good") {
            goodEndingTalk = true;
        }


        if(type=="Bad") {

            badEndingWalk = true;
            
            Player player = FindObjectOfType<Player>();
            player.ChangeAnimation("WalkUp");

        }
    }



    public IEnumerator EndingFinish() {

        yield return new WaitForSeconds(0.8f);

        Player player = FindObjectOfType<Player>();

        StartCoroutine(GameMaster.Instance.Fading(true,0.2f,false,true));
        yield return new WaitForSeconds(8f);
        menu.gameObject.SetActive(true);
        menu.controlling = true;
        player.transform.position = new Vector3(0,-7f,0);
        StartCoroutine(GameMaster.Instance.Fading(false,1f,false,true));

        SaveSystem.SaveData(this);

    }

    public void LoadGame() {
        SaveData data = SaveSystem.Load();

        if(data!=null) {
            unlockBadEnding = data.unlockBadEnding;
            unlockGoodEnding = data.unlockGoodEnding;
            unlock = data.unlock;

            levelClear[0] = data.levelClear[0];
            levelClear[1] = data.levelClear[1];
            levelClear[2] = data.levelClear[2];
            levelClear[3] = data.levelClear[3];
            levelClear[4] = data.levelClear[4];
            levelClear[5] = data.levelClear[5];
            levelClear[6] = data.levelClear[6];
            levelClear[7] = data.levelClear[7];
            levelClear[8] = data.levelClear[8];
            levelClear[9] = data.levelClear[9];
            levelClear[10] = data.levelClear[10];
            levelClear[11] = data.levelClear[11];
            levelClear[12] = data.levelClear[12];
            levelClear[13] = data.levelClear[13];
            levelClear[14] = data.levelClear[14];
            levelClear[15] = data.levelClear[15];
        }

        
    }


    
    
}
