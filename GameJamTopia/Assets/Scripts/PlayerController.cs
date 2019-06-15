using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float jumpForce, jumpBrushForce;

    private Rigidbody playerRgbd;

    // Movimiento
    private Vector3 extraMovement, originalExtraMovement;
    private bool isGoingRight = true;

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
    public Transform shootPositionRight, shootPositionLeft;
    private bool shootButtonPressed = false;
    private bool canShoot = true;
    public float shootDelay = 0.5f;
    public int inkNeededToShot = 10;

    // Carga tinta
    [SerializeField]
    [Range(0, 1000)]
    public int maxInk = 200;
    private int inkCharge = 100;
    public TextMesh inkDiegeticDebug;
    public GameObject inkContainer;

    // Animator
    public Animator anim;
    public GameObject crabMesh;

    // Ataque
    public GameObject rightBrushAttackCollider, leftBrushAttackCollider;
    private float attackMeleeDuration = 0.08f * 0.38f; // OJO Tiene q ser el tiempo exacto
    public float attackMeleeExtraCooldown = 0.3f;
    private bool onAttackDuration = false;
    private bool onAttackCooldown = false;

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
        anim = GetComponentInChildren<Animator>();
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

        // Animacion melee
        if (!onAttackCooldown && Input.GetButtonDown("Fire2"))
        {
            anim.SetTrigger("attack");
            onAttackDuration = true;
            onAttackCooldown = true;
            if(isGoingRight)
            {
                rightBrushAttackCollider.SetActive(true);
            }
            else
            {
                leftBrushAttackCollider.SetActive(true);
            }
            StartCoroutine(AttackBrushColliderDelay());            
        }
    }

    private IEnumerator AttackBrushColliderDelay()
    {
        yield return new WaitForSeconds(attackMeleeDuration);
        if(isGoingRight)
        {
            rightBrushAttackCollider.SetActive(false);
        }
        else
        {
            leftBrushAttackCollider.SetActive(false);
        }
        onAttackDuration = false;
        
        yield return new WaitForSeconds(attackMeleeExtraCooldown);
        onAttackCooldown = false;
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
                anim.SetTrigger("jump");
                endVelocity = new Vector3(endVelocity.x, 0, 0);
                endVelocity += new Vector3(0, jumpForce, 0);
                jumpButtonPressed = false;
            }
            else if(isOnBrushLeft || isOnBrushRight)
            {
                anim.SetTrigger("jump");
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

        // Animation stuff
        anim.SetFloat("hSpeed", Mathf.Abs(velocityX));
        anim.SetFloat("vSpeed", playerRgbd.velocity.y);

        // Para saber si va hacia la derecha o hacia la izquierda
        if(!onAttackDuration && velocityX != 0)
        {
            isGoingRight = velocityX > 0;
            anim.SetBool("goingRight", isGoingRight);
        }

        anim.SetBool("hanging", isOnBrushLeft || isOnBrushRight);
        // Flip character when player changes direction
        if(!onAttackDuration)
        {
            Vector3 newScale = crabMesh.transform.localScale;
            if (velocityX > 0)
            {
                newScale.x = 1;
                crabMesh.transform.localScale = newScale;
            }else if (velocityX < 0)
            {
                newScale.x = -1;
                crabMesh.transform.localScale = newScale;
            }
        }        
    }

    private void Shoot(){
        if(inkCharge >= inkNeededToShot && canShoot && shootButtonPressed)
        {
            if(isGoingRight)
            {
                GameObject projectile = Instantiate(projectilePrefab, shootPositionRight.position, Quaternion.identity) as GameObject;
                projectile.GetComponent<ProjectileController>().SetDirectionRight();
            }
            else
            {
                GameObject projectile = Instantiate(projectilePrefab, shootPositionLeft.position, Quaternion.identity);
                projectile.GetComponent<ProjectileController>().SetDirectionLeft();
            }
            
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
        Vector3 newInkScale = inkContainer.transform.localScale;
        newInkScale.z = (float) inkCharge / maxInk;
        inkContainer.transform.localScale = newInkScale;
    }

    public void SetIsOnGround(bool value)
    {
        isOnGround = value;
        anim.SetBool("isGrounded", value);
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
            anim.SetBool("goingRight", false);
        }
        else
        {
            // Golpe de la derecha
            extraMovement = new Vector3(1, 0, 0).normalized;

            anim.SetBool("goingRight", true);
        }

        anim.SetTrigger("knockback");

        originalExtraMovement = extraMovement;
        extraMovement *= 30f;
    }
}
