using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SleepTrigger : MonoBehaviour
{
    public string nextSceneName;
    public GameObject sleepPrompt;
    public ScreenFade screenFade;

    private bool playerInRange;

    void Start()
    {
        if (sleepPrompt != null)
            sleepPrompt.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(SleepAndLoad());
        }
    }

    IEnumerator SleepAndLoad()
    {
        sleepPrompt.SetActive(false);

        yield return StartCoroutine(screenFade.FadeOut());

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(nextSceneName);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            sleepPrompt.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            sleepPrompt.SetActive(false);
        }
    }
}