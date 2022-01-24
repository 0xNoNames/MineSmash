using UnityEngine;

public class Player1Spawn : MonoBehaviour
{
    private void Awake()
    {
        GameObject.FindGameObjectWithTag("Player1").transform.position = transform.position;
    }
}
