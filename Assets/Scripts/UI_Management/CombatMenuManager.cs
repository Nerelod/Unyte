using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatMenuManager : MonoBehaviour
{
    public static CombatMenuManager combatMenuManager;
    // Ability Panel shenanigans
    public GameObject abilitySelectPanel;
    public Button abilityReturnButton;
    // Item Panel shenanigans
    public GameObject itemSelectPanel;
    public Button itemReturnButton;
    public Button healthPotionButton;
    public Button stoneButton;

    public Text healthPotionText;
    public Text stoneButtonText;
    // Ally Select Panel shenanigans
    public GameObject allySelectPanel;
    public Button junakButton;
    public Button saralfButton;

    void Start()
    {
        abilitySelectPanel.SetActive(false);
        itemSelectPanel.SetActive(false);
        allySelectPanel.SetActive(false);
    }

    private void checkItemInCombat(string itemName, Button button, Text textBox) {
        if (DataManager.Junak.itemManager.aquiredItems.Contains(itemName)) {
            button.gameObject.SetActive(true);
            textBox.text = itemName + " " + DataManager.Junak.itemManager.getAmountOfItem(itemName);
        }
        else {
            button.gameObject.SetActive(false);
        }
    }

    public void itemPanelWhenTurnedOn(){
        itemSelectPanel.SetActive(true);
        itemReturnButton.Select();
        // activate or deactivate x item button if there are/aren't x items
        checkItemInCombat("Health Potion", healthPotionButton, healthPotionText);
        checkItemInCombat("Stone", stoneButton, stoneButtonText); 

    }
    public void abilityPanelWhenTurnedOn()
    {
        abilitySelectPanel.SetActive(true);
        abilityReturnButton.Select();
    }
    public void allySelectPanelWhenTurnedOn() {
        allySelectPanel.SetActive(true);
        junakButton.Select();
    }

    void Update()
    {
        
    }
}
