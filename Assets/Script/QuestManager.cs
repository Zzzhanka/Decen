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
        // ��������� ���������� ������
        Debug.Log("����� ��������: " + questName);
    }
}
