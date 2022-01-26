using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Camera cam;
    [SerializeField] private SpriteRenderer sprite;

    private Rigidbody2D playerRB;
    private Vector2 mousePos;

    private void Start() => playerRB = player.GetComponent<Rigidbody2D>();

    void Update() => mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

    private void FixedUpdate()
    {
        Vector2 lookDir = mousePos - playerRB.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x);

        float distance = 1f;

        sprite.transform.position = new Vector2(playerRB.position.x + Mathf.Cos(angle) * distance, playerRB.position.y + Mathf.Sin(angle) * distance);

        sprite.transform.eulerAngles = new Vector3(0, 0, angle * Mathf.Rad2Deg - 90f);
    }
}
