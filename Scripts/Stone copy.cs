using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stonecopy : MonoBehaviour
{
    public bool isMoving = false;
    //private string movingTo = "";
    public float speed;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {  
        //if(isMoving) {
        //    if(movingTo==""){
        //        
        //    } else if (movingTo=="Left") {
        //        //rb.velocity = speed*Vector2.left;
        //        transform.Translate(-speed*Time.deltaTime,0,0);
        //    } else if (movingTo=="Right") {
        //        //rb.velocity = speed*Vector2.right;
        //        transform.Translate(speed*Time.deltaTime,0,0);
        //    } else if (movingTo=="Up") {
        //        //rb.velocity = speed*Vector2.up;
        //        transform.Translate(0,speed*Time.deltaTime,0);
        //    } else if (movingTo=="Down") {
        //        //rb.velocity = speed*Vector2.down;
        //        transform.Translate(0,-speed*Time.deltaTime,0);
        //    }  
        //}
        
    }

    void FixedUpdate() {
        if(!isMoving) {
            transform.position = new Vector3(Mathf.RoundToInt(transform.position.x),Mathf.RoundToInt(transform.position.y),0);
        }
    }

    public void Move(string direction) {
        //movingTo = direction;
        //isMoving = true;

        if(direction==""){
            return;
        } else if (direction=="Left") {
            rb.velocity = speed*Vector2.left;
            //transform.Translate(-speed*Time.deltaTime,0,0);
        } else if (direction=="Right") {
            rb.velocity = speed*Vector2.right;
            //transform.Translate(speed*Time.deltaTime,0,0);
        } else if (direction=="Up") {
            rb.velocity = speed*Vector2.up;
            //transform.Translate(0,speed*Time.deltaTime,0);
        } else if (direction=="Down") {
            rb.velocity = speed*Vector2.down;
            //transform.Translate(0,-speed*Time.deltaTime,0);
        }
        isMoving = true;
        
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Debug.Log("hit");
        
        rb.velocity = new Vector2(0,0);
        transform.position = new Vector3(Mathf.RoundToInt(transform.position.x),Mathf.RoundToInt(transform.position.y),0);
        isMoving = false;
    }

}
