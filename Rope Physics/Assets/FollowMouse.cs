using UnityEngine;
using System.Collections;

public class FollowMouse : MonoBehaviour {

	void Start () {
	    
	}
	
	void Update () {
        //Get the Mouse Position in-world
        Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        newPos.z = 0f;                  //Make sure it's on the correct Z value for 2D.
        transform.position = newPos;    //Follow that position.
	}
}
