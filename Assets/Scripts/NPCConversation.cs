using UnityEngine;
using TMPro;
using System.Collections;

public class NPCConversation : MonoBehaviour
{
    [Header("One-time talk")]
    public bool oneTimeOnly = true;
    private bool talkedOnce;

    [Header("Dialogue Data")]
    public string npcName = "NPC";
    [TextArea(2, 5)] public string[] lines;

    [Header("NPC Voice")]
    public AudioSource npcVoiceSource;
    public AudioClip[] voiceClips;

    [Header("UI")]
    public GameObject promptRoot;
    public TMP_Text promptText;
    public GameObject dialoguePanel;
    public TMP_Text nameText;
    public TMP_Text lineText;

    [Header("Stop movement while talking")]
    public MonoBehaviour movementScript;

    [Header("Typewriter")]
    public float typeSpeed = 0.03f;

    [Header("Typewriter SFX")]
    public AudioSource typeAudioSource;
    public AudioClip typeClip;
    public float typeVolume = 0.6f;
    public int playTypeSoundEveryNChars = 2;

    [Header("Phone Ring After Dialogue")]
    public AudioSource phoneRingSource;
    public bool ringAfterDialogue = true;
    public float phoneRingSeconds = 2f;

    public bool conversationEnded = false;

    private bool playerInRange;
    private bool inDialogue;
    private int lineIndex;

    private Coroutine typingCo;
    private bool isTyping;

    private Coroutine phoneRingCo;

    void Start()
    {
        if (promptRoot) promptRoot.SetActive(false);
        if (dialoguePanel) dialoguePanel.SetActive(false);
    }

    void Update()
    {
        if (!playerInRange) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!inDialogue)
            {
                if (oneTimeOnly && talkedOnce) return;
                StartDialogue();
            }
            else
            {
                NextLine();
            }
        }
    }

    void StartDialogue()
    {
        if (lines == null || lines.Length == 0) return;

        inDialogue = true;
        lineIndex = 0;

        if (promptRoot) promptRoot.SetActive(false);
        if (dialoguePanel) dialoguePanel.SetActive(true);

        if (nameText) nameText.text = npcName;

        PlayVoice(lineIndex);
        StartTyping(lines[lineIndex]);

        if (movementScript) movementScript.enabled = false;
    }

    void NextLine()
    {
        if (isTyping)
        {
            StopTyping();
            lineText.text = lines[lineIndex];
            return;
        }

        lineIndex++;

        if (lineIndex >= lines.Length)
        {
            EndDialogue();
            return;
        }

        PlayVoice(lineIndex);
        StartTyping(lines[lineIndex]);
    }

    void EndDialogue()
    {
        StopTyping();

        if (npcVoiceSource != null)
            npcVoiceSource.Stop();

        conversationEnded = true;

        // ✅ ปลดล็อกให้รับโทรศัพท์ได้
        GameState.phoneUnlocked = true;

        Debug.Log("Phone Unlocked");

        if (oneTimeOnly) talkedOnce = true;

        inDialogue = false;

        if (dialoguePanel) dialoguePanel.SetActive(false);

        if (movementScript) movementScript.enabled = true;

        if (ringAfterDialogue && phoneRingSource != null)
        {
            if (phoneRingCo != null) StopCoroutine(phoneRingCo);
            phoneRingCo = StartCoroutine(PlayPhoneRingForSeconds(phoneRingSeconds));
        }
    }

    IEnumerator PlayPhoneRingForSeconds(float seconds)
    {
        phoneRingSource.Stop();
        phoneRingSource.time = 0f;
        phoneRingSource.Play();

        yield return new WaitForSeconds(seconds);

        phoneRingSource.Stop();
        phoneRingSource.time = 0f;

        phoneRingCo = null;
    }

    void PlayVoice(int index)
    {
        if (npcVoiceSource == null) return;

        if (voiceClips != null && index < voiceClips.Length)
        {
            npcVoiceSource.Stop();
            npcVoiceSource.clip = voiceClips[index];
            npcVoiceSource.Play();
        }
    }

    void StartTyping(string fullText)
    {
        StopTyping();
        typingCo = StartCoroutine(TypeLine(fullText));
    }

    IEnumerator TypeLine(string fullText)
    {
        isTyping = true;
        lineText.text = "";

        int charCount = 0;

        foreach (char c in fullText)
        {
            lineText.text += c;
            charCount++;

            if (typeAudioSource != null && typeClip != null)
            {
                if (charCount % playTypeSoundEveryNChars == 0 && !char.IsWhiteSpace(c))
                {
                    typeAudioSource.PlayOneShot(typeClip, typeVolume);
                }
            }

            yield return new WaitForSeconds(typeSpeed);
        }

        isTyping = false;
        typingCo = null;
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

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;

        if (oneTimeOnly && talkedOnce) return;

        if (!inDialogue && promptRoot)
        {
            promptRoot.SetActive(true);
            if (promptText) promptText.text = "กด E เพื่อคุย";
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;

        if (promptRoot) promptRoot.SetActive(false);

        if (inDialogue) EndDialogue();
    }
}