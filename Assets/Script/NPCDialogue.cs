using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCDialog : MonoBehaviour
{
    public GameObject dialogPanel;       // ������ �������
    public TextMeshProUGUI dialogText;   // ����� �������
    public TextMeshProUGUI questText;    // ����� ��� ������� (����� NPC)

    public string[] npcDialogs;          // ����� NPC
    public string[] playerDialogs;       // ����� ������
    public string angryPhrase;           // ������ ����� NPC (������� ���� ������)

    private bool playerInRange = false;  // ����� ����� � NPC
    private bool npcAngry = false;       // ����, ��� NPC ���
    private Coroutine dialogCoroutine;   // ��� ������������ ��������
    private bool dialogActive = false;   // ��� �� ������

    private List<string> usedNpcDialogs = new List<string>();  // ������ �������������� ���� NPC

    private void Start()
    {
        dialogPanel.SetActive(false);
        questText.text = "";  // �������� ����� �������
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            // ���� �������� ��� ��������, ��������� �
            if (dialogCoroutine != null)
                StopCoroutine(dialogCoroutine);

            dialogCoroutine = StartCoroutine(StartDialog(1f));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            StopDialog();
        }
    }

    private IEnumerator StartDialog(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (playerInRange)
        {
            dialogPanel.SetActive(true);
            dialogActive = true;

            if (npcAngry)  // ���� NPC ��� ���, ����� ������� ���� �����
            {
                dialogText.text = $"NPC: {angryPhrase}";
                QuestToKillNPC();  // ����� �������� �������
                yield break;  // �������� ������
            }
            else
            {
                yield return StartCoroutine(DialogExchange());
            }
        }
    }

    private IEnumerator DialogExchange()
    {
        while (dialogActive)
        {
            // ��� NPC: �������� ��������� �����, ������� ��� �� ��������������
            string npcPhrase = GetUniquePhrase(npcDialogs);
            if (npcPhrase == null)  // ���� ����� �����������
            {
                dialogText.text = $"NPC: {angryPhrase}";
                QuestToKillNPC();  // ����� �������� �������
                yield break;  // �������� ������
            }

            dialogText.text = $"NPC: {npcPhrase}";
            yield return new WaitForSeconds(2f);

            // ��� ������
            string playerPhrase = GetRandomPhrase(playerDialogs);
            dialogText.text = $"�����: {playerPhrase}";

            yield return new WaitForSeconds(2f);
        }
    }

    private void StopDialog()
    {
        if (dialogCoroutine != null)
        {
            StopCoroutine(dialogCoroutine);
            dialogCoroutine = null;  // ��������� ��� ���������� �������
        }

        dialogPanel.SetActive(false);
        dialogActive = false;

        // ���������, ��� ��� ��������� ������ NPC ����� �������
        npcAngry = true;
    }

    private string GetUniquePhrase(string[] phrases)
    {
        // �������� �����, ������� ��� �� ���� ������������
        List<string> availablePhrases = new List<string>(phrases);
        availablePhrases.RemoveAll(phrase => usedNpcDialogs.Contains(phrase));

        if (availablePhrases.Count == 0) return null;  // ���� ����� �����������

        string randomPhrase = availablePhrases[Random.Range(0, availablePhrases.Count)];
        usedNpcDialogs.Add(randomPhrase);  // ��������� ����� � ��������������
        return randomPhrase;
    }

    private string GetRandomPhrase(string[] phrases)
    {
        if (phrases.Length == 0) return "";  // �������� �� ������ ������
        int randomIndex = Random.Range(0, phrases.Length);
        return phrases[randomIndex];
    }

    private void QuestToKillNPC()
    {
        questText.text = "�������: ���� NPC!";
        StopDialog();  // ��������� ������ � ��������� ������
    }
}
