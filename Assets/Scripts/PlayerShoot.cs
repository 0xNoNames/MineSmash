using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private Transform bow;
    [SerializeField] private Animator bowAnimator;

    [SerializeField] private GameObject arrowPrefab;

    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip shotSound;

    [SerializeField] private PlayerDetails player;
    [SerializeField] private PlayerController playerController;

    [SerializeField] private float fireRate = 0.5f;

    private float lastFireTime = 0f;
    private float fireChargeMin = 5f;
    private float fireChargeMax = 55f;
    private float fireCharge;

    private bool isCharging;

    private void Start()
    {
        bowAnimator = bow.GetComponent<Animator>();
    }

    private void Update()
    {
        Charge();
    }

    public void Fire(InputAction.CallbackContext keyPress)
    {
        if (keyPress.started && !playerController.GetDesactivateState())
        {
            bowAnimator.SetBool("isBending", true);
            isCharging = true;
        }

        if (keyPress.canceled && Time.time - lastFireTime > fireRate && !playerController.GetDesactivateState())
        {
            Shoot();
            isCharging = false;
            bowAnimator.SetBool("isBending", false);
            lastFireTime = Time.time;
        }
        else if (keyPress.canceled)
        {
            isCharging = false;
            bowAnimator.SetBool("isBending", false);
        }
    }

    void Charge()
    {
        if (isCharging)
        {
            if (fireCharge < fireChargeMax)
                fireCharge += Time.deltaTime * 33.3f;
        }
        else
            fireCharge = fireChargeMin;

    }

    void Shoot()
    {
        source.PlayOneShot(shotSound, 0.25f);

        GameObject arrow = Instantiate(arrowPrefab, bow.position, bow.rotation);
        ArrowDetails arrowComponent = arrow.GetComponent<ArrowDetails>();
        arrowComponent.SetShootingPlayer(player);
        arrowComponent.SetShootingForce(fireCharge);
    }
}