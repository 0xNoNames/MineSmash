using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class DummyDetails : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] private GameObject currentPercentageUI;

    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip[] hitClips;

    public Vector2 startPos;

    public bool isInvicible;

    public float currentPercentage;

    private void Start()
    {
        startPos = transform.position;
        currentPercentageUI = GameObject.FindGameObjectWithTag("DummyPercentage");
    }

    //private void Update()
    //{
    //    if ("H")
    //        currentPercentage += 5;
    //    if ("K")
    //        Death();
    //}

    public void Death()
    {
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

        // Punched selon vecteur Fl�che, vecteur Joueur et pourcentage Joueur 

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

    IEnumerator DamagedAnimation()
    {
        isInvicible = true;
        animator.SetBool("isDamaged", true);
        yield return new WaitForSeconds(0.25f);
        isInvicible = false;
        animator.SetBool("isDamaged", false);
    }

    IEnumerator SpawnAnimation()
    {
        yield return new WaitForSeconds(1f);
    }


    IEnumerator RespawnAnimation()
    {
        animator.SetBool("isInvincible", false);
        animator.SetBool("isDead", true);
        isInvicible = true;

        transform.position = startPos;

        yield return new WaitForSeconds(1f);

        animator.SetBool("isDead", false);
        animator.SetBool("isInvincible", true);

        yield return new WaitForSeconds(2.5f);

        isInvicible = false;
        animator.SetBool("isInvincible", false);
    }
}