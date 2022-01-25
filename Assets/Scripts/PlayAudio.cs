using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    [SerializeField]
    private AudioSource source;

    [SerializeField]
    private AudioClip clip;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !source.isPlaying)
        {
            print("Started");
            //source.Play();
            source.PlayOneShot(clip);
        }
    }
}