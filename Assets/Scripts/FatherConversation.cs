using UnityEngine;

public class FatherConversation : MonoBehaviour
{
    public NPCConversation npcConversation;
    bool done = false;

    void Update()
    {
        if (!done && npcConversation.conversationEnded)
        {
            GameState2.talkedToFather = true;
            done = true;

            Debug.Log("คุยกับพ่อเสร็จแล้ว");
        }
    }
}