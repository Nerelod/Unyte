using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public List<string> aquiredItems = new List<string>();

    public string itemToUse;
    
    public bool isInCombat;
    void Start()
    {
        isInCombat = false;
    }

    public void healthPotion(DataManager player){     
        player.health = player.health + 5;
        if(player.health > player.totalHealth){ player.health = player.totalHealth; }
        if(player.itemManager.isInCombat){
            CombatTextManager.combatTextManager.ManageText("Used Health Potion!");
        }
        player.itemManager.aquiredItems.Remove("Health_Potion");
    }
    public void stone(DataManager player){
        if(player.itemManager.isInCombat){
            CombatTextManager.combatTextManager.ManageText("Used Stone!");
            EnemyDataManager.EnemyManager.health -= 2;
        }

    }
    public void playerOneSelectItem(string selectedItem){
        if(DataManager.playerOne.itemManager.aquiredItems.Contains(selectedItem)){
            DataManager.playerOne.itemManager.itemToUse = selectedItem;
            CombatMenuManager.combatMenuManager.itemSelectPanel.SetActive(false);         
        }
    }
    public void playerOneSelectItemOutsideCombat(string selectedItem){
        if(DataManager.playerOne.itemManager.aquiredItems.Contains(selectedItem)){
            DataManager.playerOne.itemManager.itemToUse = selectedItem;
            DataManager.playerOne.itemManager.useItem(DataManager.playerOne);        
        }
    }
    public void useItem(DataManager player){
        if(itemToUse == "Health_Potion"){
            healthPotion(player);
        }
        else if(itemToUse == "Stone"){
            stone(player);
        }
    }
    public void turnOffItemSelect(){
        CombatMenuManager.combatMenuManager.itemSelectPanel.SetActive(false);
        CombatTextManager.combatTextManager.textHasBeenPrompt = false;
    }
    public int getAmountOfItem(string itemToCount){
        int amount = 0;
        foreach(string item in aquiredItems){
            if(item == itemToCount){
                amount++;
            }
        }
        return amount;
    }
    void Update()
    {
        
    }
}
