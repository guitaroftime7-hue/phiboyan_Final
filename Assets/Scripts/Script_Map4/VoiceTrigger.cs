using UnityEngine;

public class VoiceTrigger : MonoBehaviour
{
    public AudioSource voiceAudio;
    private bool hasPlayed = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasPlayed)
        {
            hasPlayed = true;
            voiceAudio.Play();
        }
    }
}