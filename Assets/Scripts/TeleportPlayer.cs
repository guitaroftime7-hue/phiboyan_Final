using UnityEngine;
using System.Collections;

public class TeleportPlayer : MonoBehaviour
{
    public Transform warpDestination;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameState2.talkedToFather)
            {
                CharacterController cc = other.GetComponent<CharacterController>();

                if (cc != null)
                {
                    cc.enabled = false;
                    other.transform.position = warpDestination.position;
                    cc.enabled = true;
                }
            }
            else
            {
                Debug.Log("ต้องคุยกับพ่อก่อน");
            }
        }
    }
}