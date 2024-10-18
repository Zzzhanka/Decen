using System.Collections.Generic;
using UnityEngine;

public class GameSetup : MonoBehaviour
{
    private void Start()
    {
        var sentences = new List<string>
        {
            "Ты опаздываешь на работу!",
            "Ты ведь не хочешь потерять свою работу?"
        };

        var responses = new List<string[]>
        {
            new string[] { "Извини, я спешу!", "Сейчас постараюсь!" },
            new string[] { "Я не опаздываю.", "Это не твое дело." }
        };

        Dialogue officeWorkerDialogue = new Dialogue("Офисный работник", sentences, responses);

        // Здесь можно установить диалог для NPC
        var npc = FindObjectOfType<NPCDialogueTrigger>();
        npc.dialogue = officeWorkerDialogue;
    }
}
