using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private Transform bow;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private float bowForce = 20f;

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !GetComponent<PlayerMovement>().isDesactivated)
            Shoot();
    }

    void Shoot()
    {
        GameObject arrow = Instantiate(arrowPrefab, bow.position, bow.rotation);
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        rb.AddForce(bow.up * bowForce, ForceMode2D.Impulse);
    }
}
