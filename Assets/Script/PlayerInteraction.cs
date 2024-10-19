using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        NPC npc = collision.GetComponent<NPC>();
        if (npc != null)
        {
            // ����� ������������, ��� � ���� ���� ������ �� DialogueManager
            NPCControllerD dialogueManager = FindObjectOfType<NPCControllerD>();
            dialogueManager.StartDialogue(npc); // �������� npc, � �� Dialogue
        }
    }
}
