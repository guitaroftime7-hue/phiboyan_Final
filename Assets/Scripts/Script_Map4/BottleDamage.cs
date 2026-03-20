using UnityEngine;

public class BottleDamage : MonoBehaviour
{
    public int damage = 20;

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit Object: " + collision.gameObject.name);

        // เช็ค Boss
        BossHealth boss = collision.gameObject.GetComponentInParent<BossHealth>();
        if (boss != null)
        {
            Debug.Log("Boss Hit!");
            boss.TakeDamage(damage);
        }

        // เช็คผีเด็ก
        BabyGhostAI ghost = collision.gameObject.GetComponentInParent<BabyGhostAI>();
        if (ghost != null)
        {
            Debug.Log("Baby Ghost Hit!");
            ghost.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}