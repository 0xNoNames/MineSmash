using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 300;
    public float jumpHeight = 10;
    public int maxJumps = 2;

    private int jumpCount;

    private bool isGrounded;
    private bool isFastFalling;
    private bool isDesactivated;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask collisionLayers;

    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;

    void Update()
    {
        if (isDesactivated)
            return;

        if (Input.GetButtonDown("Jump") && jumpCount > 0)
            Jump();

        if (Input.GetAxisRaw("Vertical") < 0)
            FastFall();

        MovePlayer(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.fixedDeltaTime);

        Flip(rb.velocity.x);
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, collisionLayers);

        if (isGrounded)
        {
            jumpCount = maxJumps;
            isFastFalling = false;
        }
    }

    void MovePlayer(float _horizontalMovement)
    {
        rb.velocity = new Vector2(_horizontalMovement, rb.velocity.y);
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
        --jumpCount;
        isFastFalling = false;
    }

    void FastFall()
    {
        if (!isGrounded && !isFastFalling)
        {
            rb.velocity = new Vector2(rb.velocity.x, -jumpHeight);
            isFastFalling = true;
        }
    }

    public void SetDesactivateState(bool state)
    {
        rb.velocity = new Vector2(0, 0);
        isDesactivated = state;
    }

    void Flip(float _velocity)
    {
        if (_velocity > 0.1f)
        {
            spriteRenderer.flipX = true;
        }
        else if (_velocity < -0.1f)
        {
            spriteRenderer.flipX = false;
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    //}
}
