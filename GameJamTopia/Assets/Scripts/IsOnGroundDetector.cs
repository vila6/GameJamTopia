using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsOnGroundDetector : MonoBehaviour
{
    void OnTriggerEnter()
    {
        PlayerController.instance.SetIsOnGround(true);
    }

    void OnTriggerExit()
    {
        PlayerController.instance.SetIsOnGround(false);
    }
}
