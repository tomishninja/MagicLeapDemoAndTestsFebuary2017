using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class RotateOnFist : MonoBehaviour {

    public bool trackLeftHand = true;

    public bool trackRightHand = false;

    //public GameObject RightHand = null;
    //public GameObject LeftHand = null;

    public GameObject gameObjectToRotate = null;

    private float previousRightHandYposition = float.MinValue;
    private float previousLeftHandYposition = float.MinValue;

    // Update is called once per frame
    void Update () {
        // Don't do anything if you can't use your hands
        if (MLHands.IsStarted)
        {
            // the confidence of each hand will be set to zero by default if it isn't being tracked
            float confidenceLeft = trackLeftHand ? GetKeyPoseConfidence(MLHands.Left) : 0.0f;
            float confidenceRight = trackRightHand ? GetKeyPoseConfidence(MLHands.Right) : 0.0f;

            if (confidenceLeft > 0.0f)
            {
                if (previousLeftHandYposition != float.MinValue)
                {
                    // calculate the new y value
                    float newYValue = previousLeftHandYposition - MLHands.Left.Center.y;

                    // calculate the new x value

                    //TODO some maths to rotate this properly
                    gameObjectToRotate.transform.rotation = new Quaternion(
                        gameObjectToRotate.transform.rotation.x,
                        gameObjectToRotate.transform.rotation.y,
                        gameObjectToRotate.transform.rotation.z + newYValue,
                        gameObjectToRotate.transform.rotation.w
                        );
                }

                //Update and set the new position
                previousLeftHandYposition = MLHands.Left.Center.y;
            }

            if (confidenceRight > 0.0f)
            {

            }
        }
    }

    /// <summary>
    /// Get the confidence value for the hand being tracked.
    /// </summary>
    /// <remarks>
    /// Copied from Key Pose Visualiser class
    /// </remarks>
    /// <param name="hand">Hand to check the confidence value on. </param>
    /// <returns></returns>
    private float GetKeyPoseConfidence(MLHand hand)
    {
        if (hand != null)
        {
            if (hand.KeyPose == MLHandKeyPose.Fist)
            {
                return hand.KeyPoseConfidence;
            }
        }
        return 0.0f;
    }
}
