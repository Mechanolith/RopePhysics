using UnityEngine;
using System.Collections;

public class FixedDistanceConstraint3D : MonoBehaviour
{

    public GameObject linkTarget;
    public float targetDistance;
    [Range(0, 1)]
    public float selfCompensation;
    public float compensationSpeed;
    public float dampValue;
    [Tooltip ("How much gravity should this compensate for? Should be consistent across ALL links in a chain.")]
    public float gravityFactor = 9.8f;
    public bool pinned;

    Rigidbody selfBody;
    Rigidbody linkBody;

    void Start()
    {
        //Find the rigid bodies for later.
        selfBody = gameObject.GetComponent<Rigidbody>();
        linkBody = linkTarget.GetComponent<Rigidbody>();
    }

    void Update()
    {
        //Find the current distance between the two links.
        Vector3 diffVec = linkTarget.transform.position - transform.position;
        float dist = diffVec.magnitude;

        //If it's not the distance we want, apply force.
        if (dist != targetDistance)
        {
            float diff = (dist - targetDistance) / dist;
            if (!pinned)    //If this link is allowed to move.
            {
                //Force = (Difference between positions * tightness * compensation ratio) + Gravity - Dampening
                Vector3 selfForce = ((selfCompensation * diffVec * compensationSpeed * diff) + (gravityFactor * Vector3.down) - (dampValue * ((Vector3)selfBody.velocity - (Vector3)linkBody.velocity))) * Time.deltaTime; ;
                selfBody.AddForce(selfForce, ForceMode.VelocityChange);
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
            Vector3 linkForce = ((linkComp * -diffVec * compensationSpeed * diff) + (gravityFactor * Vector3.down) - (dampValue * ((Vector3)selfBody.velocity - (Vector3)linkBody.velocity))) * Time.deltaTime;
            linkBody.AddForce(linkForce, ForceMode.VelocityChange);
        }
    }
}