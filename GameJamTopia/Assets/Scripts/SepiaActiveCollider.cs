using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SepiaActiveCollider : MonoBehaviour
{
    public GameObject sepia;
    void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Player")
        {
            sepia.GetComponent<Sepia>().SetIsActive(true);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if(collider.tag == "Player")
        {
            sepia.GetComponent<Sepia>().SetIsActive(false);
        }
    }
}
