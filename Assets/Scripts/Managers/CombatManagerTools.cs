using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatManagerTools
{
    // current order in combat
    public int order;
    // order to reset values
    public int orderToReset;
    // The order playerOne chose to act
    public int junakChosenOrder;
    // The Order Saralf will act
    public int saralfChosenOrder;
    // Boolean that represents whether the combatant has attacked/been handled
    public bool enemyOneHasAttacked;
    public bool enemyTwoHasAttacked;
    public bool junakHandled;
    public bool saralfHandled;
    // Boolean that represents whether the win text was prompt
    public bool winTextHasBeenPrompt;

    public bool thereWasDeath;

    public List<DataManager> combatants = new List<DataManager>();
    public List<DataManager> partymembers = new List<DataManager>();
    public List<DataManager> livingPartyMembers;

    public int whichtarget;

    public int amountDead;
    public int deadEnemies;
    
    public void initiateTools() {
        thereWasDeath = false;
        amountDead = 0;
        deadEnemies = 0;
        JunakDataManager.Junak.isTurnInCombat = false;
        SaralfDataManager.Saralf.isTurnInCombat = false;
        junakHandled = saralfHandled = false;
        JunakDataManager.Junak.itemManager.isInCombat = true;
        JunakDataManager.Junak.abilityManager.choseAbilityInCombat = false;
        JunakDataManager.Junak.itemManager.itemToUse = null;
        JunakDataManager.Junak.itemManager.choseItemInCombat = false;
        SaralfDataManager.Saralf.itemManager.isInCombat = true;
        SaralfDataManager.Saralf.abilityManager.choseAbilityInCombat = false;
        SaralfDataManager.Saralf.itemManager.itemToUse = null;
        SaralfDataManager.Saralf.itemManager.choseItemInCombat = false;
        // Set the abilityselectpanel off
        CombatMenuManager.combatMenuManager.abilitySelectPanel.SetActive(false);
        // Win text is false, has not been displayed
        winTextHasBeenPrompt = false;
        // Make the damage text empty
        CombatTextManager.combatTextManager.damageText.text = "";
        // Begin the order at 1
        order = 1;
        // No text has been prompt
        CombatTextManager.combatTextManager.textHasBeenPrompt = false;
        // Space has not been pressed
        CombatTextManager.combatTextManager.pressedSpace = false;
        // PlayerOne has not chosen an order to act
        junakChosenOrder = 0;
        saralfChosenOrder = 0;
        // PlayerOne has not chosen an action
        JunakDataManager.Junak.combatOption = CombatOptions.HasNotChosen;
        SaralfDataManager.Saralf.combatOption = CombatOptions.HasNotChosen;
        // Display the text that is shown at the beginning of an encounter and wait for key press to continue
        CombatTextManager.combatTextManager.ManageText("A " + EnemyDataManager.EnemyManager.currentType + " appeared!");
        CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());

        // The enemy has not attacked
        enemyOneHasAttacked = false;
        enemyTwoHasAttacked = false;
        // Get the order that combtatants will act on
        getCombatMembers();
        getPartyMembers();
    }
    public void Run(DataManager player) {
        // run away animation
        if (player.speed > EnemyDataManager.EnemyManager.speed) {
            JunakDataManager.Junak.isBeingLoaded = true;
            JunakDataManager.Junak.ranFromCombat = true;
            SceneManager.LoadScene(EnemyDataManager.EnemyManager.theScene);
        }
        else {
            CombatTextManager.combatTextManager.ManageText("Failed to run");
            CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
            order += 1;
        }
    }
    // Returns amount of combatants
    public int getCombatantsAmount() {
        int members = 1;
        if (SaralfDataManager.Saralf.isInParty) { members += 1; }
        members += EnemyDataManager.EnemyManager.amountOfEnemies;
        return members;
    }
    // Adds all combatants to combatants
    public void getCombatMembers() {
        combatants.Add(JunakDataManager.Junak);
        combatants.Add(EnemyDataManager.EnemyManager);
        if (SaralfDataManager.Saralf.isInParty) { combatants.Add(SaralfDataManager.Saralf); }
        if (EnemyDataManager.EnemyManager.amountOfEnemies >= 2) { combatants.Add(EnemyDataManagerTwo.EnemyManagerTwo); }
    }
    // Adds party members to partymembers
    public void getPartyMembers() {
        partymembers.Add(JunakDataManager.Junak);
        if (SaralfDataManager.Saralf.isInParty) {
            partymembers.Add(SaralfDataManager.Saralf);
        }
        livingPartyMembers = partymembers;
    }
    // Returns amount of every living combatant
    public int getLivingCombatantsAmount() {
        int amountAlive = 0;
        foreach (DataManager combatant in combatants) {
            if (combatant.health > 0) {
                amountAlive += 1;
            }
        }
        return amountAlive;
    }

}
