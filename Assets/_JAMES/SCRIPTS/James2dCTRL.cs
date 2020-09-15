using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class James2dCTRL : MonoBehaviour
{
    /*
    2D Custom Character Controller by James H. Cameron 2020

    NOTE: 
    Use with BoxCollider2D component.  
    Modify Physics2D.gravity to your needs in Edit > Project Settings > Physics 2D
    Make sure AutoSync Transforms is checked in Edit > Project Settings > Physics 2D
    DO NOT adjust collider size with Edit Collider.  Just change the size values in the component
    */

    // CHARACTER ATTRIBUTES
    [SerializeField, Tooltip("Max speed, in units per second, that the character moves.")]
    float speed = 9f;

    [SerializeField, Tooltip("Max climb speed, in units per second, that the character moves.")]
    float climbSpeed = 5f;

    [SerializeField, Tooltip("Acceleration while Grounded.")]
    float walkAcceleration = 75f;

    [SerializeField, Tooltip("Acceleration while in the air.")]
    float airAcceleration = 30f;

    [SerializeField, Tooltip("Deceleration while in the air.")]
    float airDeceleration = 20f;

    [SerializeField, Tooltip("Deceleration applied when character is Grounded and not attempting to move.")]
    float groundDeceleration = 70f;

    [SerializeField, Tooltip("Max height the character will jump regardless of gravity")]
    float jumpHeight = 4f;

    [SerializeField]
    float fallMultiplier = 2.5f;

    [SerializeField]
    float lowJumpMultiplier = 2f;

    [SerializeField]
    float maxCoyoteTime = .04f;

    private float coyoteTimer;

    [SerializeField]
    float jumpPressRememberTime = .1f;

    private float jumpPressRemember = 0;

    [SerializeField]
    float deathDelay = .5f;

    // WALL JUMP

    [SerializeField]
    float wallJumpTime = .2f;

    [SerializeField]
    float wallSlideSpeed = 2f;

    [SerializeField]
    float wallDistance = .55f;

    public bool isWallSliding = false;

    public bool isWallGrabbing = false;

    RaycastHit2D wallCheckHit;

    private float jumpTime;

    private bool grabPressRemember = false;

    private GameObject wallObject;


    // MOVING PLAT

    RaycastHit2D mPlatCheckHit;

    RaycastHit2D mWallCheckHit;

    [SerializeField]
    float mPlatDistance = 1f;

    private GameObject platObject;
    

    // COMPONENT REFERENCES
    private BoxCollider2D boxCollider;

    private Animator animator;

    // PLAYER STATES
    public bool isGrounded;

    private bool isAlive = true;

    private bool isFacingRight = true;



    /// //////////////////////////////////////

    [HideInInspector]
    public Vector2 velocity;

    private float acceleration;

    private float deceleration;

    public LayerMask groundLayer;

    private GameManager gm;

    private AudioManager am;

    /// //////////////////////////////////////

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();

        animator = GetComponentInChildren<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
        am = FindObjectOfType<AudioManager>();
        transform.position = gm.lastCheckPointPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive)
            return;

        MoveAbout();

        HandleCollision();

        Jump();

        Fall();

        Drift();

        FlipSprite();

        WallCling();

        Animate();

        Death();

        StickToPlat();


    }

    void MoveAbout()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        // WHEN WE HAVE AN INPUT VALUE, APPROACH MAX SPEED WITH A RATE OF CHANGE OF WALK ACCELERATION
        if (moveInput != 0)
        {
            velocity.x = Mathf.MoveTowards(velocity.x, speed * moveInput, acceleration * Time.deltaTime);
        }
        // WHEN WE DONT HAVE AN INPUT VALUE, APPROACH 0 SPEED WITH A RATE OF CHANGE OF GROUNDDECELREATION
        else
        {
            velocity.x = Mathf.MoveTowards(velocity.x, 0, deceleration * Time.deltaTime);
        }

        transform.Translate(velocity * Time.deltaTime);

    }

    void HandleCollision()
    {
        isGrounded = false;

        // Retrieve all colliders we have intersected after velocity has been applied.
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, boxCollider.size, 0);

        foreach (Collider2D hit in hits)
        {
            // Ignore our own collider and ignore triggers
            if (hit == boxCollider || hit.isTrigger)
                continue;

            ColliderDistance2D colliderDistance = hit.Distance(boxCollider);

            // Ensure that we are still overlapping this collider.
            // The overlap may no longer exist due to another intersected collider pushing us out of this one. 
            if (colliderDistance.isOverlapped)
            {
                transform.Translate(colliderDistance.pointA - colliderDistance.pointB);

                // If we intersect an object beneath us, set Grounded to true. 
                if (Vector2.Angle(colliderDistance.normal, Vector2.up) < 90 && velocity.y < 0)
                {
                    isGrounded = true;
                }

            }

        }

    }

    void Jump()
    {
        if (isGrounded)
        {
            coyoteTimer = Time.time;

            velocity.y = 0;

        }

        jumpPressRemember -= Time.deltaTime;

        if (Input.GetButtonDown("Jump"))
        {
            jumpPressRemember = jumpPressRememberTime;
        }

        if ((jumpPressRemember > 0) && ((Time.time - coyoteTimer) < maxCoyoteTime))
        {
            jumpPressRemember = 0;

            velocity.y = Mathf.Sqrt(2 * jumpHeight * Mathf.Abs(Physics2D.gravity.y));

            am.PlaySound("Jump");
        }

    }

    void Fall()
    {
        velocity.y += Physics2D.gravity.y * Time.deltaTime;

        if (velocity.y < 0)
        {
            velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (velocity.y > 0 && !Input.GetButton("Jump"))
        {
            velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

    }

    void Drift()
    {
        // DEFINITELY WANT TO TOY WITH AIR ACCEL AND DECCEL
        acceleration = isGrounded ? walkAcceleration : airAcceleration;
        deceleration = isGrounded ? groundDeceleration : airDeceleration;

    }

    void FlipSprite()
    {
        // IF PLAYER'S X VELOCITY > EPSILON THEN THIS BOOL WILL RETURN AS TRUE
        bool hasHorizontalSpeed = Mathf.Abs(velocity.x) > Mathf.Epsilon;

        if (hasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(velocity.x), 1f);
        }

    }

    void WallCling()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            grabPressRemember = true;
        }

        if (Input.GetButtonUp("Fire1"))
        {
            grabPressRemember = false;
        }

        float moveInput = Input.GetAxisRaw("Horizontal");

        if (moveInput < 0)
        {
            isFacingRight = false;
        }
        else if (moveInput > 0)
        {
            isFacingRight = true;
        }

        if (isFacingRight) // IF FACING RIGHT THEN CAST A RAY TO THE RIGHT TO CHECK FOR THE WALL
        {
            wallCheckHit = Physics2D.Raycast(transform.position, new Vector2(wallDistance, 0), wallDistance, groundLayer);

            Debug.DrawRay(transform.position, new Vector2(wallDistance, 0), Color.green);
        }
        else // OTHERWISE CHECK LEFT
        {
            wallCheckHit = Physics2D.Raycast(transform.position, new Vector2(-wallDistance, 0), wallDistance, groundLayer);

            Debug.DrawRay(transform.position, new Vector2(-wallDistance, 0), Color.green);
        }

        if (wallCheckHit && !isGrounded && moveInput != 0)
        {
            isWallSliding = true;
            jumpTime = Time.time + wallJumpTime;
        }
        else if (jumpTime < Time.time)
        {
            isWallSliding = false;
        }

        if (wallCheckHit && grabPressRemember)
        {
            isWallGrabbing = true;
            jumpTime = Time.time + wallJumpTime;
        }
        else if (jumpTime < Time.time)
        {
            isWallGrabbing = false;
        }

        if (isWallSliding)
        {
            velocity = new Vector2(velocity.x, Mathf.Clamp(velocity.y, -wallSlideSpeed, float.MaxValue));
        }

        if (isWallSliding && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(2 * jumpHeight * Mathf.Abs(Physics2D.gravity.y));
        }

        if (isWallGrabbing)
        {
            wallObject = wallCheckHit.transform.gameObject;

            velocity = new Vector2(velocity.x, Mathf.Clamp(velocity.y, 0, float.MaxValue));

            // CHECK TO SEE IF YOU ARE GRABBING A STATIONARY WALL OR MOVING WALL
            if (wallObject.tag == "Moving Plats")
            {
                // IF WE ARE GRABBING A MOVING WALL THEN CHILD THE PLAYER'S TRANSFORM TO THE WALL UNTIL WE LET GO
                this.gameObject.transform.SetParent(wallObject.transform);
                Debug.Log("i am touching u");
            }
            else
            {
                // IF STATIONARY WALL THEN VELOCITY.Y = 0 AND SET TRANSFORM PARENT TO NULL
                this.gameObject.transform.SetParent(null);
            }

        }

        if (isWallGrabbing && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(2 * jumpHeight * Mathf.Abs(Physics2D.gravity.y));

            this.gameObject.transform.SetParent(null);

        }


    }

    void StickToPlat()
    {
        mPlatCheckHit = Physics2D.Raycast(transform.position, new Vector2(0, -mPlatDistance), mPlatDistance, groundLayer);

        if (mPlatCheckHit.collider == null)
        {
            return;
        }

        platObject = mPlatCheckHit.transform.gameObject;
        Debug.DrawRay(transform.position, new Vector2(0, -mPlatDistance), Color.green);
        Debug.Log(platObject.tag);

        if ((mPlatCheckHit) && platObject.tag == "Moving Plats")
        {
            this.gameObject.transform.SetParent(platObject.transform);
            Debug.Log("i am touching u");
        }
        else
        {
            this.gameObject.transform.SetParent(null);
            Debug.Log("not touching");
        }

    }
    
    void Animate()
    {
        // IF PLAYER'S X VELOCITY > EPSILON THEN THIS BOOL WILL RETURN AS TRUE
        bool hasHorizontalSpeed = Mathf.Abs(velocity.x) > Mathf.Epsilon;

        animator.SetBool("isRunning", hasHorizontalSpeed);

    }
    
    void Death()
    {
        if (boxCollider.IsTouchingLayers(LayerMask.GetMask("Enemy")) || boxCollider.IsTouchingLayers(LayerMask.GetMask("Hazards")))
        {
            Debug.Log("I am ded");

            isAlive = false;

            StartCoroutine(Die());

        }

    }

    IEnumerator Die()
    {
        animator.SetTrigger("Killed");
        am.PlaySound("Player Death");

        yield return new WaitForSecondsRealtime(deathDelay);

        FindObjectOfType<GameManager>().RestartLVL();

    }







    // END OF FILE
}
