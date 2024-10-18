using System.Collections.Generic;

[System.Serializable]
public class Dialogue
{
    public string npcName; // ��� NPC
    public List<string> sentences; // ������ ����������� NPC
    public List<string[]> responses; // �������� ������� ������

    public Dialogue(string name, List<string> sentences, List<string[]> responses)
    {
        npcName = name;
        this.sentences = sentences;
        this.responses = responses;
    }
}
