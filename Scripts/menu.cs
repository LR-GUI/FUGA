using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class menu : MonoBehaviour
{

    public int selectedButton = 1;
    public GameObject[] buttonsList;

    public GameObject[] levelButtonsList;

    public GameObject goodEndingPanel;
    public GameObject badEndingPanel;
    public bool goodEndingActive = false;
    public bool badEndingActive = false;


    public GameObject selectionPanel;
    public bool isInSelectionPanel=false;
    public int selectedPanel=0;
    public bool isAbleToChoose = false;

    public GameObject optionsMenu;
    public bool isOptions = false;
    public GameObject[] soundOptionButtons;
    public int selectedSound = 0;

    public float pressed = 0.7f;

    public bool controlling = false;
    
    // Start is called before the first frame update
    void Start()
    {
        controlling = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(controlling) {

            if(selectedButton==1  && !isInSelectionPanel && !isOptions) {
                buttonsList[0].GetComponent<Image>().color = new Color(0,1,1,1);
                buttonsList[1].GetComponent<Image>().color = new Color(0,pressed,pressed,1);
                buttonsList[2].GetComponent<Image>().color = new Color(0,pressed,pressed,1);
                if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) {
                    selectedButton = 2;
                    SoundManager.Instance.Play("Letter");
                }
                if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
                    selectedButton = 3;
                    SoundManager.Instance.Play("Letter");
                }

            } else if(selectedButton==2  && !isInSelectionPanel && !isOptions) {
                buttonsList[1].GetComponent<Image>().color = new Color(0,1,1,1);
                buttonsList[0].GetComponent<Image>().color = new Color(0,pressed,pressed,1);
                buttonsList[2].GetComponent<Image>().color = new Color(0,pressed,pressed,1);
                if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) {
                    selectedButton =3;
                    SoundManager.Instance.Play("Letter");
                }

                if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)){
                    selectedButton = 1;
                    SoundManager.Instance.Play("Letter");
                }

            } else if(selectedButton==3  && !isInSelectionPanel && !isOptions) {
                buttonsList[2].GetComponent<Image>().color = new Color(0,1,1,1);
                buttonsList[1].GetComponent<Image>().color = new Color(0,pressed,pressed,1);
                buttonsList[0].GetComponent<Image>().color = new Color(0,pressed,pressed,1);
                if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)){
                    selectedButton =1;
                    SoundManager.Instance.Play("Letter");
                }
                if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)){
                    selectedButton = 2;
                    SoundManager.Instance.Play("Letter");
                }

            }

            if(Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return)) {
                if(!isInSelectionPanel && !isOptions) {SoundManager.Instance.Play("On");}
                if(selectedButton==1 && !isInSelectionPanel && !isOptions) {
                    selectionPanel.SetActive(true);
                    isInSelectionPanel=true;
                } else if(selectedButton==2 && !isInSelectionPanel && !isOptions) {
                    OpenOptions();
                } else if(selectedButton==3 && !isInSelectionPanel && !isOptions) {
                    Debug.Log("quit");
                    Application.Quit();
                }
                
                
            }

            if(isInSelectionPanel) {SelectionPanel();}

            if(isOptions) {OptionsPanel();}

            if(!isInSelectionPanel && !isOptions) {
                isAbleToChoose = false;
            }

        }
        




        if(GameMaster.Instance.unlockGoodEnding && !goodEndingActive) {
            goodEndingPanel.SetActive(true);
            goodEndingActive = true;
        }
        if(GameMaster.Instance.unlockBadEnding && !badEndingActive) {
            badEndingPanel.SetActive(true);
            badEndingActive = true;
        }
        

        
    }

    public void OpenOptions() {
        optionsMenu.SetActive(true);
        isOptions = true;
    }

    public void OptionsPanel() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            isOptions = false;
            isAbleToChoose=false;
            optionsMenu.SetActive(false);
        }

        foreach (GameObject panel in soundOptionButtons)
        {
            panel.GetComponent<Image>().color = new Color(0,pressed,pressed,1);
        }
       soundOptionButtons[selectedSound].GetComponent<Image>().color = new Color(0,1,1,1);

       if((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && isOptions && isAbleToChoose) {
           if(selectedSound==0) {
               selectedSound=1;
               SoundManager.Instance.Play("Low Kick");
           } else if(selectedSound==1) {
               selectedSound=0;
               SoundManager.Instance.Play("Low Kick");
           }
        }

        if((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Z)) && isOptions && isAbleToChoose) {
            if(selectedSound==0) {
                GameMaster.Instance.soundOn = true;
            } else if(selectedSound==1) {
                GameMaster.Instance.soundOn = false;
            }

            SoundManager.Instance.Play("On");
            isOptions = false;
            
            optionsMenu.SetActive(false);
            isAbleToChoose=false;
        }

        isAbleToChoose = true;
    }

    public void PlayGame(int i) {

        controlling = false;

        


        //GameMaster.Instance.Stages[i].SetActive(true);
        GameMaster.Instance.activeStage=i;
        StartCoroutine(GameMaster.Instance.LevelUp(false));
        
        isInSelectionPanel=false;
        
    }

    public void SelectionPanel() {

        if(Input.GetKey(KeyCode.F) && Input.GetKey(KeyCode.U) && Input.GetKey(KeyCode.G) && Input.GetKey(KeyCode.A) && !GameMaster.Instance.unlock) {
            SoundManager.Instance.Play("On");
            for (int i = 0; i < 16; i++)
            {
                GameMaster.Instance.levelClear[i] = true;
            }
            GameMaster.Instance.unlock = true;
            SaveSystem.SaveData(GameMaster.Instance);
        }


        foreach (GameObject panel in levelButtonsList)
        {
            panel.GetComponent<Image>().color = new Color(0,pressed,pressed,1);

            if(panel.name=="1") {
                panel.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color = new Color(0.6f,0.2f,0.6f,1f);
                panel.transform.GetChild(1).gameObject.GetComponent<Image>().color = new Color(0,0,0,0);
            } else {
                int index = int.Parse(panel.name)-2;
                if(GameMaster.Instance.levelClear[index]) {
                    panel.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color = new Color(0.6f,0.2f,0.6f,1f);
                    panel.transform.GetChild(1).gameObject.GetComponent<Image>().color = new Color(0,0,0,0);
                } else {
                    panel.transform.GetChild(1).gameObject.GetComponent<Image>().color = new Color(1f,1f,1f,1f);
                    panel.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color = new Color(0,0,0,0);
                }

            }
        }
        levelButtonsList[selectedPanel].GetComponent<Image>().color = new Color(0,1,1,1);

        int yc = selectedPanel/4;
        int xc = selectedPanel%4;

        if((Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return)) && isAbleToChoose) {
            if(selectedPanel==0) {
                PlayGame(selectedPanel);
                isAbleToChoose=false;
                SoundManager.Instance.Play("On");
            } else if(GameMaster.Instance.levelClear[selectedPanel-1]) {
                PlayGame(selectedPanel);
                isAbleToChoose=false;
                SoundManager.Instance.Play("On");
            } else if(!GameMaster.Instance.levelClear[selectedPanel-1]) {
                SoundManager.Instance.Play("Low Kick");
            }
            
            
            
        }       

        else if((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) && isAbleToChoose) {
            yc=(yc+1)%4;
            SoundManager.Instance.Play("Letter");
        }
        else if((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) && isAbleToChoose) {
            xc=(xc+3)%4;
            SoundManager.Instance.Play("Letter");
        }
        else if((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && isAbleToChoose) {
            yc=(yc+3)%4;
            SoundManager.Instance.Play("Letter");
        }
        else if((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) && isAbleToChoose) {
            xc=(xc+1)%4;
            SoundManager.Instance.Play("Letter");
        }

        selectedPanel = 4*yc + xc;



        if(Input.GetKeyDown(KeyCode.Escape) && isAbleToChoose) {
            isInSelectionPanel = false;
            GameMaster.Instance.activeStage=0;
            selectionPanel.SetActive(false);
        }

        isAbleToChoose=true;

    }
}
