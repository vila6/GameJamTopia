using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public Vector3 direction;
    public float velocity = 10f;
    public float delayUntilTriggerWorking;

    void Start()
    {
        this.transform.rotation = Quaternion.Euler(0, 0, 90);
        this.GetComponent<Rigidbody>().velocity = direction * velocity;
        
        Destroy(this.gameObject, 10f);

        StartCoroutine(DelayTriggerForSpawn());
    }

    private IEnumerator DelayTriggerForSpawn()
    {
        yield return new WaitForSeconds(delayUntilTriggerWorking);
        this.GetComponent<Collider>().enabled = true;
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.tag != "ProjectileInk" || collider.tag != "ProjectileBuble")
        {
            Destroy(this.gameObject);
        }
    }
}
