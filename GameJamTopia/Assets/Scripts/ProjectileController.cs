using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float velocity = 10f;
    void Start()
    {
        this.transform.rotation = Quaternion.Euler(0, 0, 90);
        this.GetComponent<Rigidbody>().velocity = -transform.up * velocity;
        
        Destroy(this.gameObject, 10f);
    }

    void OnTriggerEnter()
    {
        Destroy(this.gameObject);
    }
}
