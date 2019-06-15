﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkPickUpController : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Player")
        {
            Destroy(this.gameObject);
        }
    }
}
