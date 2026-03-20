using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    public int maxHealth = 500;
    int currentHealth;

    public Slider bossHPBar;

    void Start()
    {
        currentHealth = maxHealth;
        bossHPBar.maxValue = maxHealth;
        bossHPBar.value = currentHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0)
            currentHealth = 0;

        bossHPBar.value = currentHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Boss Dead");
        Destroy(gameObject);
    }
}