using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinCollisionDetector : MonoBehaviour
{
    public bool rightDetector;

    void OnTriggerEnter(Collider collider)
    {
        if(!collider.isTrigger && collider.tag != "ProjectileInk" && collider.tag != "ProjectileBuble" && collider.tag != "SepiaDamageCollider") 
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
        if(!collider.isTrigger && collider.tag != "ProjectileInk" && collider.tag != "ProjectileBuble" && collider.tag != "SepiaDamageCollider")
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
