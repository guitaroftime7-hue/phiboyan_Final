using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PhoneCallInteract : MonoBehaviour
{
    [Header("Player")]
    public string playerTag = "Player";
    public MonoBehaviour movementScript;

    [Header("UI")]
    public GameObject promptRoot;
    public TMP_Text promptText;
    public GameObject dialoguePanel;
    public TMP_Text nameText;
    public TMP_Text lineText;

    [Header("Fade To Next Scene")]
    public Image fadeImage;                 // ลาก FadeImage (UI Image) มาใส่
    public float fadeDuration = 1f;
    public string nextSceneName = "NextScene"; // ใส่ชื่อฉากถัดไป (ตรงกับใน Build Settings)

    [Header("Audio")]
    public AudioSource ringSource;
    public AudioSource voiceSource;

    [Header("Call Data")]
    public string callerName = "เสียงในสาย";
    public bool oneTimeOnly = true;

    [Header("Locked Text")]
    public string lockedMessage = "ยังรับไม่ได้…ต้องคุยกับคนในร้านก่อน";

    [Header("Typewriter")]
    public float typeSpeed = 0.03f;

    [System.Serializable]
    public class SubtitleLine
    {
        [TextArea(2, 4)] public string text;
        public AudioClip voiceClip;
    }

    public SubtitleLine[] subtitles;

    bool playerInRange;
    bool inCall;
    bool callDone;

    int currentIndex;
    bool isTyping;

    Coroutine typingCo;
    bool isTransitioning;

    void Start()
    {
        if (promptRoot) promptRoot.SetActive(false);
        if (dialoguePanel) dialoguePanel.SetActive(false);

        // เริ่มด้วยเฟดโปร่งใส
        if (fadeImage != null)
        {
            var c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
            fadeImage.gameObject.SetActive(true);
        }
    }

    void Update()
    {
        if (!playerInRange) return;
        if (isTransitioning) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (oneTimeOnly && callDone) return;

            if (!GameState.phoneUnlocked)
            {
                if (promptRoot) promptRoot.SetActive(true);
                if (promptText) promptText.text = lockedMessage;
                return;
            }

            if (!inCall)
            {
                AnswerCall();
            }
            else
            {
                if (isTyping) ShowFullCurrentLine();
                else NextLine();
            }
        }
    }

    void AnswerCall()
    {
        if (subtitles == null || subtitles.Length == 0) return;

        inCall = true;
        currentIndex = 0;

        if (promptRoot) promptRoot.SetActive(false);

        // หยุดเสียงริง
        if (ringSource)
        {
            ringSource.Stop();
            ringSource.time = 0f;
        }

        // เปิด UI
        if (dialoguePanel) dialoguePanel.SetActive(true);
        if (nameText) nameText.text = callerName;
        if (lineText) lineText.text = "";

        // ล็อกเดิน
        if (movementScript) movementScript.enabled = false;

        PlayCurrentLine();
    }

    void PlayCurrentLine()
    {
        if (currentIndex < 0 || currentIndex >= subtitles.Length)
        {
            EndCallAndGoNextScene();
            return;
        }

        // เล่นเสียงพูด
        if (voiceSource)
        {
            voiceSource.Stop();
            voiceSource.clip = subtitles[currentIndex].voiceClip;
            if (subtitles[currentIndex].voiceClip != null)
                voiceSource.Play();
        }

        StartTyping(subtitles[currentIndex].text);
    }

    void NextLine()
    {
        currentIndex++;

        if (currentIndex >= subtitles.Length)
        {
            EndCallAndGoNextScene();
            return;
        }

        PlayCurrentLine();
    }

    void EndCallAndGoNextScene()
    {
        StopTyping();

        inCall = false;
        callDone = true;

        if (voiceSource) voiceSource.Stop();

        // ปิด UI โทรศัพท์
        if (dialoguePanel) dialoguePanel.SetActive(false);
        if (promptRoot) promptRoot.SetActive(false);

        // ✅ เฟดแล้วไปฉากถัดไป
        StartCoroutine(FadeAndLoad());
    }

    IEnumerator FadeAndLoad()
    {
        isTransitioning = true;

        // กันขยับระหว่างเฟด
        if (movementScript) movementScript.enabled = false;

        if (fadeImage == null)
        {
            // ถ้าไม่ได้ใส่ FadeImage ก็โหลดฉากเลย
            SceneManager.LoadScene(nextSceneName);
            yield break;
        }

        float t = 0f;
        Color c = fadeImage.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float a = Mathf.Clamp01(t / fadeDuration);
            c.a = a;
            fadeImage.color = c;
            yield return null;
        }

        c.a = 1f;
        fadeImage.color = c;

        SceneManager.LoadScene(nextSceneName);
    }

    // ---------- Typewriter ----------
    void StartTyping(string fullText)
    {
        StopTyping();
        typingCo = StartCoroutine(TypeLine(fullText));
    }

    IEnumerator TypeLine(string fullText)
    {
        isTyping = true;
        if (lineText) lineText.text = "";

        foreach (char ch in fullText)
        {
            if (lineText) lineText.text += ch;
            yield return new WaitForSeconds(typeSpeed);
        }

        isTyping = false;
        typingCo = null;
    }

    void ShowFullCurrentLine()
    {
        StopTyping();
        if (lineText) lineText.text = subtitles[currentIndex].text;
    }

    void StopTyping()
    {
        if (typingCo != null)
        {
            StopCoroutine(typingCo);
            typingCo = null;
        }
        isTyping = false;
    }

    // ---------- Trigger ----------
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        playerInRange = true;

        if (oneTimeOnly && callDone)
        {
            if (promptRoot) promptRoot.SetActive(false);
            return;
        }

        if (promptRoot) promptRoot.SetActive(true);

        if (!GameState.phoneUnlocked)
        {
            if (promptText) promptText.text = lockedMessage;
        }
        else
        {
            if (promptText) promptText.text = "กด E เพื่อรับสาย";
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        playerInRange = false;
        if (promptRoot) promptRoot.SetActive(false);
    }
}
