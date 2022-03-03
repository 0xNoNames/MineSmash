using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BowAim : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Transform playerTF;

    private Camera cam;
    private float distance = 0.75f;
    private Vector2 inputVector;
    private bool isUsingMouse;

    private void Awake()
    {
        cam = FindObjectOfType<Camera>();

        if (GetComponentInParent<PlayerInput>().currentControlScheme == "Keyboard&Mouse")
            isUsingMouse = true;
        else
            isUsingMouse = false;

        sprite.transform.position = new Vector2(playerTF.position.x + distance, playerTF.position.y);
        sprite.transform.eulerAngles = new Vector3(0, 0, -90f);
    }

    private void FixedUpdate()
    {
        Vector2 fixedInput = inputVector;

        if (isUsingMouse)
            fixedInput = cam.ScreenToWorldPoint(inputVector) - playerTF.position;

        float angle = Mathf.Atan2(fixedInput.y, fixedInput.x);

        sprite.transform.position = new Vector2(playerTF.position.x + Mathf.Cos(angle) * distance, playerTF.position.y + Mathf.Sin(angle) * distance);
        sprite.transform.eulerAngles = new Vector3(0, 0, angle * Mathf.Rad2Deg - 90f);
    }

    public void Aim(InputAction.CallbackContext directionInput)
    {
        Vector2 rawInput = directionInput.ReadValue<Vector2>();

        if (rawInput.magnitude <= 0.1f)
            return;

        inputVector = rawInput;
    }
}