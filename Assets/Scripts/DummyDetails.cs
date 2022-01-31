using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class DummyDetails : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private Animator animator;
    [SerializeField] private BumpSystem bumpSystem;

    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip[] hitClips;

    [SerializeField] private float maxPercentage = 999.9f;

    public int dummyID;
    public string dummyName;
    public Vector2 dummySpawn;
    public float currentPercentage;

    public bool isInvicible;

    private void Start()
    {
        ToSpawn();
    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(bumpSystem.value.y) > 0)
            rigidBody.velocity = new Vector2(bumpSystem.value.x, bumpSystem.value.y);
        else
            rigidBody.velocity = new Vector2(bumpSystem.value.x, rigidBody.velocity.y);
    }

    public void ToSpawn()
    {
        transform.position = dummySpawn;
    }

    public void Initialize(int _dummyID, Vector2 _dummySpawn)
    {
        dummyID = _dummyID;
        dummyName = "Dummy " + (dummyID + 1);
        dummySpawn = _dummySpawn;
    }

    public void RemoveArrows()
    {
        // Suppression des flèches plantées dans le joueur
        for (int i = 1; i < transform.childCount; i++)
            GameObject.Destroy(transform.GetChild(i).gameObject);
    }

    public void Death()
    {
        RemoveArrows();

        transform.position = dummySpawn;
        rigidBody.velocity = Vector2.zero;

        currentPercentage = 0f;
        UIManager.Instance.GetDummyUI(dummyID).SetPercentage(0f);

        // Réinitialisation de la variable de bump
        bumpSystem.value = Vector2.zero;
    }

    public void Hit(Vector2 arrowVelocity)
    {
        if (isInvicible)
            return;

        source.PlayOneShot(hitClips[Random.Range(0, hitClips.Length)], 0.25f);

        StopCoroutine("DamagedAnimation");
        StartCoroutine("DamagedAnimation");

        // Ajout du bump de la flèche au joueur
        Vector2 arrowDamage = arrowVelocity * (currentPercentage / 100);

        bumpSystem.value = arrowDamage;

        // Etourdi le joueur pendant x secondes selon son pourcentage et les dégâts de la flèche
        //StopCoroutine("DamagedStun");
        //StartCoroutine(DamagedStun(arrowDamage.magnitude / 500));

        currentPercentage += arrowVelocity.magnitude * 0.37f;

        if (currentPercentage > maxPercentage)
            currentPercentage = maxPercentage;

        UIManager.Instance.GetDummyUI(dummyID).SetPercentage(currentPercentage);
    }

    private IEnumerator DamagedAnimation()
    {
        isInvicible = true;
        animator.SetBool("isDamaged", true);
        yield return new WaitForSeconds(0.25f);
        isInvicible = false;
        animator.SetBool("isDamaged", false);
    }
}