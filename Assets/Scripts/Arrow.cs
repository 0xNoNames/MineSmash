using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private BoxCollider2D coll;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private bool DEBUG;

    [SerializeField] private float shootForce = 20f;

    private bool destroyed;
    private Player shootingPlayer;


    private void Start() => body.AddForce(transform.up * shootForce, ForceMode2D.Impulse);

    private void FixedUpdate()
    {
        if (!destroyed)
        {
            float angle = Mathf.Atan2(body.velocity.x, body.velocity.y) * -Mathf.Rad2Deg;
            sprite.transform.eulerAngles = new Vector3(0, 0, angle);
        }
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.tag == "Player")
        {
            Player touchedPlayer = hitInfo.GetComponent<Player>();

            if (touchedPlayer != (shootingPlayer && !DEBUG))
            {
                touchedPlayer.Hit(15f);
                StopAndDestroyArrow();
                transform.parent = hitInfo.transform; // Plante la flèche dans le joueur
            }
        }
        else if (hitInfo.tag != "DeathZone" && hitInfo.tag != "Arrow")
        {
            StopAndDestroyArrow();
        }
        if (gameObject != null)
            GameObject.Destroy(gameObject, 10f);
    }

    public void StopAndDestroyArrow()
    {
        destroyed = true;
        Object.Destroy(body);
        Object.Destroy(coll);
        GameObject.Destroy(gameObject, 5f);
    }

    public void SetShootingPlayer(Player _shootingPlayer) => this.shootingPlayer = _shootingPlayer;
}
