using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 300;
    [SerializeField] private float jumpHeight = 15;
    [SerializeField] private int maxJumps = 2;

    [SerializeField] private BoxCollider2D headCollider;
    [SerializeField] private BoxCollider2D armsCollider;
    [SerializeField] private BoxCollider2D legsCollider;
    [SerializeField] private LayerMask collisionLayer;
    [SerializeField] private Rigidbody2D ridigBody;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private float maxJumpTime = 0.15f;
    private float jumpTimeCounter;
    private int jumpCount;

    private bool isGrounded;
    private bool isLeftBlocked;
    private bool isRightBlocked;
    private bool isFastFalling;
    private bool isDesactivated;
    private bool hasFell;

    void Update()
    {
        if (isDesactivated)
            return;

        if (Input.GetButtonDown("Jump") && jumpCount > 0)
            Jump();

        if (Input.GetButton("Jump") && !isGrounded && jumpCount == maxJumps - 1)
        {
            if (jumpTimeCounter > 0f)
            {
                ridigBody.velocity = new Vector2(ridigBody.velocity.x, jumpHeight);
                jumpTimeCounter -= Time.deltaTime;
            }
        }

        if (Input.GetAxisRaw("Vertical") < -0.5)
            FastFall();

        MovePlayer(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.fixedDeltaTime);

        Flip(ridigBody.velocity.x);
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.BoxCast(legsCollider.bounds.center, legsCollider.bounds.size, 0f, Vector2.down, .1f, collisionLayer);
        isLeftBlocked = Physics2D.BoxCast(headCollider.bounds.center, headCollider.bounds.size, 0f, Vector2.left, .1f, collisionLayer) || Physics2D.BoxCast(armsCollider.bounds.center, armsCollider.bounds.size, 0f, Vector2.left, .1f, collisionLayer) || Physics2D.BoxCast(legsCollider.bounds.center, legsCollider.bounds.size, 0f, Vector2.left, .1f, collisionLayer);
        isRightBlocked = Physics2D.BoxCast(headCollider.bounds.center, headCollider.bounds.size, 0f, Vector2.right, .1f, collisionLayer) || Physics2D.BoxCast(armsCollider.bounds.center, armsCollider.bounds.size, 0f, Vector2.right, .1f, collisionLayer) || Physics2D.BoxCast(legsCollider.bounds.center, legsCollider.bounds.size, 0f, Vector2.right, .1f, collisionLayer);

        if (!hasFell && !isGrounded && jumpCount == maxJumps)
        {
            hasFell = true;
            jumpCount--;
        }

        if (isGrounded)
        {
            jumpCount = maxJumps;
            isFastFalling = false;
            hasFell = false;
            jumpTimeCounter = maxJumpTime;
        }
    }

    void MovePlayer(float _horizontalMovement)
    {
        if (!isLeftBlocked && _horizontalMovement < 0 || !isRightBlocked && _horizontalMovement > 0)
            ridigBody.velocity = new Vector2(_horizontalMovement, ridigBody.velocity.y);
        else
            ridigBody.velocity = new Vector2(0, ridigBody.velocity.y);

    }
    void Jump()
    {
        jumpCount--;
        isFastFalling = false;
        ridigBody.velocity = new Vector2(ridigBody.velocity.x, jumpHeight);
    }

    void FastFall()
    {
        if (!isGrounded && !isFastFalling)
        {
            ridigBody.velocity = new Vector2(ridigBody.velocity.x, -jumpHeight);
            isFastFalling = true;
        }
    }

    public void SetDesactivateState(bool state)
    {
        //ridigBody.velocity = new Vector2(0, 0);
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
}
