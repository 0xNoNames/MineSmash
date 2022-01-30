using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDetails : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Animator animator;

    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip[] hitClips;

    public int playerID;
    public string playerName;
    public Vector2 playerSpawn;
    public int wins;
    public int maxHealth = 3;
    public int currentHealth;
    public float currentPercentage;

    public bool isInvicible;

    private void Start()
    {
        Spawn();
    }

    public void Spawn()
    {
        transform.position = playerSpawn;
        currentHealth = maxHealth;
    }

    public void Initialize(int _playerID, Vector2 _playerSpawn)
    {
        playerID = _playerID;
        playerSpawn = _playerSpawn;
        playerName = "Joueur " + (_playerID + 1);
    }

    public void Death()
    {
        // Suppression des flèches plantées dans le joueur
        for (int i = 1; i < transform.childCount; i++)
            GameObject.Destroy(transform.GetChild(i).gameObject);

        StopCoroutine("RespawnAnimation");
        StartCoroutine("RespawnAnimation");

        currentHealth -= 1;
        currentPercentage = 0f;

        UIManager.Instance.getPlayerUI(playerID).SetPercentage(currentPercentage);

        UIManager.Instance.getPlayerUI(playerID).SetHealth(currentHealth);

        if (currentHealth == 0)
        {
            if (GameManager.Instance.playerList.Count > 1)
            {
                if (playerID == 0)
                    GameManager.Instance.getPlayerDetails(1).wins += 1;
                else
                    GameManager.Instance.getPlayerDetails(0).wins += 1;
                GameManager.Instance.GameOver();

            }
            else
                GameManager.Instance.RestartGame();
        }
    }

    public void Hit(Vector2 arrowVelocity)
    {
        if (isInvicible)
            return;

        source.PlayOneShot(hitClips[Random.Range(0, hitClips.Length)], 0.25f);

        StopCoroutine("DamagedAnimation");
        StartCoroutine("DamagedAnimation");

        // Etourdi le joueur pendant x secondes selon son pourcentage
        StopCoroutine("DamagedStun");
        StartCoroutine(DamagedStun(currentPercentage / 500));


        // Ajout du bump de la flèche au joueur
        playerController.bump = arrowVelocity;

        //rigidBody.AddForce(arrowVelocity * currentPercentage);

        currentPercentage += arrowVelocity.magnitude * 0.37f;

        if (currentPercentage > 999.9f)
            currentPercentage = 999.9f;

        UIManager.Instance.getPlayerUI(playerID).SetPercentage(currentPercentage);
    }

    private IEnumerator DamagedStun(float seconds)
    {
        playerController.SetDesactivateState(true, false);
        yield return new WaitForSeconds(seconds);
        playerController.SetDesactivateState(false, false);
    }

    private IEnumerator DamagedAnimation()
    {
        isInvicible = true;
        animator.SetBool("isDamaged", true);
        yield return new WaitForSeconds(0.25f);
        isInvicible = false;
        animator.SetBool("isDamaged", false);
    }

    private IEnumerator RespawnAnimation()
    {
        playerController.SetDesactivateState(true, true);

        animator.SetBool("isInvincible", false);
        animator.SetBool("isDead", true);
        isInvicible = true;

        transform.position = playerSpawn;

        yield return new WaitForSeconds(1f);

        animator.SetBool("isDead", false);
        animator.SetBool("isInvincible", true);

        playerController.SetDesactivateState(false, false);

        yield return new WaitForSeconds(2.5f);

        isInvicible = false;
        animator.SetBool("isInvincible", false);
    }
}