using System.Collections;
using UnityEngine;
using TMPro;

public class TypewriterText : MonoBehaviour
{
    public TMP_Text textUI;

    [TextArea(3, 10)]
    public string fullText;

    public float charPerSecond = 35f;
    public float startDelay = 0.2f;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip typingSound;
    public bool skipSpaceSound = true;
    public float soundCooldown = 0.05f;

    float nextSoundTime = 0f;
    Coroutine running;

    void OnEnable()
    {
        // กันเริ่มซ้ำจากการ Enable/Disable
        if (running != null) StopCoroutine(running);
        running = StartCoroutine(TypeText());
    }

    void OnDisable()
    {
        if (running != null) StopCoroutine(running);
        running = null;
    }

    IEnumerator TypeText()
    {
        if (textUI != null) textUI.text = "";
        yield return new WaitForSeconds(startDelay);

        string text = string.IsNullOrEmpty(fullText)
            ? (textUI != null ? textUI.text : "")
            : fullText;

        if (textUI != null) textUI.text = "";

        float delay = 1f / Mathf.Max(1f, charPerSecond);

        foreach (char c in text)
        {
            if (textUI != null) textUI.text += c;

            if (audioSource != null && typingSound != null)
            {
                bool isSpace = (c == ' ' || c == '\t' || c == '\n' || c == '\r');
                if (!skipSpaceSound || !isSpace)
                {
                    if (Time.time >= nextSoundTime)
                    {
                        // ไม่ให้ซ้อน: ตัดเสียงเก่าก่อนเล่นใหม่
                        audioSource.Stop();
                        audioSource.clip = typingSound;
                        audioSource.Play();

                        nextSoundTime = Time.time + Mathf.Max(0f, soundCooldown);
                    }
                }
            }

            yield return new WaitForSeconds(delay);
        }
    }
}
