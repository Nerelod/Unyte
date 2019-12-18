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
    [SerializeField] private bool playerOneHandled;
    private bool SaralfHandled;
    // Boolean that represents whether the win text was prompt
    private bool winTextHasBeenPrompt;

    private List<DataManager> partymembers = new List<DataManager>();
    private int whichtarget;

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

    private int amountDead;

    // Runs before Awake
    private void Awake() {
        // Get a reference to the CombatTextManager, so it exists
        CombatTextManager.combatTextManager = GameObject.Find("CombatTextManager").GetComponent<CombatTextManager>();
        // Get a reference to the combatMenuManager
        CombatMenuManager.combatMenuManager = GameObject.Find("CombatMenuManager").GetComponent<CombatMenuManager>();
    }

    // Used to set all the values to default at the start of combat
    void Start() {

        amountDead = 0;
        DataManager.Junak.isTurnInCombat = false;
        SaralfDataManager.Saralf.isTurnInCombat = false;
        playerOneHandled = SaralfHandled = false;
        DataManager.Junak.itemManager.isInCombat = true;
        DataManager.Junak.abilityManager.abilityToUse = "";
        DataManager.Junak.itemManager.itemToUse = "";
        SaralfDataManager.Saralf.itemManager.isInCombat = true;
        SaralfDataManager.Saralf.itemManager.itemToUse = "";
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
        textIconOne.text = "";
        textIconTwo.text = "";
        textIconThree.text = "";
        // The enemy has not attacked
        enemyOneHasAttacked = false;
        
        // Get the order that combtatants will act on
        determineOrder();
        getPartyMembers();
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
            DataManager.Junak.isBeingLoaded = true;
            DataManager.Junak.ranFromCombat = true;
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
        DataManager.Junak.abilityManager.abilityToUse = "";
        DataManager.Junak.itemManager.itemToUse = "";
    }
    private void resetSaralfOptions() {
        SaralfOption = CombatOptions.HasNotChosen;
        CombatTextManager.combatTextManager.textHasBeenPrompt = false;
        SaralfChosenOrder = 0;
        SaralfHandled = false;
        playerOneHandled = false;
        //SaralfDataManager.Saralf.abilityManager.abilityToUse = "";
        SaralfDataManager.Saralf.itemManager.itemToUse = "";
    }
    // Returns amount of combat Members
    private int getCombatMembers() {
        int members = 2;
        if (SaralfDataManager.Saralf.isInParty) {

            members += 1;
        }
        return members;
    }
    private void getPartyMembers() {
        partymembers.Add(DataManager.Junak);
        if (SaralfDataManager.Saralf.isInParty) {
            partymembers.Add(SaralfDataManager.Saralf);
        }
        Debug.Log(partymembers);
    }
    // Order is determined by speed
    private void determineOrder() {

        int[] speeds = new int[getCombatMembers()];
        speeds[0] = DataManager.Junak.speed;
        speeds[1] = EnemyDataManager.EnemyManager.speed;
        if (SaralfDataManager.Saralf.isInParty) {
            speeds[2] = SaralfDataManager.Saralf.speed;
            Debug.Log("Saralf in Party");
        }
        Array.Sort(speeds);
        Array.Reverse(speeds);
        DataManager.Junak.assignedOrderInCombat = Array.IndexOf(speeds, DataManager.Junak.speed) + 1;
        if (SaralfDataManager.Saralf.isInParty) { SaralfDataManager.Saralf.assignedOrderInCombat = Array.IndexOf(speeds, SaralfDataManager.Saralf.speed) + 1; }
        EnemyDataManager.EnemyManager.assignedOrderInCombat = Array.IndexOf(speeds, EnemyDataManager.EnemyManager.speed) + 1;

        if (DataManager.Junak.assignedOrderInCombat == 1) { // if playerOne is first
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
            if (DataManager.Junak.assignedOrderInCombat == 2) {
                iconTwo.GetComponent<SpriteRenderer>().sprite = playerOneIcon;
                CombatTextManager.combatTextManager.playerOneHealthText = textIconTwo;
            }
            else if(DataManager.Junak.assignedOrderInCombat == 3) {
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
            if (DataManager.Junak.assignedOrderInCombat == 2) {
                iconTwo.GetComponent<SpriteRenderer>().sprite = playerOneIcon;
                CombatTextManager.combatTextManager.playerOneHealthText = textIconTwo;
            }
            else if (DataManager.Junak.assignedOrderInCombat == 3) {
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
            if(player == DataManager.Junak){
                if (Input.GetKeyDown(KeyCode.Q) && playerOneOption == CombatOptions.HasNotChosen) {
                    playerOneOption = CombatOptions.Attack;
                    CombatTextManager.combatTextManager.ManageText("Choose Order To Act");
                }
                if (Input.GetKeyDown(KeyCode.W) && playerOneOption == CombatOptions.HasNotChosen) {
                    CombatMenuManager.combatMenuManager.abilityPanelWhenTurnedOn();      
                    CombatTextManager.combatTextManager.ManageText("Choose Order To Act");                               
                }
                if (Input.GetKeyDown(KeyCode.E) && playerOneOption == CombatOptions.HasNotChosen){
                    CombatMenuManager.combatMenuManager.itemPanelWhenTurnedOn();
                    CombatTextManager.combatTextManager.ManageText("Choose Order To Act");
                }
                if (Input.GetKeyDown(KeyCode.R) && playerOneOption == CombatOptions.HasNotChosen){
                    Run(DataManager.Junak);
                }

                if (DataManager.Junak.abilityManager.abilityToUse != ""){
                    playerOneOption = CombatOptions.Ability;
                }
                if (DataManager.Junak.itemManager.itemToUse != ""){
                    playerOneOption = CombatOptions.Item;
                }
            }
            else if(player == SaralfDataManager.Saralf) {
                if (Input.GetKeyDown(KeyCode.Q) && SaralfOption == CombatOptions.HasNotChosen) {
                    SaralfOption = CombatOptions.Attack;
                    CombatTextManager.combatTextManager.ManageText("Choose Order To Act");
                }
                if (Input.GetKeyDown(KeyCode.W) && SaralfOption == CombatOptions.HasNotChosen) {
                    CombatMenuManager.combatMenuManager.saralfAbilitySelectPanel.SetActive(true);
                    CombatMenuManager.combatMenuManager.saralfAbilityReturnButton.Select();
                    CombatTextManager.combatTextManager.ManageText("Choose Order To Act");
                }                                                                                                       
                if (Input.GetKeyDown(KeyCode.E) && SaralfOption == CombatOptions.HasNotChosen) {
                    CombatMenuManager.combatMenuManager.itemPanelWhenTurnedOn();
                    CombatTextManager.combatTextManager.ManageText("Choose Order To Act");
                }
                if (Input.GetKeyDown(KeyCode.R) && SaralfOption == CombatOptions.HasNotChosen) {
                    Run(SaralfDataManager.Saralf);
                }
                /*if (SaralfDataManager.Saralf.abilityManager.abilityToUse != "") {
                    SaralfOption = CombatOptions.Ability;
                }*/
                
                if (SaralfDataManager.Saralf.itemManager.itemToUse != "") {
                    SaralfOption = CombatOptions.Item;
                }
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
        else if (Input.GetKeyDown(KeyCode.Alpha3) && CombatTextManager.combatTextManager.textIsFinished && getCombatMembers() >= 3) {
            CombatTextManager.combatTextManager.ManageText("Order is Three");
            CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
            theOrder = 3;
        }
        if(player == DataManager.Junak) {        
            playerOneChosenOrder = theOrder;
        }
        else if (player == SaralfDataManager.Saralf) {
            SaralfChosenOrder = theOrder;
        }
    }
        
    // TODO: Get Target Method

    // Handles all actions that happen on the order
    private void HandleOrder(DataManager player) {
        if (player == DataManager.Junak && isTextManagerDone()) {
            if (playerOneChosenOrder == order && playerOneOption != CombatOptions.HasNotChosen && playerOneChosenOrder != 0) {
                if (playerOneOption == CombatOptions.Attack) {          
                    Attack(EnemyDataManager.EnemyManager, DataManager.Junak);
                }
                else if (playerOneOption == CombatOptions.Ability){
                    if(SaralfOption == CombatOptions.Ability) {
                        if(SaralfChosenOrder == playerOneChosenOrder) {
                            //checkForCombo
                        }
                    }
                    DataManager.Junak.abilityManager.useAbility();
                }
                else if(playerOneOption == CombatOptions.Item){
                    DataManager.Junak.itemManager.useItem(DataManager.Junak);
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
                }*/
                else if (SaralfOption == CombatOptions.Item) {
                    SaralfDataManager.Saralf.itemManager.useItem(SaralfDataManager.Saralf);
                    CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
                }
                
            }
            SaralfHandled = true;
            
        }
        if (SaralfDataManager.Saralf.isInParty) {
            if (SaralfHandled && playerOneHandled) { order += 1; Debug.Log("Both Handled"); }
        }
        else {
            if (playerOneHandled) { order += 1; }
        }
    }
    private void checkWinner() {
        List<DataManager> partyFolk = new List<DataManager>();
        partyFolk.Add(DataManager.Junak);
        if (SaralfDataManager.Saralf.isInParty) {
            partyFolk.Add(SaralfDataManager.Saralf);
        }
        
        foreach(DataManager member in partyFolk){
            if(member.health <= 0) {
                amountDead += 1;
            }
        }
        
        if(amountDead == (getCombatMembers() - 1)) {
            combatState = CombatStates.EnemyWon;
        }
        if(EnemyDataManager.EnemyManager.health <= 0) {
            combatState = CombatStates.PlayerWon;
        }
    }

    void Update() {

        
        switch (combatState) {        
            case CombatStates.PlayerOneAttacking: // On Junak's turn
                combatCharSprite.GetComponent<SpriteRenderer>().sprite = playerOneCombatantSprite;
                DataManager.Junak.isTurnInCombat = true;
                checkWinner();
                if (playerOneOption == CombatOptions.HasNotChosen ) {                 
                    GetPlayerAction(DataManager.Junak);
                }
                if (playerOneOption != CombatOptions.HasNotChosen && playerOneChosenOrder == 0) {                 
                    GetOrder(DataManager.Junak);
                }
                if (playerOneOption != CombatOptions.HasNotChosen && playerOneChosenOrder != 0 && !playerOneHandled && isTextManagerDone()){
                    HandleOrder(DataManager.Junak);
                }
                if (!SaralfHandled && isTextManagerDone() && playerOneHandled && SaralfDataManager.Saralf.isInParty) {
                    HandleOrder(SaralfDataManager.Saralf);   
                }
                
                if(order == EnemyDataManager.EnemyManager.assignedOrderInCombat) {
                    playerOneHandled = false;
                    SaralfHandled = false;
                    DataManager.Junak.isTurnInCombat = false;
                    combatState = CombatStates.EnemyAttacking;
                }
                else if(order == SaralfDataManager.Saralf.assignedOrderInCombat) {
                    resetSaralfOptions();
                    DataManager.Junak.isTurnInCombat = false;
                    combatState = CombatStates.SaralfAttacking;
                }
                else if (order == orderToReset){
                    DataManager.Junak.isTurnInCombat = false;
                    combatState = CombatStates.ResetValues;
                }
                
                break;
            case CombatStates.EnemyAttacking: // On enemy's turn
                checkWinner();
                if (CombatTextManager.combatTextManager.textIsFinished && CombatTextManager.combatTextManager.pressedSpace && !enemyOneHasAttacked) {
                    //TODO: Choose Target
                    whichtarget = UnityEngine.Random.Range(0, getCombatMembers() - 1);
                    EnemyDataManager.EnemyManager.theMonster.Attack(partymembers[whichtarget]);
                    enemyOneHasAttacked = true;
                }
                if(isTextManagerDone() && !EnemyDataManager.EnemyManager.theMonster.displayedDamage) {                    
                    EnemyDataManager.EnemyManager.theMonster.DisplayDamage(partymembers[whichtarget]);
                }

                if (enemyOneHasAttacked) {
                    if (!playerOneHandled) { HandleOrder(DataManager.Junak); }
                    if (!SaralfHandled && SaralfDataManager.Saralf.isInParty) { HandleOrder(SaralfDataManager.Saralf); }
                }


                if (order == DataManager.Junak.assignedOrderInCombat && CombatTextManager.combatTextManager.textIsFinished) {
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

                break;
            case CombatStates.SaralfAttacking: // On Saralf's Turn
                checkWinner();
                combatCharSprite.GetComponent<SpriteRenderer>().sprite = SaralfCombatantSprite;
                SaralfDataManager.Saralf.isTurnInCombat = true;
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
                    HandleOrder(DataManager.Junak);
                }
                if (order == EnemyDataManager.EnemyManager.assignedOrderInCombat) {
                    playerOneHandled = false;
                    SaralfHandled = false;
                    SaralfDataManager.Saralf.isTurnInCombat = false;
                    combatState = CombatStates.EnemyAttacking;

                }
                else if (order == orderToReset) {
                    SaralfDataManager.Saralf.isTurnInCombat = false;
                    combatState = CombatStates.ResetValues;
                }
                else if (order == DataManager.Junak.assignedOrderInCombat) {
                    SaralfDataManager.Saralf.isTurnInCombat = false;
                    combatState = CombatStates.PlayerOneAttacking;
                }

                break;
            case CombatStates.ResetValues:
                order = 1;
                playerOneHandled = false;
                SaralfHandled = false;
                enemyOneHasAttacked = false;
                CombatTextManager.combatTextManager.textHasBeenPrompt = false;
                EnemyDataManager.EnemyManager.theMonster.displayedDamage = false;
                EnemyDataManager.EnemyManager.theMonster.textWasPrompt = false;
                if (order == DataManager.Junak.assignedOrderInCombat) {
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
                    DataManager.Junak.isBeingLoaded = true;
                    EnemyDataManager.EnemyManager.defeatedEnemies.Add(EnemyDataManager.EnemyManager.currentName);
                    DataManager.Junak.addExperience(EnemyDataManager.EnemyManager.experienceGives);
                    if (SaralfDataManager.Saralf.isInParty) { SaralfDataManager.Saralf.addExperience(EnemyDataManager.EnemyManager.experienceGives); }
                    DataManager.Junak.itemManager.isInCombat = false;
                    SaralfDataManager.Saralf.itemManager.isInCombat = false;
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
                    DataManager.Junak.itemManager.isInCombat = false;
                    SaralfDataManager.Saralf.itemManager.isInCombat = false;
                    SceneManager.LoadScene("MainMenu");
                }
                    break;
            default:
                break;

        }
        // Show player's health 
        CombatTextManager.combatTextManager.playerOneHealthText.text = DataManager.Junak.health.ToString();
        if (SaralfDataManager.Saralf.isInParty) {
            CombatTextManager.combatTextManager.saralfHealthText.text = SaralfDataManager.Saralf.health.ToString();
        }
        CombatTextManager.combatTextManager.orderText.text = order.ToString();
    }
}
