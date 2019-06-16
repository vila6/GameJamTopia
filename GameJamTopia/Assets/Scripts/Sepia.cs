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
    public GameObject inkSpawn;
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
        int randomNum = Random.Range(0, 3);
        if (inkShots > 0 && randomNum == 2)
        {
            shot = Instantiate(inkProjectile, shootPoint.position, Quaternion.identity);
            inkShots--;
        }
        else
        {
            shot = Instantiate(waterProjectile, shootPoint.position, Quaternion.identity);
        }
        shot.GetComponent<ProjectileController>().SetDirection(shootPoint.position + transform.right);

        //Spawn de particulas de tinta en cono
        Vector3 spawnParticlesPosition = new Vector3 (shootPoint.position.x, shootPoint.position.y, shootPoint.position.z);

        GameObject inkCone = Instantiate(inkSpawn, spawnParticlesPosition, Quaternion.Euler(new Vector3 (0f,shot.GetComponent<ProjectileController>().direction.x * 90f, 0f)));

        Destroy(inkCone, 1f);
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
