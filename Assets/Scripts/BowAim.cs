using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BowAim : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Rigidbody2D playerRB;

    private Camera cam;
    private Vector2 aimPos;
    private bool isUsingGamepad;

    private void Awake()
    {
        cam = FindObjectOfType<Camera>();
        if (GetComponentInParent<PlayerInput>().currentControlScheme == "Keyboard&Mouse")
            isUsingGamepad = true;
        else
            isUsingGamepad = false;
    }

    private void FixedUpdate()
    {
        Vector2 lookDir = aimPos;

        if (!isUsingGamepad)
            lookDir -= playerRB.position;

        float angle = Mathf.Atan2(lookDir.y, lookDir.x);

        float distance = 0.5f;

        sprite.transform.position = new Vector2(playerRB.position.x + Mathf.Cos(angle) * distance, playerRB.position.y + Mathf.Sin(angle) * distance);

        sprite.transform.eulerAngles = new Vector3(0, 0, angle * Mathf.Rad2Deg - 90f);
    }

    public void Aim(InputAction.CallbackContext inputAim)
    {
        if (isUsingGamepad)
            aimPos = inputAim.ReadValue<Vector2>();
        else
            aimPos = cam.ScreenToWorldPoint(inputAim.ReadValue<Vector2>());
    }

    public void OnDeviceChange(PlayerInput.ControlsChangedEvent deviceChange)
    {
        print(deviceChange);
    }
}