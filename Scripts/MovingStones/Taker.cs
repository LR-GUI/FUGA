using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taker : MonoBehaviour
{
    public float speed;
    public GameObject player;
    public DialogueManager dm;


    public int startingx;
    public int startingy;
    // Start is called before the first frame update
    void Start()
    {
        dm = FindObjectOfType<DialogueManager>();
        startingx = Mathf.RoundToInt(transform.position.x);
        startingy = Mathf.RoundToInt(transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        if(!dm.dialogueOn && !GameMaster.Instance.stoned && GameMaster.Instance.isPlayingGame) {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed*Time.deltaTime);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.name=="player" && !GameMaster.Instance.stoned) {
            //Debug.Log("taken");
            GameMaster.Instance.stoned = true;
            SoundManager.Instance.Play("Drone");
            StartCoroutine(FindObjectOfType<StageMaster>().RestartLevel());
        }
    }
}
