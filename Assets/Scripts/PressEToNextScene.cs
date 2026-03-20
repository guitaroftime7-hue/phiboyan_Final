using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PressEToNextScene : MonoBehaviour
{
    [Header("Fade UI")]
    public CanvasGroup fadePanel;
    public float fadeDuration = 0.8f;

    [Header("Input")]
    public KeyCode nextKey = KeyCode.E;

    bool isLoading = false;

    void Start()
    {
        // เริ่มต้น: จอไม่ดำ
        if (fadePanel != null)
        {
            fadePanel.alpha = 0f;
            fadePanel.blocksRaycasts = false;
        }
    }

    void Update()
    {
        if (isLoading) return;

        if (Input.GetKeyDown(nextKey))
        {
            StartCoroutine(FadeAndLoadNext());
        }
    }

    IEnumerator FadeAndLoadNext()
    {
        isLoading = true;

        if (fadePanel != null)
        {
            fadePanel.blocksRaycasts = true;

            float t = 0f;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                fadePanel.alpha = Mathf.Clamp01(t / fadeDuration);
                yield return null;
            }
            fadePanel.alpha = 1f;
        }

        int current = SceneManager.GetActiveScene().buildIndex;
        int next = current + 1;

        if (next >= SceneManager.sceneCountInBuildSettings)
            next = 0; // ถ้าไม่มีซีนถัดไป ให้กลับไปซีนแรก (แก้ได้ตามใจ)

        SceneManager.LoadScene(next);
    }
}
