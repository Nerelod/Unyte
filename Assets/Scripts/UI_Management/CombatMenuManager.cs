using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatMenuManager : MonoBehaviour
{
    public static CombatMenuManager combatMenuManager;
    public GameObject abilitySelectPanel;
    public Button abilityReturnButton;

    public GameObject itemSelectPanel;
    public Button itemReturnButton;
    public Button healthPotionButton;

    public Text healthPotionText;

    void Start()
    {
        abilitySelectPanel.SetActive(false);
        itemSelectPanel.SetActive(false);
    }

    private void checkItemInCombat(string itemName, Button button, Text textBox) {
        if (DataManager.playerOne.itemManager.aquiredItems.Contains(itemName)) {
            button.gameObject.SetActive(true);
            textBox.text = itemName + " " + DataManager.playerOne.itemManager.getAmountOfItem(itemName);
        }
        else {
            button.gameObject.SetActive(false);
        }
    }

    public void whenTurnedOn(){
        itemSelectPanel.SetActive(true);
        itemReturnButton.Select();
        // activate or deactivate health potion button if there ar/aren't health potions
        checkItemInCombat("Health Potion", healthPotionButton, healthPotionText);
    }

    void Update()
    {
        
    }
}
