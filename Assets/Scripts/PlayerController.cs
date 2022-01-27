using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private BoxCollider2D wideBodyCollider;
    [SerializeField] private BoxCollider2D narrowBodyCollider;
    [SerializeField] private LayerMask collisionLayer;
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private float moveSpeed = 300f;
    [SerializeField] private float jumpHeight = 20f;
    [SerializeField] private float secondJumpRatio = 1.25f;
    [SerializeField] private int maxJumps = 2;

    private int jumpCount;
    private bool isDesactivated;
    private bool isLeftBlocked;
    private bool isRightBlocked;
    private bool isGrounded;
    private bool isFastFalling;

    private void FixedUpdate()
    {
        isGrounded = Physics2D.BoxCast(narrowBodyCollider.bounds.center, narrowBodyCollider.bounds.size, 0f, Vector2.down, .1f, collisionLayer);
        isLeftBlocked = Physics2D.BoxCast(narrowBodyCollider.bounds.center, narrowBodyCollider.bounds.size, 0f, Vector2.left, .1f, collisionLayer) || Physics2D.BoxCast(wideBodyCollider.bounds.center, wideBodyCollider.bounds.size, 0f, Vector2.left, .1f, collisionLayer);
        isRightBlocked = Physics2D.BoxCast(narrowBodyCollider.bounds.center, narrowBodyCollider.bounds.size, 0f, Vector2.right, .1f, collisionLayer) || Physics2D.BoxCast(wideBodyCollider.bounds.center, wideBodyCollider.bounds.size, 0f, Vector2.right, .1f, collisionLayer);

        if (isGrounded)
        {
            jumpCount = maxJumps;
            isFastFalling = false;
        }
    }

    public void Move(InputAction.CallbackContext keyPress)
    {
        if (isDesactivated)
            return;

        Vector2 inputMove = keyPress.ReadValue<Vector2>();

        if (inputMove.y < -0.5)
            FastFall();

        float horizontalMovement = inputMove.x * moveSpeed * Time.fixedDeltaTime;

        if (!isLeftBlocked && horizontalMovement < 0 || !isRightBlocked && horizontalMovement > 0)
            rigidBody.velocity = new Vector2(horizontalMovement, rigidBody.velocity.y);
        else
            rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);

        Flip(rigidBody.velocity.x);
    }

    public void Jump(InputAction.CallbackContext keyPress)
    {
        if (isDesactivated)
            return;

        // Deuxième saut
        if (keyPress.started && !isGrounded && jumpCount > 1)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpHeight / secondJumpRatio);
            jumpCount--;
            return;
        }

        // Fait sauter le joueur
        if (keyPress.started && isGrounded)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpHeight);
        }

        // Fait redescendre le joueur pour la hauteur selon le temps d'appui
        if (keyPress.canceled && rigidBody.velocity.y > 0f)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y * 0.5f);
        }

        isFastFalling = false;
    }

    private void FastFall()
    {
        if (!isGrounded && !isFastFalling)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, -jumpHeight);
            isFastFalling = true;
        }
    }

    public void SetDesactivateState(bool state, bool stopPlayer)
    {
        if (stopPlayer)
            rigidBody.velocity = new Vector2(0f, 0f);
        isDesactivated = state;
    }

    public bool GetDesactivateState()
    {
        return isDesactivated;
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
