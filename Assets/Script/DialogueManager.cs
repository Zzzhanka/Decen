using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText; // ����� ��� ����������� �������
    public GameObject dialoguePanel; // ������ ��� �������
    private Dialogue currentDialogue; // ������� ������
    private int currentSentenceIndex; // ������ �������� �����������

    public void StartDialogue(Dialogue dialogue)
    {
        currentDialogue = dialogue;
        currentSentenceIndex = 0;
        dialoguePanel.SetActive(true);
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (currentSentenceIndex < currentDialogue.sentences.Count)
        {
            dialogueText.text = currentDialogue.sentences[currentSentenceIndex];
            currentSentenceIndex++;
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);
    }
}
