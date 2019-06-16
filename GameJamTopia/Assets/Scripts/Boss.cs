using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Rigidbody bossRb;
    public Animator bossAnimator;

    private bool isActive = true;
    private bool checkDistance = false;

    void OnTriggerEnter(Collider collider)
    {
        if(isActive && collider.tag == "Player")
        {
            isActive = false;
            bossAnimator.enabled = true;
            StartCoroutine(StartBoss());
        }
    }

    private IEnumerator StartBoss()
    {
        yield return new WaitForSeconds(1.5f);
        bossRb.velocity = new Vector3(-3f, 0, 0);
        checkDistance = true;
    }

    void FixedUpdate()
    {
        if(checkDistance)
        {
            float distance = Vector3.Distance(PlayerController.instance.transform.position, bossRb.transform.position) - 9f;
            if(distance > 500f)
            {
                bossRb.velocity = new Vector3(-100f, 0, 0);
            }
            else if(distance > 200f)
            {
                bossRb.velocity = new Vector3(-20f, 0, 0);
            }
            else if(distance > 100f)
            {
                bossRb.velocity = new Vector3(-15f, 0, 0);
            }
            else if(distance > 50f)
            {
                bossRb.velocity = new Vector3(-6f, 0, 0);
            }
            else if(distance > 25f)
            {
                bossRb.velocity = new Vector3(-4f, 0, 0);
            }
            else{
                bossRb.velocity = new Vector3(-3f, 0, 0);
            }
            
        }
    }
}
