using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// The states of combat, the phases
public enum CombatStates {PlayerOneAttacking, EnemyAttacking, ResetValues, PlayerWon, EnemyWon}
// The actions a player can take on their turn/order of combat
public enum CombatOptions { Attack, Ability, Item, Run, HasNotChosen}

public class CombatManager : MonoBehaviour { 

    // Reference to the enemy sprite
    public GameObject enemySprite;
    // Reference to the CombatStates enum, used for deciding what part of combat it is
    CombatStates combatState;
    // Reference to the playerOneOption enum, used for assigning the chosen option
    CombatOptions playerOneOption;
    // The current order or turn of the combat
    private int order;
    // order to reset values
    private int orderToReset;
    // The order playerOne chose to act
    private int playerOneChosenOrder;
    // Boolean that represents whether the enemy has attacked
    private bool enemyOneHasAttacked;
    // Boolean that represents whether the win text was prompt
    private bool winTextHasBeenPrompt;

    // Ability Select Panel
    
    // IconOne image
    public GameObject iconOne;
    // IconTwo image
    public GameObject iconTwo;
    // Icon sprites
    public Sprite playerOneIcon;
    public Sprite enemyIcon;

    // Runs before Awake
    private void Awake() {
        // Get a reference to the CombatTextManager, so it exists
        CombatTextManager.combatTextManager = GameObject.Find("CombatTextManager").GetComponent<CombatTextManager>();
        // Get a reference to the combatMenuManager
        CombatMenuManager.combatMenuManager = GameObject.Find("CombatMenuManager").GetComponent<CombatMenuManager>();
    }

    // Used to set all the values to default at the start of combat
    void Start() {

        DataManager.playerOne.itemManager.isInCombat = true;
        DataManager.playerOne.abilityManager.abilityToUse = "";
        DataManager.playerOne.itemManager.itemToUse = "";
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
        playerOneChosenOrder = 0;
        // PlayerOne has not chosen an action
        playerOneOption = CombatOptions.HasNotChosen;
        // Display the text that is shown at the beginning of an encounter and wait for key press to continue
        CombatTextManager.combatTextManager.ManageText("A " + EnemyDataManager.EnemyManager.currentType + " appeared!");
        CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
        // Assign the enemySprite
        enemySprite = GameObject.Find("Enemy");
        enemySprite.GetComponent<SpriteRenderer>().sprite = EnemyDataManager.EnemyManager.currentSprite;

        // Assign the icons
        iconOne = GameObject.Find("IconOne");
        iconTwo = GameObject.Find("IconTwo");
        // The enemy has not attacked
        enemyOneHasAttacked = false;
        
        // Get the order that combtatants will act on
        determineOrder(); 
    }
    // Method for the Run option
    private void Run(DataManager player){
        // run away animation
        if(player.speed > EnemyDataManager.EnemyManager.speed){
            DataManager.playerOne.isBeingLoaded = true;
            DataManager.playerOne.ranFromCombat = true;
            SceneManager.LoadScene(EnemyDataManager.EnemyManager.theScene);
        }
        else{
            CombatTextManager.combatTextManager.ManageText("Failed to run");
            CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
            order += 1;
        }
    }
    private void resetPlayerOneValues(){
        playerOneOption = CombatOptions.HasNotChosen;
        playerOneChosenOrder = 0;
        DataManager.playerOne.abilityManager.abilityToUse = "";
        DataManager.playerOne.itemManager.itemToUse = "";
    }
    // Order is determined by speed
    private void determineOrder() {
        if (DataManager.playerOne.speed > EnemyDataManager.EnemyManager.speed) {
            combatState = CombatStates.PlayerOneAttacking;
            EnemyDataManager.EnemyManager.assignedOrderInCombat = 2;
            DataManager.playerOne.assignedOrderInCombat = 1;
            iconOne.GetComponent<SpriteRenderer>().sprite = playerOneIcon;
            iconTwo.GetComponent<SpriteRenderer>().sprite = EnemyDataManager.EnemyManager.currentSprite;
        }
        else {
            combatState = CombatStates.EnemyAttacking;
            EnemyDataManager.EnemyManager.assignedOrderInCombat = 1;
            DataManager.playerOne.assignedOrderInCombat = 2;
            iconOne.GetComponent<SpriteRenderer>().sprite = EnemyDataManager.EnemyManager.currentSprite;
            iconTwo.GetComponent<SpriteRenderer>().sprite = playerOneIcon;
        }
        orderToReset = 3;
    }

