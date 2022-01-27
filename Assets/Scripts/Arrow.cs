using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private BoxCollider2D coll;
    [SerializeField] private SpriteRenderer sprite;

    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip[] missClip;

    [SerializeField] private bool selfDamage;


    private float shootingForce;
    private bool destroyed;
    private Player shootingPlayer;

    private void Start() => body.AddForce(transform.up * shootingForce, ForceMode2D.Impulse);

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
        // Si la flèche touche un environnement solide 
        if (hitInfo.tag == "Solid")
        {
            source.PlayOneShot(missClip[Random.Range(0, missClip.Length)], 0.25f);
            StopAndDestroy();
        }

        // Si la flèche touche un joueur
        else if (hitInfo.tag == "Player")
        {
            Player touchedPlayer = hitInfo.GetComponent<Player>();

            if (touchedPlayer != (shootingPlayer && !selfDamage) && !touchedPlayer.isInvicible)
            {
                StopAndDestroy();
                // Plante la flèche dans le joueur
                transform.parent = hitInfo.transform;
                touchedPlayer.Hit(shootingForce);
            }
        }

        // Si la flèche sort de la zone de jeu
        else if (hitInfo.tag == "DeathZone")
            GameObject.Destroy(gameObject, 30f);
    }

    public void StopAndDestroy()
    {
        destroyed = true;
        Object.Destroy(body);
        Object.Destroy(coll);
        GameObject.Destroy(gameObject, 5f);
    }

    public void SetShootingPlayer(Player _shootingPlayer) => this.shootingPlayer = _shootingPlayer;

    public void SetShootingForce(float _shootingForce) => this.shootingForce = _shootingForce;
}
