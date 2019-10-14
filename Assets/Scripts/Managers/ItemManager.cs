using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public List<string> aquiredItems = new List<string>();
    public List<string> itemsThatWereRemoved = new List<string>();

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
            CombatTextManager.combatTextManager.ManageText(player.theName + " Used Health Potion!");
        }
        DataManager.Junak.itemManager.aquiredItems.Remove("Health Potion");
    }
    public void stone(DataManager player){
        if(player.itemManager.isInCombat){
            CombatTextManager.combatTextManager.ManageText(player.theName + " Used Stone!");
            EnemyDataManager.EnemyManager.health -= 2;
            CombatTextManager.combatTextManager.damageText.text = "-2";
            CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.FadeText(CombatTextManager.combatTextManager.damageText));
            DataManager.Junak.itemManager.aquiredItems.Remove("Stone");
        }

    }
    public void JunakSelectItem(string selectedItem){
        if(DataManager.Junak.itemManager.aquiredItems.Contains(selectedItem)){
            DataManager.Junak.itemManager.itemToUse = selectedItem;
            CombatMenuManager.combatMenuManager.itemSelectPanel.SetActive(false);         
        }
    }
    public void JunakSelectItemOutsideCombat(string selectedItem){
        if(DataManager.Junak.itemManager.aquiredItems.Contains(selectedItem)){
            DataManager.Junak.itemManager.itemToUse = selectedItem;
            DataManager.Junak.itemManager.useItem(DataManager.Junak);        
        }
    }
    public void selectItemInCombat(string selectedItem) {
        if (DataManager.Junak.isTurnInCombat) {
            if (DataManager.Junak.itemManager.aquiredItems.Contains(selectedItem)) {
                DataManager.Junak.itemManager.itemToUse = selectedItem;
                CombatMenuManager.combatMenuManager.itemSelectPanel.SetActive(false);
            }
        }
        else if (SaralfDataManager.Saralf.isTurnInCombat) {
            if (DataManager.Junak.itemManager.aquiredItems.Contains(selectedItem)) {
                SaralfDataManager.Saralf.itemManager.itemToUse = selectedItem;
                CombatMenuManager.combatMenuManager.itemSelectPanel.SetActive(false);
            }
        }
    }
    public void useItem(DataManager player){
        if(itemToUse == "Health Potion"){
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