    // Gets the player's chosen action 
    private void GetPlayerAction(DataManager player) {
        // If the previous text is finished, the text has not been prompt, and the user pressed space, display "Choose an Action" 
        if (CombatTextManager.combatTextManager.textIsFinished && !CombatTextManager.combatTextManager.textHasBeenPrompt && CombatTextManager.combatTextManager.pressedSpace) {         
            CombatTextManager.combatTextManager.ManageText("Choose an Action");
            CombatTextManager.combatTextManager.textHasBeenPrompt = true;
        }
        // Assign playerOneOption if it is HasNotChosen based on input, followed by displaying "Choose Order To Act"
        if (CombatTextManager.combatTextManager.textHasBeenPrompt && CombatTextManager.combatTextManager.textIsFinished) {
            if(player = DataManager.playerOne){
                if (Input.GetKeyDown(KeyCode.Q) && playerOneOption == CombatOptions.HasNotChosen) {
                    playerOneOption = CombatOptions.Attack;
                    CombatTextManager.combatTextManager.ManageText("Choose Order To Act");
                }
                if (Input.GetKeyDown(KeyCode.W) && playerOneOption == CombatOptions.HasNotChosen) {
                    CombatMenuManager.combatMenuManager.abilitySelectPanel.SetActive(true);
                    CombatMenuManager.combatMenuManager.abilityReturnButton.Select();           
                    CombatTextManager.combatTextManager.ManageText("Choose Order To Act");                               
                }
                if (Input.GetKeyDown(KeyCode.E) && playerOneOption == CombatOptions.HasNotChosen){
                    CombatMenuManager.combatMenuManager.whenTurnedOn();
                    CombatTextManager.combatTextManager.ManageText("Choose Order To Act");
                }
                if (Input.GetKeyDown(KeyCode.R) && playerOneOption == CombatOptions.HasNotChosen){
                    Run(DataManager.playerOne);
                }

                if (DataManager.playerOne.abilityManager.abilityToUse != ""){
                    playerOneOption = CombatOptions.Ability;
                }
                if (DataManager.playerOne.itemManager.itemToUse != ""){
                    playerOneOption = CombatOptions.Item;
                }
            }
            
        }
    }
        

