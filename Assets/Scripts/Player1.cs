using System.Collections;
using UnityEngine;

public class Player1 : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Transform playerSpawn;
    [SerializeField] private Sprite aliveSprite;
    [SerializeField] private Sprite deathSprite;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject[] currentHealthUI;
    [SerializeField] private GameObject currentPercentageUI;
    [SerializeField] public bool isInvicible;

    public int maxHealth = 3;
    public int currentHealth;
    public float currentPercentage;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Hit(5f);
        }
    }

    public void Death()
    {
        HealthDown();
        GetComponent<Player1>().transform.position = playerSpawn.position;
        StartCoroutine(Respawn());
    }

    public void Hit(float damage)
    {
        if (isInvicible)
            return;
        else if (currentPercentage < 999.9f)
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
            {
                currentHealthUI[i].SetActive(true);
            }
            else
            {
                currentHealthUI[i].SetActive(false);
            }
        }
    }

    IEnumerator Respawn()
    {
        playerMovement.SetDesactivateState(true);
        animator.SetBool("isInvincible", false);
        isInvicible = true;

        spriteRenderer.sprite = deathSprite;

        for (float i = 0; i <= 2; i += Time.deltaTime)
        {
            spriteRenderer.color = new Color(1, 1, 1, i / 2);
            yield return null;
        }

        spriteRenderer.color = new Color(1, 1, 1, 1);

        spriteRenderer.sprite = aliveSprite;

        animator.SetBool("isInvincible", true);
        playerMovement.SetDesactivateState(false);

        yield return new WaitForSeconds(2.5f);

        isInvicible = false;
        animator.SetBool("isInvincible", false);
    }

}
