using UnityEngine;

public class NPCDialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue; // ������ ��� NPC
    private bool isPlayerInRange;

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E)) // ���� ����� ����� E
        {
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // ���������, ��� ����� ����� � ����
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // ���������, ��� ����� ������� ����
        {
            isPlayerInRange = false;
        }
    }
}
