using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private BoxCollider2D coll;
    [SerializeField] private SpriteRenderer sprite;

    private bool destroyed;
    private Player player;

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
            player = hitInfo.GetComponent<Player>();

            Debug.Log(player);

            if (player != null) //&& player != arrow.player)
            {
                player.Hit(15f);
                transform.parent = hitInfo.transform;
            }
        }

        if (hitInfo.tag != "DeathZone" && hitInfo.tag != "Arrow")
        {
            destroyed = true;
            Object.Destroy(body);
            Object.Destroy(coll);
            StartCoroutine(DestroyArrow());
        }
    }

    IEnumerator DestroyArrow()
    {
        yield return new WaitForSeconds(5f);
        Object.Destroy(gameObject);
    }
}
