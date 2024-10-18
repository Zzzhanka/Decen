using System.Collections.Generic;

[System.Serializable]
public class Dialogue
{
    public string npcName; // Имя NPC
    public List<string> sentences; // Список предложений NPC
    public List<string[]> responses; // Варианты ответов игрока

    public Dialogue(string name, List<string> sentences, List<string[]> responses)
    {
        npcName = name;
        this.sentences = sentences;
        this.responses = responses;
    }
}
