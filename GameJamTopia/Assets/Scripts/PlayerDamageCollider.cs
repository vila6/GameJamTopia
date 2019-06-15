using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageCollider : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        switch(collider.tag)
        {
            case "ProjectileInk":
                break;
            case "ProjectileBuble":
                break;
            default:
                break;
        }
    }
}
