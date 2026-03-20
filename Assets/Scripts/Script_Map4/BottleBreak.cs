using UnityEngine;

public class BottleBreak : MonoBehaviour
{
    public GameObject breakEffect;
    public AudioClip breakSound;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || 
            collision.gameObject.CompareTag("Ghost"))
        {
            Vector3 pos = transform.position;

            // เล่นเสียงแก้วแตก
            if (breakSound != null)
                AudioSource.PlayClipAtPoint(breakSound, pos);

            // effect แตก
            if (breakEffect != null)
                Instantiate(breakEffect, pos, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}