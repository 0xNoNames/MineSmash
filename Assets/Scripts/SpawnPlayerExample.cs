using UnityEngine;

public class SpawnPlayerExample : MonoBehaviour
{
    private void Awake() => GameObject.FindGameObjectWithTag("Player").transform.position = transform.position;
}
