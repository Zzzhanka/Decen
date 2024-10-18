using System.Collections.Generic;
using UnityEngine;

public class GameSetup : MonoBehaviour
{
    private void Start()
    {
        var sentences = new List<string>
        {
            "�� ����������� �� ������!",
            "�� ���� �� ������ �������� ���� ������?"
        };

        var responses = new List<string[]>
        {
            new string[] { "������, � �����!", "������ ����������!" },
            new string[] { "� �� ���������.", "��� �� ���� ����." }
        };

        Dialogue officeWorkerDialogue = new Dialogue("������� ��������", sentences, responses);

        // ����� ����� ���������� ������ ��� NPC
        var npc = FindObjectOfType<NPCDialogueTrigger>();
        npc.dialogue = officeWorkerDialogue;
    }
}
