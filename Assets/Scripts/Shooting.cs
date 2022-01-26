using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private Transform bow;
    [SerializeField] private GameObject arrowPrefab;

    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip shotClip;

    [SerializeField] private Player player;
    [SerializeField] private PlayerMovement playerMovement;

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !playerMovement.isDesactivated)
            Shoot();
    }

    void Shoot()
    {
        source.PlayOneShot(shotClip, 0.25f);

        GameObject arrow = Instantiate(arrowPrefab, bow.position, bow.rotation);
        arrow.GetComponent<Arrow>().SetShootingPlayer(player);
    }
}
