using UnityEngine;
using System.Collections;

public class WarpPosition : MonoBehaviour
{
    public Transform warpTarget;
    public ScreenFade screenFade;

    private bool isWarping = false;

    void OnTriggerEnter(Collider other)
    {
        if (isWarping) return;

        if (other.CompareTag("Player"))
        {
            StartCoroutine(WarpPlayer(other));
        }
    }

    IEnumerator WarpPlayer(Collider player)
    {
        isWarping = true;

        if (screenFade != null)
            yield return StartCoroutine(screenFade.FadeOut());

        CharacterController cc = player.GetComponent<CharacterController>();

        if (cc != null) cc.enabled = false;

        player.transform.position = warpTarget.position;

        if (cc != null) cc.enabled = true;

        yield return new WaitForSeconds(0.2f);

        isWarping = false;
    }
}