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
    public int inkLoseOnSquid = 20;


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
                if (!player.isInvulnerable)
                {
                    player.AddInk(-inkLoseOnBubleProjectile);
                    player.HitBullet(collider.gameObject);
                }
                break;
            case "InkPickUp":
                player.AddInk(inkObtainedOnInkPickUp);
                break;
            case "SquidAttack":
                if (!player.isInvulnerable)
                {
                    player.AddInk(-inkLoseOnSquid);
                    player.HitSquid(collider.transform.position);
                }
                break;
            default:
                break;
        }
    }
}
