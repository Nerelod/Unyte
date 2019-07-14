using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// The states of combat, the phases
public enum CombatStates {PlayerOneAttacking, SaralfAttacking, EnemyAttacking, ResetValues, PlayerWon, EnemyWon}
// The actions a player can take on their turn/order of combat
public enum CombatOptions { Attack, Ability, Item, Run, HasNotChosen}

public class CombatManager : MonoBehaviour {

    // Reference to sprite representing who is going in combat
    public GameObject combatCharSprite;
    // Combatant Sprites                              
    public Sprite playerOneCombatantSprite;
    public Sprite SaralfCombatantSprite;
    // Reference to the enemy sprite
    public GameObject enemySprite;
    // Reference to the CombatStates enum, used for deciding what part of combat it is
    CombatStates combatState;
    // Reference to the playerOneOption enum, used for assigning the chosen option
    CombatOptions playerOneOption;
    // Saralf's Option
    CombatOptions SaralfOption;
    // The current order or turn of the combat
    private int order;
    // order to reset values
    private int orderToReset;
    // The order playerOne chose to act
    private int playerOneChosenOrder;
    // The Order Saralf will act
    private int SaralfChosenOrder;
    // Boolean that represents whether the combatant has attacked/been handled
    private bool enemyOneHasAttacked;
    private bool playerOneHandled;
    private bool SaralfHandled;
    // Boolean that represents whether the win text was prompt
    private bool winTextHasBeenPrompt;

    public Text textIconOne;
    public Text textIconTwo;
    public Text textIconThree;
    
    // IconOne image
    public GameObject iconOne;
    // IconTwo image
    public GameObject iconTwo;
    //Icon Three image
    public GameObject iconThree;
    // Icon sprites
    public Sprite playerOneIcon;
    public Sprite saralfIcon;
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

        playerOneHandled = SaralfHandled = false;
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
        SaralfChosenOrder = 0;
        // PlayerOne has not chosen an action
        playerOneOption = CombatOptions.HasNotChosen;
        SaralfOption = CombatOptions.HasNotChosen;
        // Display the text that is shown at the beginning of an encounter and wait for key press to continue
        CombatTextManager.combatTextManager.ManageText("A " + EnemyDataManager.EnemyManager.currentType + " appeared!");
        CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
        // Assign the enemySprite
        enemySprite = GameObject.Find("Enemy");
        enemySprite.GetComponent<SpriteRenderer>().sprite = EnemyDataManager.EnemyManager.currentSprite;

        // Assign the icons
        iconOne = GameObject.Find("IconOne");
        iconTwo = GameObject.Find("IconTwo");
        iconThree = GameObject.Find("IconThree");
        // The enemy has not attacked
        enemyOneHasAttacked = false;
        
