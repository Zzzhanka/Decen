using UnityEngine;
using UnityEngine.UI;

public class KillButton : MonoBehaviour
{
    public NPCMovement npc; // —сылка на NPC

    private void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(() => npc.KillNPC());
    }
}
