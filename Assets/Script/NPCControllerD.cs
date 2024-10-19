using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class NPCControllerD : MonoBehaviour
{
    public TextMeshProUGUI dialogueText; // Текст диалога
    public Button[] responseButtons; // Кнопки для ответов
    private NPC currentNPC; // Текущий NPC
    private int currentDialogueIndex = 0; // Индекс текущего диалога

    private void Start()
    {
        // Делаем кнопки невидимыми
        foreach (var button in responseButtons)
        {
            button.gameObject.SetActive(false);
        }
    }

    // Метод для начала диалога с NPC
    public void StartDialogue(NPC npc)
    {
        currentNPC = npc;
        currentDialogueIndex = 0; // Сброс индекса
        ShowNextDialogue();
    }

    // Метод для показа следующего диалога
    public void ShowNextDialogue()
    {
        // Если есть ещё диалоги
        if (currentDialogueIndex < currentNPC.dialogues.Length)
        {
            dialogueText.text = currentNPC.dialogues[currentDialogueIndex]; // Показываем текущий диалог
            ShowResponseOptions();
        }
        else
        {
            // Если диалоги закончились, скрываем панель
            dialogueText.text = "";
            foreach (var button in responseButtons)
            {
                button.gameObject.SetActive(false);
            }
        }
    }

    // Метод для показа вариантов ответов
    private void ShowResponseOptions()
    {
        // Пример случайного выбора фраз для ответов
        for (int i = 0; i < responseButtons.Length; i++)
        {
            if (i < currentNPC.dialogues.Length)
            {
                responseButtons[i].GetComponentInChildren<Text>().text = "Ответ " + (i + 1); // Измени на свои ответы
                responseButtons[i].gameObject.SetActive(true);
                int index = i; // Локальная переменная для замыкания
                responseButtons[i].onClick.RemoveAllListeners(); // Удаляем старые слушатели
                responseButtons[i].onClick.AddListener(() => OnResponseSelected(index));
            }
            else
            {
                responseButtons[i].gameObject.SetActive(false);
            }
        }
    }

    // Метод для обработки выбора ответа
    private void OnResponseSelected(int index)
    {
        // Запись выбора и скрытие кнопок
        dialogueText.text += "\n" + responseButtons[index].GetComponentInChildren<Text>().text;

        // Увеличиваем индекс текущего диалога
        currentDialogueIndex++;

        // Удаляем старые слушатели, чтобы не было дублирования
        foreach (var button in responseButtons)
        {
            button.onClick.RemoveAllListeners();
            button.gameObject.SetActive(false);
        }

        // Показать следующий диалог
        ShowNextDialogue();
    }
}