        // Get the order that combtatants will act on
        determineOrder(); 
    }
    // Method for checking if CombatTextManager finished what it does
    private bool isTextManagerDone() {
        if(CombatTextManager.combatTextManager.textIsFinished && CombatTextManager.combatTextManager.pressedSpace) {
            return true;
        }
        else {
            return false;
        }
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
        CombatTextManager.combatTextManager.textHasBeenPrompt = false;
        playerOneChosenOrder = 0;
        playerOneHandled = false;
        SaralfHandled = false;
        DataManager.playerOne.abilityManager.abilityToUse = "";
        DataManager.playerOne.itemManager.itemToUse = "";
    }
    private void resetSaralfOptions() {
        SaralfOption = CombatOptions.HasNotChosen;
        CombatTextManager.combatTextManager.textHasBeenPrompt = false;
        SaralfChosenOrder = 0;
        SaralfHandled = false;
        playerOneHandled = false;
        //SaralfDataManager.Saralf.abilityManager.abilityToUse = "";
        //SaralfDataManager.Saralf.itemManager.itemToUse = "";
    }
    private int getCombatMembers() {
        int members = 2;
        if (SaralfDataManager.Saralf.isInParty) {
            members += 1;
        }
        return members;
    }
    // Order is determined by speed
    private void determineOrder() {

        int[] speeds = new int[getCombatMembers()];
        speeds[0] = DataManager.playerOne.speed;
        speeds[1] = EnemyDataManager.EnemyManager.speed;
        if (SaralfDataManager.Saralf.isInParty) {
            speeds[2] = SaralfDataManager.Saralf.speed;
        }
        Array.Sort(speeds);
        Array.Reverse(speeds);
        DataManager.playerOne.assignedOrderInCombat = Array.IndexOf(speeds, DataManager.playerOne.speed) + 1;
        SaralfDataManager.Saralf.assignedOrderInCombat = Array.IndexOf(speeds, SaralfDataManager.Saralf.speed) + 1;
        EnemyDataManager.EnemyManager.assignedOrderInCombat = Array.IndexOf(speeds, EnemyDataManager.EnemyManager.speed) + 1;

        if (DataManager.playerOne.assignedOrderInCombat == 1) { // if playerOne is first
            combatState = CombatStates.PlayerOneAttacking;
            iconOne.GetComponent<SpriteRenderer>().sprite = playerOneIcon;
            CombatTextManager.combatTextManager.playerOneHealthText = textIconOne;
            if (EnemyDataManager.EnemyManager.assignedOrderInCombat == 2) {
                iconTwo.GetComponent<SpriteRenderer>().sprite = EnemyDataManager.EnemyManager.currentSprite;
                CombatTextManager.combatTextManager.enemyHealthText = textIconTwo;
            }
            else if(EnemyDataManager.EnemyManager.assignedOrderInCombat == 3) {
                iconThree.GetComponent<SpriteRenderer>().sprite = EnemyDataManager.EnemyManager.currentSprite;
                CombatTextManager.combatTextManager.enemyHealthText = textIconThree;
            }
            if (SaralfDataManager.Saralf.assignedOrderInCombat == 2) {
                iconTwo.GetComponent<SpriteRenderer>().sprite = saralfIcon;
                CombatTextManager.combatTextManager.saralfHealthText = textIconTwo;
            }
            else if(SaralfDataManager.Saralf.assignedOrderInCombat == 3) {
                iconThree.GetComponent<SpriteRenderer>().sprite = saralfIcon;
                CombatTextManager.combatTextManager.saralfHealthText = textIconThree;
            }
        }
        else if(EnemyDataManager.EnemyManager.assignedOrderInCombat == 1){ // if enemy is first
            combatState = CombatStates.EnemyAttacking;
            iconOne.GetComponent<SpriteRenderer>().sprite = EnemyDataManager.EnemyManager.currentSprite;
            CombatTextManager.combatTextManager.enemyHealthText = textIconOne;
            if (DataManager.playerOne.assignedOrderInCombat == 2) {
                iconTwo.GetComponent<SpriteRenderer>().sprite = playerOneIcon;
                CombatTextManager.combatTextManager.playerOneHealthText = textIconTwo;
            }
            else if(DataManager.playerOne.assignedOrderInCombat == 3) {
                iconThree.GetComponent<SpriteRenderer>().sprite = playerOneIcon;
                CombatTextManager.combatTextManager.playerOneHealthText = textIconThree;
            }
            if(SaralfDataManager.Saralf.assignedOrderInCombat == 2) {
                iconTwo.GetComponent<SpriteRenderer>().sprite = saralfIcon;
                CombatTextManager.combatTextManager.saralfHealthText = textIconTwo;
            }
            else if (SaralfDataManager.Saralf.assignedOrderInCombat == 3) {
                iconThree.GetComponent<SpriteRenderer>().sprite = saralfIcon;
                CombatTextManager.combatTextManager.saralfHealthText = textIconThree;
            }
        }
        else if(SaralfDataManager.Saralf.assignedOrderInCombat == 1) { // if Saralf is first
            combatState = CombatStates.SaralfAttacking;
            iconOne.GetComponent<SpriteRenderer>().sprite = saralfIcon;
            CombatTextManager.combatTextManager.saralfHealthText = textIconOne;
            if (DataManager.playerOne.assignedOrderInCombat == 2) {
                iconTwo.GetComponent<SpriteRenderer>().sprite = playerOneIcon;
                CombatTextManager.combatTextManager.playerOneHealthText = textIconTwo;
            }
            else if (DataManager.playerOne.assignedOrderInCombat == 3) {
                iconThree.GetComponent<SpriteRenderer>().sprite = playerOneIcon;
                CombatTextManager.combatTextManager.playerOneHealthText = textIconThree;
            }
            if (EnemyDataManager.EnemyManager.assignedOrderInCombat == 2) {
                iconTwo.GetComponent<SpriteRenderer>().sprite = EnemyDataManager.EnemyManager.currentSprite;
                CombatTextManager.combatTextManager.enemyHealthText = textIconTwo;
            }
            else if (EnemyDataManager.EnemyManager.assignedOrderInCombat == 3) {
                iconThree.GetComponent<SpriteRenderer>().sprite = EnemyDataManager.EnemyManager.currentSprite;
                CombatTextManager.combatTextManager.enemyHealthText = textIconThree;
            }
        }
        CombatTextManager.combatTextManager.enemyHealthText.text = "?";
        orderToReset = getCombatMembers() + 1;
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
            if(player == DataManager.playerOne){
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
            else if(player == SaralfDataManager.Saralf) {
                if (Input.GetKeyDown(KeyCode.Q) && SaralfOption == CombatOptions.HasNotChosen) {
                    SaralfOption = CombatOptions.Attack;
                    CombatTextManager.combatTextManager.ManageText("Choose Order To Act");
                }
                /*if (Input.GetKeyDown(KeyCode.W) && SaralfOption == CombatOptions.HasNotChosen) {
                    CombatMenuManager.combatMenuManager.abilitySelectPanel.SetActive(true);
                    CombatMenuManager.combatMenuManager.abilityReturnButton.Select();
                    CombatTextManager.combatTextManager.ManageText("Choose Order To Act");
                }*/                                                                                                         
                /*if (Input.GetKeyDown(KeyCode.E) && SaralfOption == CombatOptions.HasNotChosen) {
                    CombatMenuManager.combatMenuManager.whenTurnedOn();
                    CombatTextManager.combatTextManager.ManageText("Choose Order To Act");
                }*/
                if (Input.GetKeyDown(KeyCode.R) && SaralfOption == CombatOptions.HasNotChosen) {
                    Run(SaralfDataManager.Saralf);
                }

                if (DataManager.playerOne.abilityManager.abilityToUse != "") {
                    playerOneOption = CombatOptions.Ability;
                }
                if (DataManager.playerOne.itemManager.itemToUse != "") {
                    playerOneOption = CombatOptions.Item;
                }
                /*if (SaralfDataManager.Saralf.abilityManager.abilityToUse != "") {
                    SaralfOption = CombatOptions.Ability;
                }
                if (SaralfDataManager.Saralf.itemManager.itemToUse != "") {
                    SaralfOption = CombatOptions.Item;
                }*/
            }
        }
    }
        

    // Method for subtracting enemy health
    private void Attack(EnemyDataManager enemy, DataManager character) {     
        CombatTextManager.combatTextManager.ManageText( character.theName + " does " + character.qDamage.ToString() + " damage!");
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
        else if (Input.GetKeyDown(KeyCode.Alpha3) && CombatTextManager.combatTextManager.textIsFinished) {
            CombatTextManager.combatTextManager.ManageText("Order is Three");
            CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
            theOrder = 3;
        }
        if(player == DataManager.playerOne) {        
            playerOneChosenOrder = theOrder;
        }
        else if (player == SaralfDataManager.Saralf) {
            SaralfChosenOrder = theOrder;
        }
    }
        
    // TODO: Get Target Method

    // Handles all actions that happen on the order
    private void HandleOrder(DataManager player) { 
 
        if (player == DataManager.playerOne && isTextManagerDone()) {
            if (playerOneChosenOrder == order && playerOneOption != CombatOptions.HasNotChosen && playerOneChosenOrder != 0) {
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
                
            }
            playerOneHandled = true;
        }
        else if(player == SaralfDataManager.Saralf && isTextManagerDone()) {
            if (SaralfChosenOrder == order && isTextManagerDone() && SaralfOption != CombatOptions.HasNotChosen && SaralfChosenOrder != 0) {
                if (SaralfOption == CombatOptions.Attack) {
                    Attack(EnemyDataManager.EnemyManager, SaralfDataManager.Saralf);
                }
                /*else if (SaralfOption == CombatOptions.Ability) {
                    SaralfDataManager.Saralf.abilityManager.useAbility();
                }
                else if (SaralfOption == CombatOptions.Item) {
                    SaralfDataManager.Saralf.itemManager.useItem(SaralfDataManager.Saralf);
                    CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
                }*/
                
            }
            SaralfHandled = true;
            
        }
        if(SaralfHandled && playerOneHandled) { order += 1; Debug.Log("Adding to Order"); }
    }

    void Update() {

        
        switch (combatState) {        
            case CombatStates.PlayerOneAttacking:
                combatCharSprite.GetComponent<SpriteRenderer>().sprite = playerOneCombatantSprite;
                if (playerOneOption == CombatOptions.HasNotChosen ) {                 
                    GetPlayerAction(DataManager.playerOne);
                }
                if (playerOneOption != CombatOptions.HasNotChosen && playerOneChosenOrder == 0) {                 
                    GetOrder(DataManager.playerOne);
                }
                if (playerOneOption != CombatOptions.HasNotChosen && playerOneChosenOrder != 0 && !playerOneHandled && isTextManagerDone()){
                    HandleOrder(DataManager.playerOne);
                }
                if (!SaralfHandled && isTextManagerDone() && playerOneHandled) {
                    HandleOrder(SaralfDataManager.Saralf);
                    
                }
                
                if(order == EnemyDataManager.EnemyManager.assignedOrderInCombat) {
                    playerOneHandled = false;
                    SaralfHandled = false;
                    combatState = CombatStates.EnemyAttacking;
                }
                else if(order == SaralfDataManager.Saralf.assignedOrderInCombat) {
                    resetSaralfOptions();
                    combatState = CombatStates.SaralfAttacking;
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
                if(isTextManagerDone() && !EnemyDataManager.EnemyManager.theMonster.displayedDamage) {                    
                    EnemyDataManager.EnemyManager.theMonster.DisplayDamage(DataManager.playerOne);
                }

                if (enemyOneHasAttacked) {
                    if (!playerOneHandled) { HandleOrder(DataManager.playerOne); }
                    if (!SaralfHandled) { HandleOrder(SaralfDataManager.Saralf); }
                }


                if (order == DataManager.playerOne.assignedOrderInCombat && CombatTextManager.combatTextManager.textIsFinished) {
                    resetPlayerOneValues();
                    combatState = CombatStates.PlayerOneAttacking;
                }
                else if (order == SaralfDataManager.Saralf.assignedOrderInCombat && CombatTextManager.combatTextManager.textIsFinished) {
                    resetSaralfOptions();
                    combatState = CombatStates.SaralfAttacking;
                }
                else if (order == orderToReset) {
                    combatState = CombatStates.ResetValues;
                }
                if(DataManager.playerOne.health <= 0) {
                    combatState = CombatStates.EnemyWon;
                }

                break;
            case CombatStates.SaralfAttacking:
                combatCharSprite.GetComponent<SpriteRenderer>().sprite = SaralfCombatantSprite;
                if(SaralfOption == CombatOptions.HasNotChosen) {
                    GetPlayerAction(SaralfDataManager.Saralf);
                }
                if (SaralfOption != CombatOptions.HasNotChosen && SaralfChosenOrder == 0) {
                    GetOrder(SaralfDataManager.Saralf);
                }
                if (SaralfOption != CombatOptions.HasNotChosen && SaralfChosenOrder != 0 && !SaralfHandled && isTextManagerDone()) {
                    HandleOrder(SaralfDataManager.Saralf);
                }
                if (!playerOneHandled && isTextManagerDone() && SaralfHandled) {
                    Debug.Log("Handling playerOne on Saralf's turn");
                    HandleOrder(DataManager.playerOne);
                }
                if (order == EnemyDataManager.EnemyManager.assignedOrderInCombat) {
                    playerOneHandled = false;
                    SaralfHandled = false;
                    combatState = CombatStates.EnemyAttacking;

                }
                else if (order == orderToReset) {
                    combatState = CombatStates.ResetValues;
                }
                else if (order == DataManager.playerOne.assignedOrderInCombat) {
                    combatState = CombatStates.PlayerOneAttacking;
                }
                if (EnemyDataManager.EnemyManager.health <= 0) {
                    combatState = CombatStates.PlayerWon;
                }
                break;
            case CombatStates.ResetValues:
                order = 1;
                enemyOneHasAttacked = false;
                CombatTextManager.combatTextManager.textHasBeenPrompt = false;
                EnemyDataManager.EnemyManager.theMonster.displayedDamage = false;
                EnemyDataManager.EnemyManager.theMonster.textWasPrompt = false;
                if (order == DataManager.playerOne.assignedOrderInCombat) {
                    resetPlayerOneValues();
                    combatState = CombatStates.PlayerOneAttacking;
                }
                else if (order == SaralfDataManager.Saralf.assignedOrderInCombat) {
                    resetSaralfOptions();
                    combatState = CombatStates.SaralfAttacking;
                }
                else if (order == EnemyDataManager.EnemyManager.assignedOrderInCombat) {
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
        CombatTextManager.combatTextManager.saralfHealthText.text = SaralfDataManager.Saralf.health.ToString();
        CombatTextManager.combatTextManager.orderText.text = order.ToString();
    }
}
