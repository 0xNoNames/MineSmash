using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            collision.GetComponent<PlayerDetails>().Death();
        else if (collision.CompareTag("Arrow"))
            collision.GetComponent<ArrowDetails>().Destroy();
    }
}
