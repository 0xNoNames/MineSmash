using System.Collections;
using UnityEngine;

public class PlayerDetails : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Animator animator;

    [SerializeField] private GameObject[] currentHealthUI;
    [SerializeField] private GameObject currentPercentageUI;

    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip[] hitClips;

    public int playerID;
    public Vector2 playerSpawn;

    public bool isInvicible;

    public int maxHealth = 3;
    public int currentHealth;
    public float currentPercentage;

    private void Start()
    {
        transform.position = playerSpawn;
        currentHealth = maxHealth;
    }

    public void Death()
    {
        HealthDown();
        StopCoroutine("RespawnAnimation");
        StartCoroutine("RespawnAnimation");
    }

    public void Hit(float damage)
    {
        if (isInvicible)
            return;

        source.PlayOneShot(hitClips[Random.Range(0, hitClips.Length)], 0.25f);

        StopCoroutine("DamagedAnimation");
        StartCoroutine("DamagedAnimation");

        StopCoroutine("DamagedStun");
        StartCoroutine(DamagedStun(currentPercentage / 500));

        // Punched selon vecteur Flèche, vecteur Joueur et pourcentage Joueur 

        if (currentPercentage < 999.9f)
        {
            currentPercentage += damage * 0.37f;
            currentPercentageUI.GetComponent<UnityEngine.UI.Text>().text = currentPercentage.ToString("0.0") + "%";
            float delta = currentPercentage / 150;
            currentPercentageUI.GetComponent<UnityEngine.UI.Text>().color = new Color(1, 1 - delta, 1 - delta);
        }
        else
        {
            currentPercentage = 999.9f;
            currentPercentageUI.GetComponent<UnityEngine.UI.Text>().text = currentPercentage.ToString("0.0") + "%";
        }
    }

    public void HealthDown()
    {
        currentHealth -= 1;
        currentPercentage = 0f;
        currentPercentageUI.GetComponent<UnityEngine.UI.Text>().text = currentPercentage.ToString("0.0") + "%";
        currentPercentageUI.GetComponent<UnityEngine.UI.Text>().color = new Color(1, 1, 1);

        // Suppression des flèches plantés dans le joueur
        for (int i = 1; i < transform.childCount; i++)
            GameObject.Destroy(transform.GetChild(i).gameObject);

        for (int i = 0; i < currentHealthUI.Length; i++)
        {
            if (currentHealth > i)
                currentHealthUI[i].SetActive(true);
            else
                currentHealthUI[i].SetActive(false);
        }
    }

    public void Initialize(int _playerID, Vector2 _playerSpawn)
    {
        playerID = _playerID;
        playerSpawn = _playerSpawn;
    }

    private IEnumerator DamagedStun(float seconds)
    {
        playerController.SetDesactivateStateHit(true);
        yield return new WaitForSeconds(seconds);
        playerController.SetDesactivateStateHit(false);
    }

    private IEnumerator DamagedAnimation()
    {
        isInvicible = true;
        animator.SetBool("isDamaged", true);
        yield return new WaitForSeconds(0.25f);
        isInvicible = false;
        animator.SetBool("isDamaged", false);
    }

    private IEnumerator SpawnAnimation()
    {
        yield return new WaitForSeconds(1f);
    }


    private IEnumerator RespawnAnimation()
    {
        playerController.SetDesactivateStateDeath(true);

        animator.SetBool("isInvincible", false);
        animator.SetBool("isDead", true);
        isInvicible = true;

        transform.position = playerSpawn;

        yield return new WaitForSeconds(1f);

        animator.SetBool("isDead", false);
        animator.SetBool("isInvincible", true);

        playerController.SetDesactivateStateDeath(false);

        yield return new WaitForSeconds(2.5f);

        isInvicible = false;
        animator.SetBool("isInvincible", false);
    }
}
