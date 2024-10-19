using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        NPC npc = collision.GetComponent<NPC>();
        if (npc != null)
        {
            // Здесь предполагаем, что у тебя есть ссылка на DialogueManager
            NPCControllerD dialogueManager = FindObjectOfType<NPCControllerD>();
            dialogueManager.StartDialogue(npc); // Передаем npc, а не Dialogue
        }
    }
}
