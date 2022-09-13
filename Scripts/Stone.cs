using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{

    public float speed;

    Vector3 destination;

    public bool isMoving = false;
    public bool isReady = false;

    public Animator anim;

    public int startingx;
    public int startingy;

    public bool hitStone = false;

    public bool endingStone = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        startingx = Mathf.RoundToInt(transform.position.x);
        startingy = Mathf.RoundToInt(transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {  
        if(isMoving) {
            transform.position = Vector3.MoveTowards(transform.position,destination,speed*Time.deltaTime);
            
            if(transform.position==destination) {
                isMoving=false;
                
                if(hitStone) {
                    SoundManager.Instance.Play("Balls");
                } else {
                    SoundManager.Instance.Play("Low Kick");
                }

                transform.position = new Vector3(Mathf.RoundToInt(transform.position.x),Mathf.RoundToInt(transform.position.y),0);
            }
        }

        if(!isMoving && !isReady) {
            foreach (Hole hole in FindObjectsOfType<Hole>() )
            {
                if(hole.transform.position.x == transform.position.x && hole.transform.position.y == transform.position.y) {
                    anim.Play("ReadyStone");
                    SoundManager.Instance.Play("On");
                    isReady=true;
                }
            }
        }

        if(isReady && isMoving) {
            anim.Play("Stone");
            isReady = false;
        }
        
        
    }



    public void Move(string direction) {
        int dirX=0;
        int dirY=0;

        if(direction==""){
            return;
        } else if (direction=="Left") {
            dirX = -1;
            dirY = 0;
        } else if (direction=="Right") {
            dirX = 1;
            dirY = 0;
        } else if (direction=="Up") {
            dirX = 0;
            dirY = 1;
        } else if (direction=="Down") {
            dirX = 0;
            dirY = -1;
        }
        
        int i = 1;
        SoundManager.Instance.Play("High Kick");
        while (true)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position+new Vector3(i*dirX,i*dirY,0), new Vector2(dirX,dirY),0.1f,1<<3);

        
            if (hit.collider != null)
            {
                if(hit.collider.gameObject.GetComponent<Stone>() != null) {
                    hitStone = true;
                } else {
                    hitStone = false;
                }

                destination = new Vector3(transform.position.x+(i-1)*dirX,transform.position.y+(i-1)*dirY,0);
                isMoving = true;

                break;
            }

            

            i = i+1;
            if(i==40) {break;}
        }
        
    }


}
