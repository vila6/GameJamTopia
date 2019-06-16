using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;

    // Salto
    public float jumpForce, jumpBrushForce;
    private float jumpImpulseDuration = 0.333f / 0.46f; // OJO Tiene q ser el tiempo exacto
    private bool onJumpImpulseDuration = false;

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
    public int maxInk = 180;
    public int inkCharge = 0;
    public TextMesh inkDiegeticDebug;
    public GameObject inkContainer;
    public float timeToMarkInkAdded = 1.5f;
    private bool markingInkWon = false;

    // Animator
    public Animator anim;
    public GameObject crabMesh;

    // Ataque
    public GameObject rightBrushAttackCollider, leftBrushAttackCollider;
    private float attackMeleeDuration = 0.133f / 0.38f; // OJO Tiene q ser el tiempo exacto
    public float attackMeleeExtraCooldown = 0.3f;
    private bool onAttackDuration = false;
    private bool onAttackCooldown = false;

    // Invulnerability
    public float invulnerabilityTime = 1.5f;
    public GameObject crabMeshPeroDeVerdad;
    public bool isInvulnerable;
    private float hitTime;

    //VFX
    [Header("VFX")]
    public GameObject vfxJump;
    public GameObject vfxWalk;
    public GameObject vfxInkSplash;

    // Audios
    [Header("Audio")]
    private AudioSource myAudioSource;
    private bool walkingAudio = false;
    private bool wasOnGround = false;
    public AudioClip audioJump;
    public AudioClip audioJumpBrush;
    public AudioClip[] audioSteep;
    public AudioClip[] audioDamage;
    public AudioClip audioLanding;
    public AudioClip audioPickUpInk;
    //public AudioClip audioShootInk;
    

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
        myAudioSource = this.GetComponent<AudioSource>();
        RefreshDiegetic();

        StartCoroutine(WalkAudio());
    }

    private IEnumerator WalkAudio()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.1f);
            if(walkingAudio)
            {
                myAudioSource.volume = 0.2f;
                myAudioSource.PlayOneShot(audioSteep[Random.Range(0, audioSteep.Length)]);
            }
        }
    }

    void Update()
    {
        if((isOnGround || isOnBrushLeft || isOnBrushRight) && Input.GetButtonDown("Jump"))
        {
            jumpButtonPressed = true;
        }

        if(canShoot && Input.GetButtonDown("Fire2"))
        {
            shootButtonPressed = true;
        }

        // Animacion melee
        if (!onAttackCooldown && !isOnBrushLeft && !isOnBrushRight && Input.GetButtonDown("Fire1"))
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

        // Change color to mark invulnerability
        if (isInvulnerable)
        {
            float sinus = Mathf.Abs(Mathf.Sin((Time.time - hitTime) / invulnerabilityTime * 8f));
            Color newColor = new Color(sinus, sinus, sinus);
            crabMeshPeroDeVerdad.GetComponent<Renderer>().material.SetColor("_Color", newColor);
            Color inkContainerColor = Color.Lerp(Color.black, Color.red, sinus * 2);
            inkContainer.GetComponent<Renderer>().material.color = inkContainerColor;
        }

        // Mark ink added
        if (markingInkWon)
        {
            float sinus = Mathf.Abs(Mathf.Sin((Time.time - hitTime) / timeToMarkInkAdded * 8f));
            Color inkContainerColor = Color.Lerp(Color.black, Color.green, sinus * 2);
            inkContainer.GetComponent<Renderer>().material.color = inkContainerColor;
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

    private IEnumerator JumpImpulseAnimationDelay()
    {
        yield return new WaitForSeconds(jumpImpulseDuration);
        onJumpImpulseDuration = false;
    }

    void FixedUpdate()
    {
        Movement();
        Shoot();
    }

    private IEnumerator ParticlesMovement(float valueFacing)
    {
        //Spawn de particulas de movimiento
        Vector3 spawnParticlesPosition = new Vector3 (this.transform.position.x + valueFacing, this.transform.position.y - 0.5f, this.transform.position.z);
        GameObject movementParticles = Instantiate(vfxWalk, spawnParticlesPosition, Quaternion.Euler(new Vector3(0f, 90f * valueFacing, 0f)));
        Destroy(movementParticles, 0.5f);   

        yield return new WaitForSeconds (1f);     
    }


    private void Movement()
    {
        Vector3 endVelocity = new Vector3(0, playerRgbd.velocity.y, 0);

        // Comprobar colisiones laterales
        float velocityX = Input.GetAxis("Horizontal");

        if (velocityX > 0 && isOnGround)
        {
            StartCoroutine(ParticlesMovement(-1f));
        }
        else if (velocityX < 0 && isOnGround)
        {
            StartCoroutine(ParticlesMovement(1f));
        }

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
                //Spawn de particulas de salto
                Vector3 spawnParticlesPosition = new Vector3 (this.transform.position.x, this.transform.position.y - 0.6f, this.transform.position.z);
                GameObject jumpParticles = Instantiate(vfxJump, spawnParticlesPosition, Quaternion.identity);
                Destroy(jumpParticles, 2f);

                anim.SetTrigger("jump");
                endVelocity = new Vector3(endVelocity.x, 0, 0);
                endVelocity += new Vector3(0, jumpForce, 0);
                jumpButtonPressed = false;

                myAudioSource.volume = 1f;
                myAudioSource.PlayOneShot(audioJump);
            }
            else if(isOnBrushLeft || isOnBrushRight)
            {
                anim.SetTrigger("jump");
                endVelocity = new Vector3(endVelocity.x, 0, 0);
                endVelocity += new Vector3(0, jumpBrushForce, 0);
                jumpButtonPressed = false;
                onJumpImpulseDuration = true;
                StartCoroutine(JumpImpulseAnimationDelay());

                myAudioSource.volume = 0.5f;
                myAudioSource.PlayOneShot(audioJumpBrush);
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

        playerRgbd.velocity = endVelocity;

        // Animation stuff
        anim.SetFloat("hSpeed", Mathf.Abs(velocityX));
        anim.SetFloat("vSpeed", playerRgbd.velocity.y);

        // Para saber si va hacia la derecha o hacia la izquierda
        if(!onAttackDuration && !onJumpImpulseDuration && velocityX != 0)
        {
            isGoingRight = velocityX > 0;
            anim.SetBool("goingRight", isGoingRight);
        }

        anim.SetBool("hanging", isOnBrushLeft || isOnBrushRight);
        // Flip character when player changes direction
        if(!onAttackDuration && !onJumpImpulseDuration)
        {
            Vector3 newScale = crabMesh.transform.localScale;
            if (velocityX > 0)
            {
                newScale.x = 1;
                crabMesh.transform.localScale = newScale;
                
                walkingAudio = true;
            }else if (velocityX < 0)
            {
                newScale.x = -1;
                crabMesh.transform.localScale = newScale;

                walkingAudio = true;
            }
            else
            {
                walkingAudio = false;
            }
        }        
    }

    private void Shoot(){
        if(inkCharge >= inkNeededToShot && canShoot && shootButtonPressed)
        {
            anim.SetTrigger("shoot");
            
            //myAudioSource.volume = 1f;
            //myAudioSource.PlayOneShot(audioShootInk);

            if(isGoingRight)
            {

                //Camera Shake
                this.GetComponent<CameraShake>().shakeDuration = 0.5f;

                //Spawn de particulas de tinta en cono
                Vector3 spawnParticlesPosition = new Vector3 (shootPositionRight.position.x - 0.4f, shootPositionRight.position.y, shootPositionRight.position.z);
                GameObject inkCone = Instantiate(vfxInkSplash, spawnParticlesPosition, Quaternion.Euler(new Vector3 (0f, 90f, 0f)));
                Destroy(inkCone, 2f);

                GameObject projectile = Instantiate(projectilePrefab, shootPositionRight.position, Quaternion.identity) as GameObject;
                projectile.GetComponent<ProjectileController>().SetDirectionRight();
            }
            else
            {
                //Spawn de particulas de tinta en cono
                Vector3 spawnParticlesPosition = new Vector3 (shootPositionLeft.position.x + 0.4f, shootPositionLeft.position.y, shootPositionLeft.position.z);
                GameObject inkCone = Instantiate(vfxInkSplash, spawnParticlesPosition, Quaternion.Euler(new Vector3 (0f, -90f, 0f)));
                Destroy(inkCone, 2f);

                GameObject projectile = Instantiate(projectilePrefab, shootPositionLeft.position, Quaternion.identity);
                projectile.GetComponent<ProjectileController>().SetDirectionLeft();
            }
            
            shootButtonPressed = false;
            canShoot = false;

            inkCharge -= inkNeededToShot;
            RefreshDiegetic();

            StartCoroutine(DelayShoot());
        }
        else
        {
            shootButtonPressed = false;
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
        if(!isOnGround)
        {
            walkingAudio = false;
        }
        else if(isOnGround && !wasOnGround)
        {
            myAudioSource.volume = 0.5f;
            myAudioSource.PlayOneShot(audioLanding);            
        }
        wasOnGround = value;
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
        if (value > 0)
            StartCoroutine(InkAdded());
        RefreshDiegetic();

        if(value < 0)
        {
            myAudioSource.volume = 1f;
            myAudioSource.PlayOneShot(audioDamage[Random.Range(0, audioDamage.Length)]);
        }
    }

    public IEnumerator InkAdded()
    {
        myAudioSource.volume = 1f;
        myAudioSource.PlayOneShot(audioPickUpInk);
        
        markingInkWon = true;
        yield return new WaitForSeconds(timeToMarkInkAdded);
        markingInkWon = false;
        inkContainer.GetComponent<Renderer>().material.color = Color.black;
        float sinus = Mathf.Abs(Mathf.Sin((Time.time - hitTime) / invulnerabilityTime * 8f));
    }

    private IEnumerator HitInvulnerability()
    {
        StopCoroutine(InkAdded());
        isInvulnerable = true;
        hitTime = Time.time;

        //Camera Shake
        this.GetComponent<CameraShake>().shakeDuration = 0.5f;

        yield return new WaitForSeconds(invulnerabilityTime);

        crabMeshPeroDeVerdad.GetComponent<Renderer>().material.SetColor("_Color", new Color(1, 1, 1));
        inkContainer.GetComponent<Renderer>().material.color = Color.black;
        isInvulnerable = false;
    }

    /// <summary>
    /// Pasar el punto desde el que recibe el golpe ()
    /// </summary>
    public void HitSquid(Vector3 origin)
    {
        
        // Eje X
        if (origin.x > this.transform.position.x)
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
        StartCoroutine(HitInvulnerability());
        originalExtraMovement = extraMovement;
        extraMovement *= 30f;
    }

    public void HitBullet(GameObject origin)
    {
        extraMovement = origin.GetComponent<ProjectileController>().direction.normalized;
        anim.SetBool("goingRight", false);

        anim.SetTrigger("knockback");
        StartCoroutine(HitInvulnerability());
        originalExtraMovement = extraMovement;
        extraMovement *= 30f;
    }

    public float GetInkRatio()
    {
        float inkRatio = (float)inkCharge / (float)maxInk;
        return inkRatio;
    }
}
