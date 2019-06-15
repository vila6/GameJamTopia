using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageCollider : MonoBehaviour
{
    public int inkObtainedOnInkProjectile = 10;
    public int inkLoseOnBubleProjectile = 10;
    public int inkObtainedOnInkPickUp = 30;
    void OnTriggerEnter(Collider collider)
    {
        switch(collider.tag)
        {
            case "ProjectileInk":
                PlayerController.instance.AddInk(inkObtainedOnInkProjectile);
                break;
            case "ProjectileBuble":
                PlayerController.instance.AddInk(-inkLoseOnBubleProjectile);
                break;
            case "InkPickUp":
                PlayerController.instance.AddInk(inkObtainedOnInkPickUp);
                break;
            default:
                break;
        }
    }
}
