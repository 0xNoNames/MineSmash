using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private BoxCollider2D wideBodyCollider;
    [SerializeField] private BoxCollider2D narrowBodyCollider;
    [SerializeField] private LayerMask collisionLayer;
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private BumpSystem bumpSystem;

    [SerializeField] private GameObject dummyPrefab;

    [SerializeField] private float moveSpeed = 300f;
    [SerializeField] private float jumpHeight = 20f;
    [SerializeField] private float secondJumpRatio = 0.75f;
    [SerializeField] private int maxJumps = 2;

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

        if (isGrounded)
        {
            jumpCount = maxJumps;
            isFastFalling = false;
            isFirstJump = true;
        }

        if (keyInput.y < -0.5 && rigidBody.velocity.y <= 0 && !isGrounded && bumpSystem.value == Vector2.zero)
            FastFall();

        float horizontalVelocity = keyInput.x * moveSpeed * Time.fixedDeltaTime;
        float verticalVelocity = keyInput.y * moveSpeed * Time.fixedDeltaTime;

        if (Mathf.Abs(bumpSystem.value.x) > 0)
            horizontalVelocity /= 2;

        if (Mathf.Abs(bumpSystem.value.y) > 0)
            verticalVelocity /= 2;

        rigidBody.velocity = new Vector2(horizontalVelocity + bumpSystem.value.x, rigidBody.velocity.y + bumpSystem.value.y);

        if (Mathf.Abs(bumpSystem.value.y) > 0)
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, verticalVelocity + bumpSystem.value.y);

        if (Mathf.Abs(bumpSystem.value.x) - Mathf.Abs(horizontalVelocity) <= 0)
            bumpSystem.value.x = 0;

        if (Mathf.Abs(bumpSystem.value.y) - Mathf.Abs(verticalVelocity) <= 0)
            bumpSystem.value.y = 0;

        Flip(rigidBody.velocity.x);
    }

    public void Debug(InputAction.CallbackContext _keyInput)
    {
        if (_keyInput.performed)
        {
            if (GameManager.Instance.dummy != null)
            {
                DummyDetails DD = GameManager.Instance.dummy.GetComponent<DummyDetails>();
                DD.currentPercentage += 5;
                UIManager.Instance.GetDummyUI(DD.dummyID).SetPercentage(DD.currentPercentage);
            }
            else
            {
                GameManager.Instance.dummy = Instantiate(dummyPrefab);
                DummyDetails dummyDetails = dummyPrefab.GetComponent<DummyDetails>();
                dummyDetails.Initialize(0, Vector2.zero);

                UIManager.Instance.GetDummyUI(dummyDetails.dummyID).gameObject.SetActive(true);
                UIManager.Instance.GetDummyUI(dummyDetails.dummyID).SetDummyName(dummyDetails.dummyName);
            }
        }
    }

    public void Move(InputAction.CallbackContext _keyInput)
    {
        keyInput = _keyInput.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext keyInput)
    {
        if (bumpSystem.value != Vector2.zero)
            return;

        // Deuxieme saut
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
        if (!isGrounded && !isFastFalling && !isDesactivated)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, -jumpHeight);
            isFastFalling = true;
        }
    }

    public void SetDesactivateState(bool state)
    {
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
