using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsOnGroundDetector : MonoBehaviour
{   
    public Transform maxRight, maxLeft;
    void Update()
    {
        Debug.DrawLine(maxLeft.position, maxLeft.position + Vector3.down * 0.4f, Color.red);
        Debug.DrawLine(maxRight.position, maxRight.position + Vector3.down * 0.4f, Color.red);
        if(Physics.Raycast(maxLeft.position, Vector3.down, 0.4f) || Physics.Raycast(maxRight.position, Vector3.down, 0.4f))
        {
            PlayerController.instance.SetIsOnGround(true);
        }
        else
        {
            PlayerController.instance.SetIsOnGround(false);
        }
    }
}
