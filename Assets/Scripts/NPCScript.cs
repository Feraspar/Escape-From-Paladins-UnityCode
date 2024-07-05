using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class NPCScript : MonoBehaviour
{
    public string npcName = "Бернард";
    public string chestName = "Сундук";
    [HideInInspector] public bool isSpeak;
    [HideInInspector] public bool invisChestIsOpen;
    [HideInInspector] public bool speedChestIsOpen;
    [HideInInspector] public bool keyChestIsOpen;
    public Object mainDialogue;
    public Object invisPotionDialogue;
    public Object speedPotionDialogue;
    public Object keyDialogue;
    public GameObject dialogueSystem;

    public PlayerStats playerStats;
    public void interactNPC()
    {
        if (gameObject.name.Equals("Bernard"))
        {
            playerStats.GetComponent<MovementStateManager>().mouseSense = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            dialogueSystem.GetComponent<DialogueSystem>().loadDialogue(mainDialogue);
            playerStats.isKey = true;
            isSpeak = false;
        }
    }

    public void interactInvisiblePotionChest()
    {
        if (gameObject.name.Equals("ChestInvis"))
        {
            FindObjectOfType<AudioManager>().Play("Chest");
            playerStats.GetComponent<MovementStateManager>().mouseSense = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            dialogueSystem.GetComponent<DialogueSystem>().loadDialogue(invisPotionDialogue);
            dialogueSystem.GetComponent<DialogueSystem>().setAction("Взять зелье невидимости", pickUpInvisiblePotion);
        }
    }

    public void interactSpeedPotionChest()
    {
        if (gameObject.name.Equals("ChestSpeed"))
        {
            FindObjectOfType<AudioManager>().Play("Chest");
            playerStats.GetComponent<MovementStateManager>().mouseSense = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            dialogueSystem.GetComponent<DialogueSystem>().loadDialogue(speedPotionDialogue);
            dialogueSystem.GetComponent<DialogueSystem>().setAction("Взять зелье скорости", pickUpSpeedPotion);
        }
    }

    public void interactKey()
    {
        if (gameObject.name.Equals("ChestKey"))
        {
            FindObjectOfType<AudioManager>().Play("Chest");
            playerStats.GetComponent<MovementStateManager>().mouseSense = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            dialogueSystem.GetComponent<DialogueSystem>().loadDialogue(keyDialogue);
            playerStats.isMainKey = true;
            keyChestIsOpen = false;
        }
    }

    private void Start()
    {
        isSpeak = true;
        invisChestIsOpen = true;
        speedChestIsOpen = true;
        keyChestIsOpen = true;
}

    void pickUpInvisiblePotion()
    {
        playerStats.invisiblePotion = true;
        invisChestIsOpen = false;
        FindObjectOfType<UIMainMenu>().ActiveInvisIcon();
    }

    void pickUpSpeedPotion()
    {
        playerStats.speedPotion = true;
        speedChestIsOpen = false;
        FindObjectOfType<UIMainMenu>().ActiveSpeedIcon();
    }
}
