using UnityEngine;

public class BabyGhostAI : MonoBehaviour
{
    public Transform player;
    public float speed = 3f;
    public int health = 20;

    public float attackDistance = 1.5f;
    public int damage = 10;

    public float attackCooldown = 1.5f;
    float lastAttackTime;

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // วิ่งเข้าหาผู้เล่น
        transform.LookAt(player);
        transform.position = Vector3.MoveTowards(
            transform.position,
            player.position,
            speed * Time.deltaTime
        );

        // ถ้าเข้าใกล้ → โจมตี
        if (distance < attackDistance)
        {
            AttackPlayer();
        }
    }

    void AttackPlayer()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            PlayerHealth ph = player.GetComponent<PlayerHealth>();

            if (ph != null)
            {
                ph.TakeDamage(damage);
                Debug.Log("Ghost attack player!");
            }

            lastAttackTime = Time.time;
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}