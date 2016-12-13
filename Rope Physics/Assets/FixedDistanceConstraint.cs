using UnityEngine;
using System.Collections;

public class FixedDistanceConstraint : MonoBehaviour {

    public GameObject linkTarget;
    public float targetDistance;
    [Range(0,1)]
    public float selfCompensation;
    public float compensationSpeed;
    public float dampValue;
    public bool pinned;

    Rigidbody2D selfBody;
    Rigidbody2D linkBody;

    void Start () {
        //Find the rigid bodies for later.
        selfBody = gameObject.GetComponent<Rigidbody2D>();
        linkBody = linkTarget.GetComponent<Rigidbody2D>();
	}
	
	void Update () {
        //Find the current distance between the two links.
        Vector3 diffVec = linkTarget.transform.position - transform.position;
        float dist = diffVec.magnitude;

        //If it's not the distance we want, apply force.
        if(dist != targetDistance)
        {
            float diff = (dist - targetDistance) / dist;
            if (!pinned)    //If this link is allowed to move.
            {
                //Force = (Difference between positions * tightness * compensation ratio) + Gravity - Dampening
                Vector3 selfForce = ((selfCompensation * diffVec * compensationSpeed * diff) + (linkBody.gravityScale * Vector3.down) - (dampValue * ((Vector3)selfBody.velocity - (Vector3)linkBody.velocity))) * Time.deltaTime; ;
                selfBody.AddForce(selfForce, ForceMode2D.Impulse);
            }

            float linkComp = (1 - selfCompensation);

            //Make sure the target has the script we need.
            //If it does and is pinned, don't let it move when we calculate force.
            if (linkTarget.GetComponent<FixedDistanceConstraint>() != null)
            {
                if (linkTarget.GetComponent<FixedDistanceConstraint>().pinned)
                {
                    linkComp = 0;
                }
            }

            //Force = (-Difference between positions * tightness * compensation ratio) + Gravity - Dampening
            Vector3 linkForce =  ((linkComp * -diffVec * compensationSpeed * diff) + (linkBody.gravityScale * Vector3.down) - (dampValue * ((Vector3)selfBody.velocity - (Vector3)linkBody.velocity))) * Time.deltaTime;
            linkBody.AddForce(linkForce, ForceMode2D.Impulse);
        }
    }
}