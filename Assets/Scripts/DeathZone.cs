using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player1"))
            collision.GetComponent<Player1>().Death();
        //else if (collision.CompareTag("Player2"))
        //    collision.GetComponent<Player2>().Death();
    }
}
