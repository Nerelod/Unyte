using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public List<string> aquiredItems = new List<string>();

    public string itemToUse;
    void Start()
    {
        
    }

    public void healthPotion(DataManager player){       
        player.health = player.health + 5;
        if(player.health > player.totalHealth){ player.health = player.totalHealth; }
        CombatTextManager.combatTextManager.ManageText("Used Health Potion!");
        player.itemManager.aquiredItems.Remove("Health_Potion");
    }
    public void playerOneSelectItem(string selectedItem){
        if(DataManager.playerOne.itemManager.aquiredItems.Contains(selectedItem)){
            DataManager.playerOne.itemManager.itemToUse = selectedItem;
            CombatMenuManager.combatMenuManager.itemSelectPanel.SetActive(false);
        }
    }
    public void useItem(DataManager player){
        if(itemToUse == "Health_Potion"){
            healthPotion(player);
        }
    }

    public void turnOffItemSelect(){
        CombatMenuManager.combatMenuManager.itemSelectPanel.SetActive(false);
        CombatTextManager.combatTextManager.textHasBeenPrompt = false;
    }
    void Update()
    {
        
    }
}
