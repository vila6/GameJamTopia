using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squid : MonoBehaviour
{
    public int life = 2;
    public int inkShots = 3;
    public float shootMaxCooldown = 1, shootCooldown = 1;
    public float movementMaxCooldown = 3, movementCooldown = 3;

    public GameObject inkProjectile;
    public GameObject waterProjectile;

    public GameObject prefabPickupInk;

    Transform target;

    private void Start()
    {
        target = PlayerController.instance.transform;
    }

    void Update()
    {
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
            shot = Instantiate(inkProjectile, this.transform.position, Quaternion.identity);
            inkShots--;
        }
        else
        {
            shot = Instantiate(waterProjectile, this.transform.position, Quaternion.identity);
        }
        shot.GetComponent<ProjectileController>().SetDirection(target.position);
        shot.transform.LookAt(target);
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
}
