using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    public bool isActive = false;

    public int startingx;
    public int startingy;
    // Start is called before the first frame update
    void Start()
    {
        startingx = Mathf.RoundToInt(transform.position.x);
        startingy = Mathf.RoundToInt(transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        
        int i=0;
        foreach (Stone stone in FindObjectsOfType<Stone>())
        {
            
            if(stone.transform.position.x == transform.position.x && stone.transform.position.y == transform.position.y) {
                i = i+1;
            }  
        }
        if(i>0 && !isActive) {
            isActive = true;
            
        }
        if(i==0 && isActive) {
            isActive = false;
            
        }

        
    }
}
