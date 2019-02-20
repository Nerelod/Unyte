using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CombatStates {PlayerOneAttacking, EnemyAttacking, ResetValues}
public enum CombatOptions { Attack, Ability, Item, Run, HasNotChosen}

public class CombatManager : MonoBehaviour { 


    public GameObject enemySprite;
    CombatStates combatState;
    CombatOptions playerOneOption;
    private int order;
    private int playerOneChosenOrder;
    private bool enemyOneHasAttacked;
    void Start() {    
        CombatTextManager.combatTextManager.damageText.text = "";
        order = 1;
        CombatTextManager.combatTextManager.textHasBeenPrompt = false;
        CombatTextManager.combatTextManager.pressedSpace = false;
        playerOneChosenOrder = 0;
        playerOneOption = CombatOptions.HasNotChosen;
        CombatTextManager.combatTextManager.ManageText("A " + EnemyDataManager.EnemyManager.currentName + " appeared!");
        CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
        enemySprite = GameObject.Find("Enemy");
        enemySprite.GetComponent<SpriteRenderer>().sprite = EnemyDataManager.EnemyManager.currentSprite;
        enemyOneHasAttacked = false;
        if(DataManager.manager.speed > EnemyDataManager.EnemyManager.speed) {        
            combatState = CombatStates.PlayerOneAttacking;
            EnemyDataManager.EnemyManager.assignedOrderInCombat = 2;
        }
        else {        
            combatState = CombatStates.EnemyAttacking;
            EnemyDataManager.EnemyManager.assignedOrderInCombat = 1;
        }
    }

    private void GetPlayerAction(int player) {    
        if (CombatTextManager.combatTextManager.textIsFinished && !CombatTextManager.combatTextManager.textHasBeenPrompt && CombatTextManager.combatTextManager.pressedSpace) {         
            CombatTextManager.combatTextManager.ManageText("Choose an Action");
            CombatTextManager.combatTextManager.textHasBeenPrompt = true;
        }
        if(player == 1 && CombatTextManager.combatTextManager.textHasBeenPrompt) {         
            if (Input.GetKeyDown(KeyCode.Q) && playerOneOption == CombatOptions.HasNotChosen) {              
                playerOneOption = CombatOptions.Attack;
                CombatTextManager.combatTextManager.ManageText("Choose Order To Act");
            }
        }
    }

    private void Attack(EnemyDataManager enemy, DataManager character) {     
        CombatTextManager.combatTextManager.ManageText("Player does " + character.qDamage.ToString() + " damage!");
        //play enemy sprite damaged animation
        CombatTextManager.combatTextManager.damageText.text = "-" + character.qDamage.ToString();
        CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.FadeText(CombatTextManager.combatTextManager.damageText));
        CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
        enemy.health -= character.qDamage;
    }

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
        
    //Get Target Method
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
                break;
            case CombatStates.ResetValues:
                Debug.LogWarning("ResetValues");
                playerOneOption = CombatOptions.HasNotChosen;
                playerOneChosenOrder = 0;
                break;
            default:
                break;

        }
    }
}
