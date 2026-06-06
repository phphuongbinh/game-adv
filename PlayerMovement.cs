using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 5f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector2 groundCheckBox;
    [SerializeField] private LayerMask groundMask;

    [Header("Double Jump")]
    [SerializeField] private int maxJumps = 2;
    private int jumpCount;


    [Header("Jump Assist ")]
    [SerializeField] private float jumpBufferTime = 0.15f;
    [SerializeField] private float coyoteTime = 0.15f;
    [SerializeField] private float jumpBufferCounter;
    [SerializeField] private float coyoteTimeCounter;

    [Header("Dash")]
    [SerializeField] private float dashingPower = 24f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 1f;
    [SerializeField] private TrailRenderer tr;

    //** Components
    private Rigidbody2D rb;
    private Animator anim;

    //** Input
    private float xInput;

    //** State
    private bool isFacingRight = true;

    public bool canMove = true;
    private bool canDash = true;
    public bool isDashing = false;

    public bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (!canMove) return;

        CheckGround();
        HandleInput();
        HandleFlip();
        HandleBufferJump();
        UpdateAnimations();
    }

    void FixedUpdate()
    {
        if (!canMove) return;
        if (isDashing) return;

        Move();
    }

    private void HandleInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        // Ghi nho nguoi choi bam Space
        if (Input.GetKeyDown(KeyCode.X))
        {
            jumpBufferCounter = jumpBufferTime;
        }

        if (Input.GetKeyDown(KeyCode.Z) && canDash && !isDashing)
        {
            StartCoroutine(Dash());
        }
    }

    private void HandleBufferJump()
    {
        // Jump Buffer 
        jumpBufferCounter -= Time.deltaTime;

        // Coyote Time 
        if (isGrounded)
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;

        // Nếu người chơi bấm jump gần đây + vẫn còn coyote time => cho nhảy
        if (jumpBufferCounter > 0 && coyoteTimeCounter > 0 && !isDashing)
        {
            Jump();
            jumpBufferCounter = 0;
            coyoteTimeCounter = 0;
        }
    }

    private void Move()
    {
        rb.velocity = new Vector2(xInput * speed, rb.velocity.y);
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void CheckGround()
    {
        if (groundCheck == null) return;

        isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckBox, 0f, groundMask);
    }

    private void HandleFlip()
    {
        if (xInput > 0 && !isFacingRight)
            Flip();
        else if (xInput < 0 && isFacingRight)
            Flip();
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;

        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    private void UpdateAnimations()
    {
        anim.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
        anim.SetBool("isGrounded", isGrounded);
    }

    public void StopMove()
    {
        rb.velocity = Vector2.zero;
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        rb.velocity = new Vector2((isFacingRight ? 1 : -1) * dashingPower, 0f);

        if (tr != null) tr.emitting = true;

        yield return new WaitForSeconds(dashingTime);

        if (tr != null) tr.emitting = false;

        rb.gravityScale = originalGravity;
        isDashing = false;

        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(groundCheck.position, groundCheckBox);
    }
}