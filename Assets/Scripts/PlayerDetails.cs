using System.Collections;
using UnityEngine;

public class PlayerDetails : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Animator animator;

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

    public void Initialize(int _playerID, Vector2 _playerSpawn)
    {
        playerID = _playerID;
        playerSpawn = _playerSpawn;
    }

    public void Death()
    {
        StopCoroutine("RespawnAnimation");
        StartCoroutine("RespawnAnimation");

        currentHealth -= 1;
        currentPercentage = 0f;

        UIManager.Instance.getPlayerUI(playerID - 1).prct.text = currentPercentage.ToString("0.0") + "%";
        UIManager.Instance.getPlayerUI(playerID - 1).prct.color = new Color(1, 1, 1);

        // Suppression des fl�ches plant�s dans le joueur
        for (int i = 1; i < transform.childCount; i++)
            GameObject.Destroy(transform.GetChild(i).gameObject);

        //for (int i = 0; i < currentHealthUI.Length; i++)
        //{
        //    if (currentHealth > i)
        //        currentHealthUI[i].SetActive(true);
        //    else
        //        currentHealthUI[i].SetActive(false);
        //}
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

        rigidBody.AddForce(arrowVelocity * currentPercentage);

        currentPercentage += arrowVelocity.magnitude * 0.37f;

        if (currentPercentage > 999.9f)
            currentPercentage = 999.9f;

        float delta = currentPercentage / 150;
        UIManager.Instance.getPlayerUI(playerID - 1).prct.text = currentPercentage.ToString("0.0") + "%";
        UIManager.Instance.getPlayerUI(playerID - 1).prct.color = new Color(1, 1 - delta, 1 - delta);
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

    //private IEnumerator SpawnAnimation()
    //{
    //    yield return new WaitForSeconds(1f);
    //}

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