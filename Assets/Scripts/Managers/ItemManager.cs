﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public List<string> aquiredItems = new List<string>();
    public List<string> itemsThatWereRemoved = new List<string>();
    // Items that require an ally to be chosen
    public List<string> allyItems = new List<string>();

    public string itemToUse;
    public DataManager allyToTarget;
    public bool isInCombat;
    void Start()
    {
        isInCombat = false;
        allyItems.Add("Health Potion");
    }

    public void selectItemOutsideCombat(string selectedItem){
        if(DataManager.Junak.itemManager.aquiredItems.Contains(selectedItem)){
            if (allyItems.Contains(selectedItem)) {
                GameMenuManager.gameMenuManager.canInteractWith = false;
                GameMenuManager.gameMenuManager.allySelectPanelWhenTurnedOn();
            }
            DataManager.Junak.itemManager.itemToUse = selectedItem;
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
        if (DataManager.Junak.itemManager.aquiredItems.Contains(selectedItem)) {
            if (allyItems.Contains(selectedItem)) {
                CombatMenuManager.combatMenuManager.allySelectPanelWhenTurnedOn();
            }
        }
        DataManager.Junak.itemManager.aquiredItems.Remove(selectedItem);
    }
    public void selectAlly(string allyName) {
        if(allyName == "Junak") {
            if (DataManager.Junak.isTurnInCombat) { DataManager.Junak.itemManager.allyToTarget = DataManager.Junak; }
            else if (SaralfDataManager.Saralf.isTurnInCombat) { SaralfDataManager.Saralf.itemManager.allyToTarget = DataManager.Junak; }
        }
        else if (allyName == "Saralf") {
            if (DataManager.Junak.isTurnInCombat) { DataManager.Junak.itemManager.allyToTarget = SaralfDataManager.Saralf; }
            else if (SaralfDataManager.Saralf.isTurnInCombat) { SaralfDataManager.Saralf.itemManager.allyToTarget = SaralfDataManager.Saralf; }
        }
        CombatMenuManager.combatMenuManager.allySelectPanel.SetActive(false);
    }
    public void useItem(DataManager player){
        if(itemToUse == "Health Potion"){
            HealthPotionItem.healthPotionItem.execute(allyToTarget);
            // Select Health Potion Button after ally select panel
            GameMenuManager.gameMenuManager.healthPotionButton.Select();
        }
        else if(itemToUse == "Stone"){
            //stone(player);
            StoneItem.stoneItem.execute(player);
        }
        if (!DataManager.Junak.itemManager.isInCombat) {
            if (!DataManager.Junak.itemManager.aquiredItems.Contains(itemToUse)) {
                GameMenuManager.gameMenuManager.itemReturnButton.Select();
            }
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
