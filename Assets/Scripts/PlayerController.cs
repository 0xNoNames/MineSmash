using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 300;
    [SerializeField] private float jumpHeight = 15;
    [SerializeField] private int maxJumps = 2;

    [SerializeField] private BoxCollider2D wideBodyCollider;
    [SerializeField] private BoxCollider2D narrowBodyCollider;
    [SerializeField] private LayerMask collisionLayer;
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    public bool isDesactivated;

    [SerializeField] private float maxJumpTime = 0.15f;
    private float jumpTimeCounter;
    private int jumpCount;

    private Vector2 inputMove;

    private bool isGrounded;
    private bool isLeftBlocked;
    private bool isRightBlocked;
    private bool isFastFalling;
    private bool hasFell;

    void Update()
    {
        if (isDesactivated)
            return;

        if (inputMove.y < -0.5)
            FastFall();

        float horizontalMovement = inputMove.x * moveSpeed * Time.fixedDeltaTime;

        if (!isLeftBlocked && horizontalMovement < 0 || !isRightBlocked && horizontalMovement > 0)
            rigidBody.velocity = new Vector2(horizontalMovement, rigidBody.velocity.y);
        else
            rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);

        Flip(rigidBody.velocity.x);
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.BoxCast(narrowBodyCollider.bounds.center, narrowBodyCollider.bounds.size, 0f, Vector2.down, .1f, collisionLayer);
        isLeftBlocked = Physics2D.BoxCast(narrowBodyCollider.bounds.center, narrowBodyCollider.bounds.size, 0f, Vector2.left, .1f, collisionLayer) || Physics2D.BoxCast(wideBodyCollider.bounds.center, wideBodyCollider.bounds.size, 0f, Vector2.left, .1f, collisionLayer);
        isRightBlocked = Physics2D.BoxCast(narrowBodyCollider.bounds.center, narrowBodyCollider.bounds.size, 0f, Vector2.right, .1f, collisionLayer) || Physics2D.BoxCast(wideBodyCollider.bounds.center, wideBodyCollider.bounds.size, 0f, Vector2.right, .1f, collisionLayer);

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

    public void Move(InputAction.CallbackContext context)
    {
        inputMove = context.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && jumpCount > 0)
        {
            jumpCount--;
            isFastFalling = false;
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpHeight);
        }

        if (context.canceled && rigidBody.velocity.y > 0f)
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y * 0.5f);

        //if (Input.GetButton("Jump") && !isGrounded && jumpCount == maxJumps - 1)
        //{
        //    if (jumpTimeCounter > 0f)
        //    {
        //        rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpHeight);
        //        jumpTimeCounter -= Time.deltaTime;
        //    }
        //}
    }

    void FastFall()
    {
        if (!isGrounded && !isFastFalling)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, -jumpHeight);
            isFastFalling = true;
        }
    }

    public void SetDesactivateStateDeath(bool state)
    {
        rigidBody.velocity = new Vector2(0, 0);
        isDesactivated = state;
    }

    public void SetDesactivateStateHit(bool state)
    {
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
