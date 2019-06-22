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
    public Button stoneButton;

    public Text healthPotionText;
    public Text stoneButtonText;

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
        // activate or deactivate x item button if there are/aren't x items
        checkItemInCombat("Health Potion", healthPotionButton, healthPotionText);
        checkItemInCombat("Stone", stoneButton, stoneButtonText); 

    }

    void Update()
    {
        
    }
}
