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
    // The order playerOne chose to act
    private int playerOneChosenOrder;
    // Boolean that represents whether the enemy has attacked
    private bool enemyOneHasAttacked;
    // Boolean that represents whether the win text was prompt
    private bool winTextHasBeenPrompt;

    // Runs before Awake
    private void Awake() {
        // Get a reference to the CombatTextManager, so it exists
        CombatTextManager.combatTextManager = GameObject.Find("CombatTextManager").GetComponent<CombatTextManager>();
    }

    // Used to set all the values to default at the start of combat
    void Start() {
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
        CombatTextManager.combatTextManager.ManageText("A " + EnemyDataManager.EnemyManager.currentName + " appeared!");
        CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
        // Assign the enemySprite
        enemySprite = GameObject.Find("Enemy");
        enemySprite.GetComponent<SpriteRenderer>().sprite = EnemyDataManager.EnemyManager.currentSprite;
        // The enemy has not attacked
        enemyOneHasAttacked = false;
        
        // Get the order that combtatants will act on
        determineOrder(); 
    }

    // Order is determined by speed
    private void determineOrder() {
        if (DataManager.manager.speed > EnemyDataManager.EnemyManager.speed) {
            combatState = CombatStates.PlayerOneAttacking;
            EnemyDataManager.EnemyManager.assignedOrderInCombat = 2;
            DataManager.manager.assignedOrderInCombat = 1;
        }
        else {
            combatState = CombatStates.EnemyAttacking;
            EnemyDataManager.EnemyManager.assignedOrderInCombat = 1;
            DataManager.manager.assignedOrderInCombat = 2;
        }
    }

    // Gets the player's chosen action 
    private void GetPlayerAction(int player) {
        // If the previous text is finished, the text has not been prompt, and the user pressed space, display "Choose an Action" 
        if (CombatTextManager.combatTextManager.textIsFinished && !CombatTextManager.combatTextManager.textHasBeenPrompt && CombatTextManager.combatTextManager.pressedSpace) {         
            CombatTextManager.combatTextManager.ManageText("Choose an Action");
            CombatTextManager.combatTextManager.textHasBeenPrompt = true;
        }
        // Assign playerOneOption if it is HasNotChosen based on input, followed by displaying "Choose Order To Act"
        if (player == 1 && CombatTextManager.combatTextManager.textHasBeenPrompt) {         
            if (Input.GetKeyDown(KeyCode.Q) && playerOneOption == CombatOptions.HasNotChosen) {              
                playerOneOption = CombatOptions.Attack;
                CombatTextManager.combatTextManager.ManageText("Choose Order To Act");
            }
        }
        if (player == 1 && CombatTextManager.combatTextManager.textHasBeenPrompt) {
            if (Input.GetKeyDown(KeyCode.W) && playerOneOption == CombatOptions.HasNotChosen) {
                CombatTextManager.combatTextManager.ManageText("You do not have any abilities");
                CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
                CombatTextManager.combatTextManager.textHasBeenPrompt = false;
                // playerOneOption = CombatOptions.Ability;
                //CombatTextManager.combatTextManager.ManageText("Choose Order To Act");
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
    private void GetOrder(int player) {    
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
        if(player == 1) {        
            playerOneChosenOrder = theOrder;
        }
    }
        
    // TODO: Get Target Method

    // Handles all actions that happen on the order
    private void HandleOrder(int player) {    
        if (player == 1) {        
            if (playerOneChosenOrder != order && CombatTextManager.combatTextManager.textIsFinished && CombatTextManager.combatTextManager.pressedSpace && playerOneOption != CombatOptions.HasNotChosen && playerOneChosenOrder != 0) { 
                order += 1;
            }
            else if (playerOneChosenOrder == order && CombatTextManager.combatTextManager.textIsFinished && CombatTextManager.combatTextManager.pressedSpace && playerOneOption != CombatOptions.HasNotChosen && playerOneChosenOrder != 0) {            
                if (playerOneOption == CombatOptions.Attack) {            
                    Attack(EnemyDataManager.EnemyManager, DataManager.manager);
                }
                order += 1;
            }
        }
    }

    void Update() {

        
        switch (combatState) {        
            case CombatStates.PlayerOneAttacking:
                if (playerOneOption == CombatOptions.HasNotChosen) {                 
                    GetPlayerAction(1);
                }
                if (playerOneOption != CombatOptions.HasNotChosen && playerOneChosenOrder == 0) {                 
                    GetOrder(1);
                }

                HandleOrder(1);
                
                if(order == EnemyDataManager.EnemyManager.assignedOrderInCombat) {                  
                    combatState = CombatStates.EnemyAttacking;
                }
                if(EnemyDataManager.EnemyManager.health <= 0) {
                    combatState = CombatStates.PlayerWon;
                }
                break;
            case CombatStates.EnemyAttacking:
                if (CombatTextManager.combatTextManager.textIsFinished && CombatTextManager.combatTextManager.pressedSpace && !enemyOneHasAttacked) {              
                    EnemyDataManager.EnemyManager.theMonster.Attack(DataManager.manager);
                    enemyOneHasAttacked = true;
                }
                if(CombatTextManager.combatTextManager.pressedSpace && CombatTextManager.combatTextManager.textIsFinished && !EnemyDataManager.EnemyManager.theMonster.displayedDamage) {                    
                    EnemyDataManager.EnemyManager.theMonster.DisplayDamage(DataManager.manager);
                }
                if(CombatTextManager.combatTextManager.pressedSpace && CombatTextManager.combatTextManager.textIsFinished) {
                    combatState = CombatStates.ResetValues;
                }
                if(DataManager.manager.health <= 0) {
                    combatState = CombatStates.EnemyWon;
                }
                HandleOrder(1);
                break;
            case CombatStates.ResetValues:
                playerOneOption = CombatOptions.HasNotChosen;
                playerOneChosenOrder = 0;
                order = 1;
                enemyOneHasAttacked = false;
                combatState = CombatStates.PlayerOneAttacking;
                CombatTextManager.combatTextManager.textHasBeenPrompt = false;
                EnemyDataManager.EnemyManager.theMonster.displayedDamage = false;
                EnemyDataManager.EnemyManager.theMonster.textWasPrompt = false;
                combatState = CombatStates.PlayerOneAttacking;               
                break;
            case CombatStates.PlayerWon:
                if (CombatTextManager.combatTextManager.textIsFinished && CombatTextManager.combatTextManager.pressedSpace && !winTextHasBeenPrompt) {
                    CombatTextManager.combatTextManager.ManageText("You Win!");
                    CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
                    winTextHasBeenPrompt = true;
                }
                if(CombatTextManager.combatTextManager.pressedSpace && winTextHasBeenPrompt) {
                    DataManager.manager.isBeingLoaded = true;
                    EnemyDataManager.EnemyManager.defeatedEnemies.Add(EnemyDataManager.EnemyManager.currentName);
                    DataManager.manager.addExperience(EnemyDataManager.EnemyManager.experienceGives);
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
                    //Destroy(CombatTextManager.combatTextManager);
                    SceneManager.LoadScene("MainMenu");
                }
                    break;
            default:
                break;

        }
        // Show player's health 
        CombatTextManager.combatTextManager.playerOneHealthText.text = DataManager.manager.health.ToString();
    }
}
