using UnityEngine;
using TMPro;

public class RadioInteract : MonoBehaviour
{
    [SerializeField] private AudioSource radioSource;

    [Header("UI Prompt")]
    [SerializeField] private GameObject promptRoot; // ลาก InteractText (ทั้ง GameObject) ใส่
    [SerializeField] private TMP_Text promptText;   // ลาก TextMeshPro component ใส่

    private bool playerInRange;

    void Start()
    {
        if (promptRoot != null) promptRoot.SetActive(false);
        if (radioSource == null) radioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!playerInRange) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (radioSource == null) return;

            if (radioSource.isPlaying) radioSource.Pause();
            else radioSource.Play();

            UpdatePromptText(); // เปลี่ยนข้อความตามสถานะ
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;
        if (promptRoot != null) promptRoot.SetActive(true);
        UpdatePromptText();
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;
        if (promptRoot != null) promptRoot.SetActive(false);
    }

    void UpdatePromptText()
    {
        if (promptText == null || radioSource == null) return;

        // ถ้าเปิดอยู่ ให้ขึ้นคำว่า "ปิด" ถ้าปิดอยู่ให้ขึ้นคำว่า "เปิด"
        promptText.text = radioSource.isPlaying
            ? "กด E เพื่อปิดวิทยุ"
            : "กด E เพื่อเปิดวิทยุ";
    }
}
