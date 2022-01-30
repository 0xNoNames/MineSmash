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
    [SerializeField] private float secondJumpRatio = 0.75f;
    [SerializeField] private int maxJumps = 2;

    public Vector2 bump;

    private Vector2 keyInput;
    private int jumpCount;
    private bool isDesactivated;
    private bool isGrounded;
    private bool isFastFalling;
    private bool isFirstJump;

    private void FixedUpdate()
    {
        if (isDesactivated)
            return;

        isGrounded = Physics2D.BoxCast(narrowBodyCollider.bounds.center, narrowBodyCollider.bounds.size, 0f, Vector2.down, .1f, collisionLayer);
        //isLeftBlocked = Physics2D.BoxCast(narrowBodyCollider.bounds.center, narrowBodyCollider.bounds.size, 0f, Vector2.left, .1f, collisionLayer) || Physics2D.BoxCast(wideBodyCollider.bounds.center, wideBodyCollider.bounds.size, 0f, Vector2.left, .1f, collisionLayer);
        //isRightBlocked = Physics2D.BoxCast(narrowBodyCollider.bounds.center, narrowBodyCollider.bounds.size, 0f, Vector2.right, .1f, collisionLayer) || Physics2D.BoxCast(wideBodyCollider.bounds.center, wideBodyCollider.bounds.size, 0f, Vector2.right, .1f, collisionLayer);

        if (isGrounded)
        {
            jumpCount = maxJumps;
            isFastFalling = false;
            isFirstJump = true;
        }

        if (keyInput.y < -0.5 && rigidBody.velocity.y <= 0 && !isGrounded)
            FastFall();

        float horizontalMovement = keyInput.x * moveSpeed * Time.fixedDeltaTime;

        rigidBody.velocity = new Vector2(horizontalMovement + bump.x, rigidBody.velocity.y);

        Flip(rigidBody.velocity.x);
    }

    public void Move(InputAction.CallbackContext _keyInput)
    {
        keyInput = _keyInput.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext keyInput)
    {
        if (isDesactivated)
            return;

        // Deuxi�me saut
        if (keyInput.started && !isGrounded && jumpCount > 1)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpHeight * secondJumpRatio);
            jumpCount--;
            return;
        }

        // Fait sauter le joueur
        if (keyInput.started && isGrounded)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpHeight);
        }

        // Fait redescendre le joueur pour la hauteur selon le temps d'appui
        if (keyInput.canceled && rigidBody.velocity.y > 0f && isFirstJump)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y * 0.25f);
            isFirstJump = false;
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
            spriteRenderer.flipX = true;
        else if (_velocity < -0.1f)
            spriteRenderer.flipX = false;
    }
}
