using UnityEngine;
using System.Collections;

public class PlayerDash : MonoBehaviour
{
    public float dashSpeed = 20f;
    public float dashTime = 0.2f;
    public float dashCooldown = 5f;

    public AudioSource dashSound;

    // 🔥 EFFECTS
    public ParticleSystem dashStartEffect; // ระเบิดตอนเริ่ม
    public ParticleSystem dashTrailEffect; // ควันลากตอนพุ่ง
    public ParticleSystem dashEndEffect;   // ตอนหยุด

    float lastDash;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            TryDash();
        }
    }

    void TryDash()
    {
        if (Time.time - lastDash < dashCooldown) return;

        StartCoroutine(Dash());

        if (dashSound != null)
            dashSound.Play();

        // 💥 เริ่ม Dash
        if (dashStartEffect != null)
        {
            dashStartEffect.Stop();
            dashStartEffect.Play();
        }

        // 💨 เปิด Trail
        if (dashTrailEffect != null)
        {
            dashTrailEffect.Play();
        }

        lastDash = Time.time;
    }

    IEnumerator Dash()
    {
        float startTime = Time.time;

        while (Time.time < startTime + dashTime)
        {
            transform.position += transform.forward * dashSpeed * Time.deltaTime;
            yield return null;
        }

        // ❌ ปิด Trail ตอนจบ
        if (dashTrailEffect != null)
        {
            dashTrailEffect.Stop();
        }

        // 💥 เอฟเฟกต์ตอนจบ
        if (dashEndEffect != null)
        {
            dashEndEffect.Stop();
            dashEndEffect.Play();
        }
    }
}