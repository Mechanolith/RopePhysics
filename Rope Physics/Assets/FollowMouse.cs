using UnityEngine;
using System.Collections;

public class FollowMouse : MonoBehaviour {
    public float moveSpeed = 1f;
    float currentZ = 0f;

	void Start () {
	    
	}
	
	void Update () {
        //Get the Mouse Position in-world
        Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            currentZ += moveSpeed * Time.deltaTime;
        }
        else if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            currentZ -= moveSpeed * Time.deltaTime;
        }

        newPos.z = currentZ;                  //Make sure it's on the correct Z value for 2D.
        transform.position = newPos;    //Follow that position.
	}
}
