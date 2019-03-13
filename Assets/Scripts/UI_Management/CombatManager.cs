﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum CombatStates {PlayerOneAttacking, EnemyAttacking, ResetValues, PlayerWon, EnemyWon}
public enum CombatOptions { Attack, Ability, Item, Run, HasNotChosen}

public class CombatManager : MonoBehaviour { 


    public GameObject enemySprite;
    CombatStates combatState;
    CombatOptions playerOneOption;
    private int order;
    private int playerOneChosenOrder;
    private bool enemyOneHasAttacked;
    private bool winTextHasBeenPrompt;
    private void Awake() {
        CombatTextManager.combatTextManager = GameObject.Find("CombatTextManager").GetComponent<CombatTextManager>();
    }
    void Start() {
        winTextHasBeenPrompt = false;
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
    }
}
