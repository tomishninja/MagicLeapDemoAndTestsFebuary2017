using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayInRelationTo : MonoBehaviour {

    public GameObject ObjectToFollow = null;
    public bool FollowOnX = false;
    public bool FollowOnY = false;
    public bool FollowOnZ = false;
    public Vector3 offset;
	
	// Update is called once per frame
	void Update () {
        if (ObjectToFollow != null)
        {
            // set the new object to the position nessarcary
            transform.position = calculateNewPos();
        }
	}

    private Vector3 calculateNewPos()
    {
        // create all the final values set them to the current positions if they arn't supposed to follow
        float x = transform.position.x;
        float y = transform.position.y;
        float z = transform.position.z;

        // if the object should follow determine how this should be done
        if (FollowOnX)
            x = ObjectToFollow.transform.position.x + offset.x;

        if (FollowOnY)
            y = ObjectToFollow.transform.position.y + offset.y;

        if (FollowOnZ)
            z = ObjectToFollow.transform.position.z + offset.z;

        // return the final vector
        return new Vector3(x, y, z);

    }
}
