using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinCollisionDetector : MonoBehaviour
{
    public bool rightDetector;

    void OnTriggerEnter()
    {
        if(rightDetector)
        {
            PlayerController.instance.collisionRight = true;
        }
        else
        {
            PlayerController.instance.collisionLeft = true;
        }        
    }

    void OnTriggerExit()
    {
        if(rightDetector)
        {
            PlayerController.instance.collisionRight = false;
        }
        else
        {
            PlayerController.instance.collisionLeft = false;
        }        
    }
}
