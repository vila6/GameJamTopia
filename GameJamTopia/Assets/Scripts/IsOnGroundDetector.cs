using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsOnGroundDetector : MonoBehaviour
{
    void Update()
    {
        Debug.DrawLine(this.transform.position, this.transform.position + Vector3.down * 0.4f, Color.red);
        if(Physics.Raycast(this.transform.position, Vector3.down, 0.4f))
        {
            PlayerController.instance.SetIsOnGround(true);
        }
        else
        {
            PlayerController.instance.SetIsOnGround(false);
        }
    }
}
