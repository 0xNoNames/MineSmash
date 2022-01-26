using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private Transform bow;
    [SerializeField] private GameObject arrowPrefab;

    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip shotClip;

    [SerializeField] private Player player;
    [SerializeField] private PlayerMovement playerMovement;

    [SerializeField] private Animator bowAnimator;

    private float lastFireTime = 0f;
    private float fireRate = 0.5f;
    private float fireCharge = 5f;

    private void Start() => bowAnimator = bow.GetComponent<Animator>();

    void Update()
    {
        if (Input.GetButton("Fire1") && !playerMovement.isDesactivated)
        {
            bowAnimator.SetBool("isBending", true);
            if (fireCharge < 55f)
                fireCharge += Time.deltaTime * 33.3f;
        }

        if (Input.GetButtonUp("Fire1") && Time.time - lastFireTime > fireRate && !playerMovement.isDesactivated)
        {
            Shoot();
            lastFireTime = Time.time;
        }

        if (Input.GetButtonUp("Fire1"))
        {
            fireCharge = 5f;
            bowAnimator.SetBool("isBending", false);
        }
    }

    void Shoot()
    {
        source.PlayOneShot(shotClip, 0.25f);

        GameObject arrow = Instantiate(arrowPrefab, bow.position, bow.rotation);
        Arrow arrowComponent = arrow.GetComponent<Arrow>();
        arrowComponent.SetShootingPlayer(player);
        arrowComponent.SetShootingForce(fireCharge);
    }
}
