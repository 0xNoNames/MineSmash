using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BowAim : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Rigidbody2D playerRB;

    private Camera cam;
    private Vector2 mousePos;

    public void Look(InputAction.CallbackContext context)
    {
        mousePos = cam.ScreenToWorldPoint(context.ReadValue<Vector2>());
    }

    private void Start()
    {
        cam = FindObjectOfType<Camera>();
    }

    private void FixedUpdate()
    {
        Vector2 lookDir = mousePos - playerRB.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x);

        float distance = 1f;

        sprite.transform.position = new Vector2(playerRB.position.x + Mathf.Cos(angle) * distance, playerRB.position.y + Mathf.Sin(angle) * distance);

        sprite.transform.eulerAngles = new Vector3(0, 0, angle * Mathf.Rad2Deg - 90f);
    }
}