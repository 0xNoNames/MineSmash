using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Transform playerSpawn;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject[] currentHealthUI;
    [SerializeField] private GameObject currentPercentageUI;
    [SerializeField] public bool isInvicible;

    public int maxHealth = 3;
    public int currentHealth;
    public float currentPercentage;

    private void Start() => currentHealth = maxHealth;

    /*
     *  DEBUG
     */
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
            Hit(5f);
        if (Input.GetKeyDown(KeyCode.K))
            Death();
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

        StopCoroutine("DamagedAnimation");
        StartCoroutine("DamagedAnimation");

        StopCoroutine("DamagedStun");
        StartCoroutine(DamagedStun(currentPercentage/500));

        // Punched selon vecteur Flèche, vecteur Joueur et pourcentage Joueur 


        if (currentPercentage < 999.9f)
        {
            currentPercentage += damage;
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

        for (int i = 0; i < currentHealthUI.Length; i++)
        {
            if (currentHealth > i)
                currentHealthUI[i].SetActive(true);
            else
                currentHealthUI[i].SetActive(false);
        }
    }

    IEnumerator DamagedStun(float seconds)
    {
        playerMovement.SetDesactivateState(true);
        yield return new WaitForSeconds(seconds);
        playerMovement.SetDesactivateState(false);
    }

    IEnumerator DamagedAnimation()
    {
        isInvicible = true;
        animator.SetBool("isDamaged", true);
        yield return new WaitForSeconds(0.25f);
        isInvicible = false;
        animator.SetBool("isDamaged", false);
    }

    IEnumerator RespawnAnimation()
    {
        playerMovement.SetDesactivateState(true);
        animator.SetBool("isInvincible", false);
        animator.SetBool("isDead", true);
        isInvicible = true;

        yield return new WaitForSeconds(0.1f);

        transform.position = playerSpawn.position;

        yield return new WaitForSeconds(0.9f);

        animator.SetBool("isDead", false);
        animator.SetBool("isInvincible", true);
        playerMovement.SetDesactivateState(false);

        yield return new WaitForSeconds(2.5f);

        isInvicible = false;
        animator.SetBool("isInvincible", false);
    }
}
