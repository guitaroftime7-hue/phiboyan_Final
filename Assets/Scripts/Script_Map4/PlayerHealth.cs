using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    int currentHealth;

    public TextMeshProUGUI hpText;

    public Image damageFlash;
    public Image fadeImage;

    public float fadeSpeed = 1.5f;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHP();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0)
            currentHealth = 0;

        UpdateHP();

        StartCoroutine(FlashScreen());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        UpdateHP();
    }

    void UpdateHP()
    {
        hpText.text = "HP : " + currentHealth;
    }

    IEnumerator FlashScreen()
    {
        damageFlash.color = new Color(1,0,0,0.2f);

        yield return new WaitForSeconds(0.2f);

        damageFlash.color = new Color(1,0,0,0f);
    }

    void Die()
    {
        StartCoroutine(FadeAndLoad());
    }

    IEnumerator FadeAndLoad()
    {
        float alpha = 0;

        while(alpha < 1)
        {
            alpha += Time.deltaTime * fadeSpeed;
            fadeImage.color = new Color(0,0,0,alpha);
            yield return null;
        }

        SceneManager.LoadScene(5);
    }
}