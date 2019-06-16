using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public Vector3 direction;
    public float velocity = 10f;
    public float delayUntilTriggerWorking;
    public bool isInk;
    public GameObject particleDeathInk;
    public GameObject particleDeathBubble;

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
        if((!collider.isTrigger || (collider.isTrigger && collider.tag == "Player") || (collider.isTrigger && collider.tag == "SquidAttack")) && collider.tag != "ProjectileInk" && collider.tag != "ProjectileBuble" && collider.tag != "Brush" )
        {
            if(isInk)
            {
                Destroy(GameObject.Instantiate(particleDeathInk, this.transform.position, Quaternion.identity), 0.5f);
            }
            else
            {
                Destroy(GameObject.Instantiate(particleDeathBubble, this.transform.position, Quaternion.identity), 0.5f);
            }
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Pasar el punto hacia el que dispara
    /// </summary>
    public void SetDirection(Vector3 target)
    {
        direction = (target - this.transform.position).normalized;
    }

    public void SetDirectionRight()
    {
        direction = Vector3.right;
    }

    public void SetDirectionLeft()
    {
        direction = Vector3.left;
    }

}
