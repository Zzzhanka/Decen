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
    private bool npcAngry = false;       // Флаг, что NPC зол
    private Coroutine dialogCoroutine;   // Для отслеживания корутины
    private bool dialogActive = false;   // Идёт ли диалог

    private List<string> usedNpcDialogs = new List<string>();  // Список использованных фраз NPC

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

            // Если корутина уже запущена, остановим её
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

            if (npcAngry)  // Если NPC уже зол, сразу говорим злую фразу
            {
                dialogText.text = $"NPC: {angryPhrase}";
                QuestToKillNPC();  // Игрок получает задание
                yield break;  // Прервать диалог
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
            // Ход NPC: получаем случайную фразу, которая ещё не использовалась
            string npcPhrase = GetUniquePhrase(npcDialogs);
            if (npcPhrase == null)  // Если фразы закончились
            {
                dialogText.text = $"NPC: {angryPhrase}";
                QuestToKillNPC();  // Игрок получает задание
                yield break;  // Прервать диалог
            }

            dialogText.text = $"NPC: {npcPhrase}";
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
        {
            StopCoroutine(dialogCoroutine);
            dialogCoroutine = null;  // Обнуление для повторного запуска
        }

        dialogPanel.SetActive(false);
        dialogActive = false;

        // Фиксируем, что при повторном заходе NPC будет злиться
        npcAngry = true;
    }

    private string GetUniquePhrase(string[] phrases)
    {
        // Выбираем фразу, которая ещё не была использована
        List<string> availablePhrases = new List<string>(phrases);
        availablePhrases.RemoveAll(phrase => usedNpcDialogs.Contains(phrase));

        if (availablePhrases.Count == 0) return null;  // Если фразы закончились

        string randomPhrase = availablePhrases[Random.Range(0, availablePhrases.Count)];
        usedNpcDialogs.Add(randomPhrase);  // Добавляем фразу в использованные
        return randomPhrase;
    }

    private string GetRandomPhrase(string[] phrases)
    {
        if (phrases.Length == 0) return "";  // Проверка на пустой массив
        int randomIndex = Random.Range(0, phrases.Length);
        return phrases[randomIndex];
    }

    private void QuestToKillNPC()
    {
        questText.text = "Задание: Убей NPC!";
        StopDialog();  // Закрываем панель и завершаем диалог
    }
}
