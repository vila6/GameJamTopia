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
    private bool canShoot = true;
    public float shootDelay = 0.5f;
    public int inkNeededToShot = 10;

    // Carga tinta
    [SerializeField]
    private int inkCharge = 100;
    public TextMesh inkDiegeticDebug;

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

        RefreshDiegetic();
    }

    void Update()
    {
        if((isOnGround || isOnBrushLeft || isOnBrushRight) && Input.GetButtonDown("Jump"))
        {
            jumpButtonPressed = true;
        }

        if(canShoot && Input.GetButton("Fire1"))
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
        if(inkCharge >= inkNeededToShot && canShoot && shootButtonPressed)
        {
            Instantiate(projectilePrefab, shootPosition.position, shootPosition.rotation);
            shootButtonPressed = false;
            canShoot = false;

            inkCharge -= inkNeededToShot;
            RefreshDiegetic();

            StartCoroutine(DelayShoot());
        }
    }

    private IEnumerator DelayShoot()
    {
        yield return new WaitForSeconds(shootDelay);
        canShoot = true;
    }

    private void RefreshDiegetic()
    {
        inkDiegeticDebug.text = inkCharge.ToString(); // A SUSTITUIR POR EL SISTEMA DIEGETICO FINAL
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

    public void AddInk(int value)
    {
        if(inkCharge + value < 0)
        {
            inkCharge = 0;
        }
        else
        {
            inkCharge += value;
        }
        RefreshDiegetic();
    }
}
