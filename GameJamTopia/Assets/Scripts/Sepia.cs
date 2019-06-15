using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sepia : MonoBehaviour
{
    public int life = 2;
    public int inkShots = 3;
    public float shootMaxCooldown = 1, shootCooldown = 1;

    public GameObject inkProjectile;
    public GameObject waterProjectile;
    public Transform shootPoint;

    private bool isActive = false;

    void Update()
    {
        if(isActive){
            if ( shootCooldown <= 0 )
            {
                Shoot();
                shootCooldown = shootMaxCooldown;
            }
            else
            {
                shootCooldown -= Time.deltaTime;
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

    public void Shoot()
    {
        GameObject shot;
        if (inkShots > 0 && Random.Range(1, 2) == 1)
        {
            shot = Instantiate(inkProjectile, shootPoint.position, Quaternion.identity);
            inkShots--;
        }
        else
        {
            shot = Instantiate(waterProjectile, shootPoint.position, Quaternion.identity);
        }
        shot.GetComponent<ProjectileController>().SetDirection(shootPoint.position + transform.right);
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
        Destroy(gameObject);
    }

    public void SetIsActive(bool value)
    {
        isActive = value;
    }
}
