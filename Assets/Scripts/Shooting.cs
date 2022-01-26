using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform bow;
    public GameObject arrowPrefab;

    public float arrowForce = 20f;


    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject arrow = Instantiate(arrowPrefab, bow.position, bow.rotation);

        arrow.GetComponent<Rigidbody2D>().velocity = bow.up * arrowForce;
        //new Vector3(bow.up.x * 2f, bow.up.y * 5f, bow.up.z)
        //Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        //rb.AddForce(bow.up * arrowForce, ForceMode2D.Impulse);
    }
}
