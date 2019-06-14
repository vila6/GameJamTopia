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
    private bool isOnGround, isOnBrush;    

    // Skin
    [HideInInspector]
    public bool collisionRight, collisionLeft;

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
        if((isOnGround || isOnBrush) && Input.GetButtonDown("Jump"))
        {
            jumpButtonPressed = true;
        }
    }

    void FixedUpdate()
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
            else if(isOnBrush)
            {
                endVelocity += new Vector3(0, jumpBrushForce, 0);
                jumpButtonPressed = false;
            } 
        }

        playerRgbd.velocity = endVelocity;
    }

    public void SetIsOnGround(bool value)
    {
        isOnGround = value;
    }

    public void SetIsOnBrush(bool value)
    {
        isOnBrush = value;
    }
}
