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

    Transform target;

    private void Start()
    {
        target = PlayerController.instance.transform;
    }

    void Update()
    {
        if ( shootCooldown <= 0 )
        {
            // target = player
            Shoot();
            shootCooldown = shootMaxCooldown;
        }
        else
        {
            shootCooldown -= Time.deltaTime;
        }
    }

    public void Shoot()
    {
        //TODO lanzar proyectiles desde un shootpoint
        GameObject shot;
        if (inkShots > 0 && Random.Range(1, 2) == 1)
        {
            shot = Instantiate(inkProjectile);
            inkShots--;
        }
        else
        {
            shot = Instantiate(waterProjectile);
        }
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
        //TODO spawnear pickup de tinta
        Destroy(gameObject);
    }
}
