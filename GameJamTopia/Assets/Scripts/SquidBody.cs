using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquidBody : MonoBehaviour
{
    public Squid squid;
    public int brushDamage = 2;
    void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "BrushAttack")
        {
            squid.Hurt(brushDamage);
        }
    }
}
