using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageCollider : MonoBehaviour
{
    [Range(0,1000)]
    public int inkObtainedOnInkProjectile = 10;
    [Range(0,1000)]
    public int inkLoseOnBubleProjectile = 10;
    [Range(0,1000)]
    public int inkObtainedOnInkPickUp = 30;
    [Range(0,1000)]
    public int inkLoseOnSepia = 20;


    private PlayerController player;

    void Start()
    {
        player = PlayerController.instance;
    }
    void OnTriggerEnter(Collider collider)
    {
        switch(collider.tag)
        {
            case "ProjectileInk":
               player.AddInk(inkObtainedOnInkProjectile);
                break;
            case "ProjectileBuble":
                player.AddInk(-inkLoseOnBubleProjectile);
                break;
            case "InkPickUp":
                player.AddInk(inkObtainedOnInkPickUp);
                break;
            case "SepiaDamageCollider":
                player.AddInk(-inkLoseOnSepia);
                player.HitMovement(collider.transform.position);
                break;
            default:
                break;
        }
    }
}
