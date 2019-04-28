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

    void Start()
    {
        abilitySelectPanel.SetActive(false);
        itemSelectPanel.SetActive(false);
    }

    public void whenTurnedOn(){
        itemSelectPanel.SetActive(true);
        itemReturnButton.Select();
        // activate or deactivate health potion button if there ar/aren't health potions
        if(DataManager.playerOne.itemManager.aquiredItems.Contains("Health_Potion")){ healthPotionButton.gameObject.SetActive(true); } else { healthPotionButton.gameObject.SetActive(false); }
    }

    void Update()
    {
        
    }
}
