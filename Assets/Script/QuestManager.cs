using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateQuest(string questName)
    {
        // Обработка обновления квеста
        Debug.Log("Квест обновлен: " + questName);
    }
}
