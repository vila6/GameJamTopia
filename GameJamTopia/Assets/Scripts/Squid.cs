using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squid : MonoBehaviour
{
    public int life = 2;
    public int inkShots = 3;
    public float movementMaxCooldown = 3, movementCooldown = 3;

    public GameObject prefabPickupInk;

    // Patrulla
    private Vector3 targetPosition;
    public SquidPatrol myPatrol;
    public float speed = 5f;
    public float speedAttacking = 10f;
    private bool isPatrolling = true;
    private Vector3 attackPosition;
    private Vector3 beforeAttackPosition;
    private bool backToPatrol = false;
    public GameObject squidModel;

    private void Start()
    {
        targetPosition = myPatrol.RequestPatrolPoint();
        CustomLookAt(targetPosition);
    }

    void FixedUpdate()
    {
        if(isPatrolling)
        {
            if((targetPosition - this.transform.position).magnitude > speed * Time.deltaTime)
            {
                this.transform.position += (targetPosition - this.transform.position).normalized * Time.deltaTime * speed;
            }
            else
            {
                this.transform.position = targetPosition;
                targetPosition = myPatrol.RequestPatrolPoint();
                CustomLookAt(targetPosition);
            }
        }
        else if(!backToPatrol)
        {
            if((attackPosition - this.transform.position).magnitude > Time.deltaTime * speedAttacking + 2f)
            {
                this.transform.position += (attackPosition - this.transform.position).normalized * Time.deltaTime * speedAttacking;
            }
            else
            {
                backToPatrol = true;
                CustomLookAt(beforeAttackPosition);
            }
        }
        else
        {
            if((beforeAttackPosition - this.transform.position).magnitude > speed * Time.deltaTime)
            {
                this.transform.position += (beforeAttackPosition - this.transform.position).normalized * Time.deltaTime * speed;
            }
            else
            {
                this.transform.position = beforeAttackPosition;
                backToPatrol = false;
                isPatrolling = true;
                CustomLookAt(targetPosition);
            }
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "ProjectileInk")
        {
            Hurt(1);
        }
    }

    private void CustomLookAt(Vector3 target)
    {
        squidModel.transform.LookAt(target, Vector3.forward);
        squidModel.transform.RotateAround(squidModel.transform.position, squidModel.transform.right, 90f);
    }

    // The enemy recieves an ammount of damage
    public void Hurt(int damage)
    {
        life -= damage;
        if (life <= 0)
            Die();
    }

    // The enemy dies
    public void Die()
    {
        Instantiate(prefabPickupInk, this.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void PlayerDetected(Vector3 position)
    {
        if(isPatrolling)
        {
            beforeAttackPosition = this.transform.position;
            isPatrolling = false;
            attackPosition = position;
            CustomLookAt(attackPosition);
        }        
    }
}
