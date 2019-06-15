using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float jumpForce, jumpBrushForce;

    private Rigidbody playerRgbd;

    // Singleton
    public static PlayerController instance = null;

    // Controles
    private bool jumpButtonPressed = false;
    private bool isOnGround, isOnBrushLeft, isOnBrushRight;    

    // Skin
    [HideInInspector]
    public bool collisionRight, collisionLeft;

    // Disparo
    public GameObject projectilePrefab;
    public Transform shootPosition;
    private bool shootButtonPressed = false;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);  
    }

    void Start()
    {
        playerRgbd = this.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if((isOnGround || isOnBrushLeft || isOnBrushRight) && Input.GetButtonDown("Jump"))
        {
            jumpButtonPressed = true;
        }

        if(Input.GetButtonDown("Fire1"))
        {
            shootButtonPressed = true;
        }
    }

    void FixedUpdate()
    {
        Movement();
        Shoot();
    }

    private void Movement()
    {
        Vector3 endVelocity = new Vector3(0, playerRgbd.velocity.y, 0);

        // Comprobar colisiones laterales
        float velocityX = Input.GetAxis("Horizontal");
        if(collisionRight && velocityX > 0f)
        {
            velocityX = 0f;
        }
        else if(collisionLeft && velocityX < 0)
        {
            velocityX = 0f;
        }

        endVelocity += new Vector3(velocityX * speed, 0, 0);

        if(jumpButtonPressed)
        {
            if(isOnGround)
            {
                endVelocity += new Vector3(0, jumpForce, 0);
                jumpButtonPressed = false;
            }
            else if(isOnBrushLeft || isOnBrushRight)
            {
                endVelocity += new Vector3(0, jumpBrushForce, 0);
                jumpButtonPressed = false;
            } 
        }

        playerRgbd.velocity = endVelocity;
    }

    private void Shoot(){
        if(shootButtonPressed)
        {
            Instantiate(projectilePrefab, shootPosition.position, shootPosition.rotation);
            shootButtonPressed = false;
        }
    }

    public void SetIsOnGround(bool value)
    {
        isOnGround = value;
    }

    public void SetIsOnBrushLeft(bool value)
    {
        isOnBrushLeft = value;
    }

    public void SetIsOnBrushRight(bool value)
    {
        isOnBrushRight = value;
    }
}
