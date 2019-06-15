using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinCollisionDetector : MonoBehaviour
{
    public bool rightDetector;

    void OnTriggerEnter(Collider collider)
    {
        if(collider.tag != "ProjectileInk" && collider.tag != "ProjectileBuble")
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
                
    }

    void OnTriggerExit(Collider collider)
    {
        if(collider.tag != "ProjectileInk" && collider.tag != "ProjectileBuble")
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
}