    // Method for subtracting enemy health
    private void Attack(EnemyDataManager enemy, DataManager character) {     
        CombatTextManager.combatTextManager.ManageText("Player does " + character.qDamage.ToString() + " damage!");
        //TODO: play enemy sprite damaged animation
        CombatTextManager.combatTextManager.damageText.text = "-" + character.qDamage.ToString();
        CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.FadeText(CombatTextManager.combatTextManager.damageText));
        CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
        enemy.health -= character.qDamage;
    }

    // Method for getting the player's chosen order to act
    private void GetOrder(DataManager player) {    
        int theOrder = 0;
        
        if (Input.GetKeyDown(KeyCode.Alpha1) && CombatTextManager.combatTextManager.textIsFinished) {        
            CombatTextManager.combatTextManager.ManageText("Order is One");
            CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
            theOrder = 1;                       
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && CombatTextManager.combatTextManager.textIsFinished) {       
            CombatTextManager.combatTextManager.ManageText("Order is Two");
            CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
            theOrder = 2;
        }
        if(player == DataManager.playerOne) {        
            playerOneChosenOrder = theOrder;
        }
    }
        
    // TODO: Get Target Method

    // Handles all actions that happen on the order
    private void HandleOrder(DataManager player) { 

        //Debug.Log("Handling Order " + order.ToString() + " player selected order is " + playerOneChosenOrder.ToString());   
        if (player == DataManager.playerOne) {        
            if (playerOneChosenOrder != order && CombatTextManager.combatTextManager.textIsFinished && CombatTextManager.combatTextManager.pressedSpace && playerOneOption != CombatOptions.HasNotChosen && playerOneChosenOrder != 0) { 
                order += 1;
            }
            else if (playerOneChosenOrder == order && CombatTextManager.combatTextManager.textIsFinished && CombatTextManager.combatTextManager.pressedSpace && playerOneOption != CombatOptions.HasNotChosen && playerOneChosenOrder != 0) {            
                if (playerOneOption == CombatOptions.Attack) {          
                    Attack(EnemyDataManager.EnemyManager, DataManager.playerOne);
                }
                else if (playerOneOption == CombatOptions.Ability){
                    DataManager.playerOne.abilityManager.useAbility();
                }
                else if(playerOneOption == CombatOptions.Item){
                    DataManager.playerOne.itemManager.useItem(DataManager.playerOne);
                    CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
                }
                order += 1;
            }
            else if (playerOneChosenOrder != order && CombatTextManager.combatTextManager.textIsFinished && CombatTextManager.combatTextManager.pressedSpace && playerOneOption == CombatOptions.HasNotChosen && playerOneChosenOrder == 0){
                order += 1;
            }
        }
    }

    void Update() {

        
        switch (combatState) {        
            case CombatStates.PlayerOneAttacking:
                if (playerOneOption == CombatOptions.HasNotChosen ) {                 
                    GetPlayerAction(DataManager.playerOne);
                }
                if (playerOneOption != CombatOptions.HasNotChosen && playerOneChosenOrder == 0) {                 
                    GetOrder(DataManager.playerOne);
                }
                if (playerOneOption != CombatOptions.HasNotChosen && playerOneChosenOrder != 0){
                    HandleOrder(DataManager.playerOne);
                }
                
                if(order == EnemyDataManager.EnemyManager.assignedOrderInCombat) {                
                    combatState = CombatStates.EnemyAttacking;

                }
                else if (order == orderToReset){
                    combatState = CombatStates.ResetValues;
                }
                if(EnemyDataManager.EnemyManager.health <= 0) {
                    combatState = CombatStates.PlayerWon;
                }
                break;
            case CombatStates.EnemyAttacking:
                if (CombatTextManager.combatTextManager.textIsFinished && CombatTextManager.combatTextManager.pressedSpace && !enemyOneHasAttacked) {              
                    EnemyDataManager.EnemyManager.theMonster.Attack(DataManager.playerOne);
                    enemyOneHasAttacked = true;
                }
                if(CombatTextManager.combatTextManager.pressedSpace && CombatTextManager.combatTextManager.textIsFinished && !EnemyDataManager.EnemyManager.theMonster.displayedDamage) {                    
                    EnemyDataManager.EnemyManager.theMonster.DisplayDamage(DataManager.playerOne);
                }

                HandleOrder(DataManager.playerOne);

                if(order == DataManager.playerOne.assignedOrderInCombat && CombatTextManager.combatTextManager.textIsFinished){
                    resetPlayerOneValues();
                    combatState = CombatStates.PlayerOneAttacking;
                }
                else if(order == orderToReset) {
                    combatState = CombatStates.ResetValues;
                }
                if(DataManager.playerOne.health <= 0) {
                    combatState = CombatStates.EnemyWon;
                }

                break;
            case CombatStates.ResetValues:
                order = 1;
                enemyOneHasAttacked = false;
                CombatTextManager.combatTextManager.textHasBeenPrompt = false;
                EnemyDataManager.EnemyManager.theMonster.displayedDamage = false;
                EnemyDataManager.EnemyManager.theMonster.textWasPrompt = false;
                if(order == DataManager.playerOne.assignedOrderInCombat){ 
                    resetPlayerOneValues();
                    combatState = CombatStates.PlayerOneAttacking; 
                }
                else if(order == EnemyDataManager.EnemyManager.assignedOrderInCombat){ 
                    combatState = CombatStates.EnemyAttacking; 
                }             
                break;
            case CombatStates.PlayerWon:
                if (CombatTextManager.combatTextManager.textIsFinished && CombatTextManager.combatTextManager.pressedSpace && !winTextHasBeenPrompt) {
                    CombatTextManager.combatTextManager.ManageText("You Win!");
                    CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
                    winTextHasBeenPrompt = true;
                }
                if(CombatTextManager.combatTextManager.pressedSpace && winTextHasBeenPrompt && CombatTextManager.combatTextManager.textIsFinished) {
                    DataManager.playerOne.isBeingLoaded = true;
                    EnemyDataManager.EnemyManager.defeatedEnemies.Add(EnemyDataManager.EnemyManager.currentName);
                    DataManager.playerOne.addExperience(EnemyDataManager.EnemyManager.experienceGives);
                    DataManager.playerOne.itemManager.isInCombat = false;
                    SceneManager.LoadScene(EnemyDataManager.EnemyManager.theScene);
                }
                break;
            case CombatStates.EnemyWon:
                if (CombatTextManager.combatTextManager.textIsFinished && CombatTextManager.combatTextManager.pressedSpace && !winTextHasBeenPrompt) {
                    CombatTextManager.combatTextManager.ManageText("You Lost!");
                    CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
                    winTextHasBeenPrompt = true;
                }
                if (CombatTextManager.combatTextManager.pressedSpace && winTextHasBeenPrompt) {
                    DataManager.playerOne.itemManager.isInCombat = false;
                    SceneManager.LoadScene("MainMenu");
                }
                    break;
            default:
                break;

        }
        // Show player's health 
        CombatTextManager.combatTextManager.playerOneHealthText.text = DataManager.playerOne.health.ToString();
        CombatTextManager.combatTextManager.orderText.text = order.ToString();
    }
}
