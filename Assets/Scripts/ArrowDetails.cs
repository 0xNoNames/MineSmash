using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowDetails : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private BoxCollider2D coll;
    [SerializeField] private SpriteRenderer sprite;

    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip[] missClip;

    private float shootingForce;
    private bool destroyed;
    private PlayerDetails shootingPlayer;

    private void Start()
    {
        rigidBody.AddForce(transform.up * shootingForce, ForceMode2D.Impulse);
    }

    private void FixedUpdate()
    {
        if (!destroyed)
        {
            float angle = Mathf.Atan2(rigidBody.velocity.x, rigidBody.velocity.y) * -Mathf.Rad2Deg;
            sprite.transform.eulerAngles = new Vector3(0, 0, angle);
        }
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // Si la fl�che touche un environnement solide 
        if (hitInfo.tag == "Solid")
        {
            source.PlayOneShot(missClip[Random.Range(0, missClip.Length)], 0.25f);
            StopAndDestroy();
        }

        // Si la fl�che touche un joueur
        else if (hitInfo.tag == "Player")
        {
            PlayerDetails touchedPlayer = hitInfo.GetComponent<PlayerDetails>();

            if (touchedPlayer != shootingPlayer && !touchedPlayer.isInvicible)
            {
                StopAndDestroy();
                // Plante la fl�che dans le joueur
                transform.parent = hitInfo.transform;
                touchedPlayer.Hit(rigidBody.velocity);
            }
        }

        // Si la fl�che touche un mannequin
        else if (hitInfo.tag == "Dummy")
        {
            DummyDetails touchedDummy = hitInfo.GetComponent<DummyDetails>();

            if (!touchedDummy.isInvicible)
            {
                StopAndDestroy();

                // Plante la fl�che dans le dummy
                transform.parent = hitInfo.transform;
                touchedDummy.Hit(rigidBody.velocity);
            }
        }
    }

    public void Destroy()
    {
        GameObject.Destroy(gameObject, 30f);
    }

    public void StopAndDestroy()
    {
        destroyed = true;
        Object.Destroy(rigidBody);
        Object.Destroy(coll);
        GameObject.Destroy(gameObject, 5f);
    }

    public void SetShootingPlayer(PlayerDetails _shootingPlayer)
    {
        this.shootingPlayer = _shootingPlayer;
    }

    public void SetShootingForce(float _shootingForce)
    {
        this.shootingForce = _shootingForce;
    }
}
