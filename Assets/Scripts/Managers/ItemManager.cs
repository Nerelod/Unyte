using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public List<string> aquiredItems = new List<string>();
    public List<ItemScript> itemScripts = new List<ItemScript>();
    public List<string> itemsThatWereRemoved = new List<string>();
    // Items that require an ally to be chosen
    public List<string> allyItems = new List<string>();

    public ItemScript itemToUse;
    public DataManager allyToTarget;
    public bool isInCombat;
    public bool choseItemInCombat;
    void Start()
    {
        isInCombat = false;
        allyItems.Add("Health Potion");
        JunakDataManager.Junak.itemManager.itemScripts.Add(StoneItem.stoneItem);
        JunakDataManager.Junak.itemManager.itemScripts.Add(HealthPotionItem.healthPotionItem);
    }

    public void selectItemOutsideCombat(string itemName)
    {
        if (JunakDataManager.Junak.itemManager.aquiredItems.Contains(itemName))
        {
            int itemIndex = JunakDataManager.Junak.itemManager.itemScripts.FindIndex(a => a.name == itemName);
            if (allyItems.Contains(itemName))
            {
                GameMenuManager.gameMenuManager.canInteractWith = false;
                GameMenuManager.gameMenuManager.allySelectPanelWhenTurnedOn();
            }
            JunakDataManager.Junak.itemManager.itemToUse = JunakDataManager.Junak.itemManager.itemScripts[itemIndex];
        }
    }
    public void selectItemInCombat(string itemName)
    {
        int itemIndex = JunakDataManager.Junak.itemManager.itemScripts.FindIndex(a => a.name == itemName);
        if (JunakDataManager.Junak.isTurnInCombat)
        {
            if (JunakDataManager.Junak.itemManager.aquiredItems.Contains(itemName))
            {
                JunakDataManager.Junak.itemManager.itemToUse = JunakDataManager.Junak.itemManager.itemScripts[itemIndex];
                JunakDataManager.Junak.itemManager.choseItemInCombat = true;
                CombatMenuManager.combatMenuManager.itemSelectPanel.SetActive(false);
                if (JunakDataManager.Junak.itemManager.itemToUse.requiresEnemyTarget) { CombatMenuManager.combatMenuManager.enemySelectPanelWhenTurnedOn(); }
            }
        }
        else if (SaralfDataManager.Saralf.isTurnInCombat)
        {
            if (JunakDataManager.Junak.itemManager.aquiredItems.Contains(itemName))
            {
                SaralfDataManager.Saralf.itemManager.itemToUse = JunakDataManager.Junak.itemManager.itemScripts[itemIndex];
                SaralfDataManager.Saralf.itemManager.choseItemInCombat = true;
                CombatMenuManager.combatMenuManager.itemSelectPanel.SetActive(false);
                if (SaralfDataManager.Saralf.itemManager.itemToUse.requiresEnemyTarget) { CombatMenuManager.combatMenuManager.enemySelectPanelWhenTurnedOn(); }
            }
        }
        if (JunakDataManager.Junak.itemManager.aquiredItems.Contains(itemName))
        {
            if (allyItems.Contains(itemName))
            {
                CombatMenuManager.combatMenuManager.allySelectPanelWhenTurnedOn();
            }

        }
        JunakDataManager.Junak.itemManager.aquiredItems.Remove(itemName);
    }
    public void selectAlly(string allyName)
    {
        if (allyName == "Junak")
        {
            if (JunakDataManager.Junak.isTurnInCombat) { JunakDataManager.Junak.itemManager.allyToTarget = JunakDataManager.Junak; }
            else if (SaralfDataManager.Saralf.isTurnInCombat) { SaralfDataManager.Saralf.itemManager.allyToTarget = JunakDataManager.Junak; }
        }
        else if (allyName == "Saralf")
        {
            if (JunakDataManager.Junak.isTurnInCombat) { JunakDataManager.Junak.itemManager.allyToTarget = SaralfDataManager.Saralf; }
            else if (SaralfDataManager.Saralf.isTurnInCombat) { SaralfDataManager.Saralf.itemManager.allyToTarget = SaralfDataManager.Saralf; }
        }
        CombatMenuManager.combatMenuManager.allySelectPanel.SetActive(false);
    }
    public void useItem(DataManager player)
    {
        if (JunakDataManager.Junak.itemManager.allyItems.Contains(itemToUse.name))
        {
            itemToUse.execute(allyToTarget);
            // Select Health Potion Button after ally select panel
            GameMenuManager.gameMenuManager.healthPotionButton.Select();
        }
        else
        {
            itemToUse.execute(player);
        }
        if (!JunakDataManager.Junak.itemManager.isInCombat)
        {
            if (!JunakDataManager.Junak.itemManager.aquiredItems.Contains(itemToUse.name))
            {
                GameMenuManager.gameMenuManager.itemReturnButton.Select();
            }
        }

    }
    public void turnOffItemSelect()
    {
        CombatMenuManager.combatMenuManager.itemSelectPanel.SetActive(false);
        CombatTextManager.combatTextManager.textHasBeenPrompt = false;
    }
    public int getAmountOfItem(string itemToCount)
    {
        int amount = 0;
        foreach (string item in aquiredItems)
        {
            if (item == itemToCount)
            {
                amount++;
            }
        }
        return amount;
    }
    void Update()
    {

    }
}
