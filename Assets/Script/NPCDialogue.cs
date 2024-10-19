using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCDialog : MonoBehaviour
{
    public GameObject dialogPanel;       // Панель диалога
    public TextMeshProUGUI dialogText;   // Текст диалога
    public TextMeshProUGUI questText;    // Текст для задания (убить NPC)

    public string[] npcDialogs;          // Фразы NPC
    public string[] playerDialogs;       // Фразы игрока
    public string angryPhrase;           // Плохая фраза NPC (которая злит игрока)

    private bool playerInRange = false;  // Игрок рядом с NPC
    private Coroutine dialogCoroutine;   // Для отслеживания корутины
    private bool dialogActive = false;   // Диалог идёт?

    private void Start()
    {
        dialogPanel.SetActive(false);
        questText.text = "";  // Очистить текст задания
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
        dialogPanel.SetActive(true);  // Открываем панель диалога
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
            StartCoroutine(DialogExchange());  // Запуск диалога
        }
    }

    private IEnumerator DialogExchange()
    {
        while (dialogActive)
        {
            // Ход NPC
            string npcPhrase = GetRandomPhrase(npcDialogs);
            dialogText.text = $"NPC: {npcPhrase}";

            // Проверка на "плохую" фразу
            if (npcPhrase == angryPhrase)
            {
                yield return new WaitForSeconds(2f);
                QuestToKillNPC();  // Игрок злится и получает задание
                yield break;       // Прервать диалог
            }

            yield return new WaitForSeconds(2f);

            // Ход игрока
            string playerPhrase = GetRandomPhrase(playerDialogs);
            dialogText.text = $"Игрок: {playerPhrase}";

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
        questText.text = "Задание: Убей NPC!";
        dialogPanel.SetActive(false);
        dialogActive = false;
    }
}
