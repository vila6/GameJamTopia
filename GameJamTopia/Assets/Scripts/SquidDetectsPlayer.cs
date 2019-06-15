using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquidDetectsPlayer : MonoBehaviour
{
    public Squid squid;

    void OnTriggerStay(Collider collider)
    {
        if(collider.tag == "Player")
        {
            LayerMask mask = 1 << LayerMask.GetMask("Player");
            mask = mask << LayerMask.GetMask("IgnoreRaycast");
            RaycastHit hit;
            if(!Physics.SphereCast(squid.transform.position, 0.5f, (collider.transform.position - squid.transform.position).normalized, out hit, (collider.transform.position - squid.transform.position).magnitude -0.5f, mask))
            {
                squid.PlayerDetected(collider.transform.position);
            }
        }
    }


}
