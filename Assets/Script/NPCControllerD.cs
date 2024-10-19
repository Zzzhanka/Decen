using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class NPCControllerD : MonoBehaviour
{
    public TextMeshProUGUI dialogueText; // ����� �������
    public Button[] responseButtons; // ������ ��� �������
    private NPC currentNPC; // ������� NPC
    private int currentDialogueIndex = 0; // ������ �������� �������

    private void Start()
    {
        // ������ ������ ����������
        foreach (var button in responseButtons)
        {
            button.gameObject.SetActive(false);
        }
    }

    // ����� ��� ������ ������� � NPC
    public void StartDialogue(NPC npc)
    {
        currentNPC = npc;
        currentDialogueIndex = 0; // ����� �������
        ShowNextDialogue();
    }

    // ����� ��� ������ ���������� �������
    public void ShowNextDialogue()
    {
        // ���� ���� ��� �������
        if (currentDialogueIndex < currentNPC.dialogues.Length)
        {
            dialogueText.text = currentNPC.dialogues[currentDialogueIndex]; // ���������� ������� ������
            ShowResponseOptions();
        }
        else
        {
            // ���� ������� �����������, �������� ������
            dialogueText.text = "";
            foreach (var button in responseButtons)
            {
                button.gameObject.SetActive(false);
            }
        }
    }

    // ����� ��� ������ ��������� �������
    private void ShowResponseOptions()
    {
        // ������ ���������� ������ ���� ��� �������
        for (int i = 0; i < responseButtons.Length; i++)
        {
            if (i < currentNPC.dialogues.Length)
            {
                responseButtons[i].GetComponentInChildren<Text>().text = "����� " + (i + 1); // ������ �� ���� ������
                responseButtons[i].gameObject.SetActive(true);
                int index = i; // ��������� ���������� ��� ���������
                responseButtons[i].onClick.RemoveAllListeners(); // ������� ������ ���������
                responseButtons[i].onClick.AddListener(() => OnResponseSelected(index));
            }
            else
            {
                responseButtons[i].gameObject.SetActive(false);
            }
        }
    }

    // ����� ��� ��������� ������ ������
    private void OnResponseSelected(int index)
    {
        // ������ ������ � ������� ������
        dialogueText.text += "\n" + responseButtons[index].GetComponentInChildren<Text>().text;

        // ����������� ������ �������� �������
        currentDialogueIndex++;

        // ������� ������ ���������, ����� �� ���� ������������
        foreach (var button in responseButtons)
        {
            button.onClick.RemoveAllListeners();
            button.gameObject.SetActive(false);
        }

        // �������� ��������� ������
        ShowNextDialogue();
    }
}
