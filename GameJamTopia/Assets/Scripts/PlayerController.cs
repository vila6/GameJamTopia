using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float jumpForce, jumpBrushForce;

    private Rigidbody playerRgbd;
    private Vector3 extraMovement, originalExtraMovement;

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

        // Salto
        if(jumpButtonPressed)
        {
            if(isOnGround)
            {
                endVelocity = new Vector3(endVelocity.x, 0, 0);
                endVelocity += new Vector3(0, jumpForce, 0);
                jumpButtonPressed = false;
            }
            else if(isOnBrushLeft || isOnBrushRight)
            {
                endVelocity = new Vector3(endVelocity.x, 0, 0);
                endVelocity += new Vector3(0, jumpBrushForce, 0);
                jumpButtonPressed = false;
            } 
        }

        // Movimiento extra (Por ejemplo al recibir daño)
        if(extraMovement != Vector3.zero)
        {
            endVelocity += extraMovement;
            extraMovement -= extraMovement * Time.deltaTime * 4f;
            if((originalExtraMovement.x < 0 && (extraMovement.x > -0.1f || endVelocity.x > -0.1f)) || (originalExtraMovement.x > 0 && (extraMovement.x < 0.1f || endVelocity.x < 0.1f)))
            {
                extraMovement = Vector3.zero;
            }
        }

        if(endVelocity.x > speed)
        {
            endVelocity = new Vector3(speed, endVelocity.y, 0);
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

    /// <summary>
    /// Pasar el punto desde el que recibe el golpe ()
    /// </summary>
    public void HitMovement(Vector3 origin)
    {
        // Eje X
        if(origin.x > this.transform.position.x)
        {
            // Golpe de la izquierda
            extraMovement = new Vector3(-1, 0, 0).normalized;
        }
        else
        {
            // Golpe de la derecha
            extraMovement = new Vector3(1, 0, 0).normalized;
        }

        originalExtraMovement = extraMovement;
        extraMovement *= 30f;
    }
}
