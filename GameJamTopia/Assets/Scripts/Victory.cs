using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victory : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        GameManager.instance.Victory();       
    }
}
