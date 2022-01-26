using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private Transform bow;
    [SerializeField] private GameObject arrowPrefab;

    [SerializeField] private Player player;
    [SerializeField] private PlayerMovement playerMovement;

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !playerMovement.isDesactivated)
            Shoot();
    }

    void Shoot()
    {
        GameObject arrow = Instantiate(arrowPrefab, bow.position, bow.rotation);
        arrow.GetComponent<Arrow>().SetShootingPlayer(player);
    }
}
