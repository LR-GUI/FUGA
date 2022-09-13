using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hstone : MonoBehaviour
{
    public int amount = 4;
    public int startingx = 0;
    public float speed = 20;

    public float position;
    public bool isIncreasing = true;

    public bool isVertical = false;
    // Start is called before the first frame update
    void Start()
    {
        position = startingx;
    }

    // Update is called once per frame
    void Update()
    {
        if(position>=amount) {
            isIncreasing = false;
        }
        if(position<=0) {
            isIncreasing = true;
        }

        if(!isVertical && !GameMaster.Instance.stoned && GameMaster.Instance.isPlayingGame) {
            if(isIncreasing) {
                transform.Translate(Vector3.right * Time.deltaTime*speed);
                position += Time.deltaTime*speed;
            } else {
                transform.Translate(Vector3.left * Time.deltaTime*speed);
                position -= Time.deltaTime*speed;
            }
        }
        if(isVertical && !GameMaster.Instance.stoned && GameMaster.Instance.isPlayingGame) {
            if(isIncreasing) {
                transform.Translate(Vector3.up * Time.deltaTime*speed);
                position += Time.deltaTime*speed;
            } else {
                transform.Translate(Vector3.down * Time.deltaTime*speed);
                position -= Time.deltaTime*speed;
            }
        }

        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.name=="player" && !GameMaster.Instance.stoned)  {
            SoundManager.Instance.Play("Tin");
            //Debug.Log("taken");
            GameMaster.Instance.stoned = true;
            StartCoroutine(FindObjectOfType<StageMaster>().RestartLevel());
        }
    }


}
