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
    private Coroutine dialogCoroutine;   // ��� ������������ ��������
    private bool dialogActive = false;   // ������ ���?

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
            dialogCoroutine = StartCoroutine(StartDialog(5f));
        }
    }  
    public void StartDialog()
    {
        dialogPanel.SetActive(true);  // ��������� ������ �������
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
            StartCoroutine(DialogExchange());  // ������ �������
        }
    }

    private IEnumerator DialogExchange()
    {
        while (dialogActive)
        {
            // ��� NPC
            string npcPhrase = GetRandomPhrase(npcDialogs);
            dialogText.text = $"NPC: {npcPhrase}";

            // �������� �� "������" �����
            if (npcPhrase == angryPhrase)
            {
                yield return new WaitForSeconds(2f);
                QuestToKillNPC();  // ����� ������ � �������� �������
                yield break;       // �������� ������
            }

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
            StopCoroutine(dialogCoroutine);

        dialogPanel.SetActive(false);
        dialogActive = false;
    }

    private string GetRandomPhrase(string[] phrases)
    {
        int randomIndex = Random.Range(0, phrases.Length);
        return phrases[randomIndex];
    }

    private void QuestToKillNPC()
    {
        questText.text = "�������: ���� NPC!";
        dialogPanel.SetActive(false);
        dialogActive = false;
    }
}
