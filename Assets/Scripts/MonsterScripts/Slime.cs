using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Slime : Monster
{
    private int damage;
    private MonsterController monsterController;

    void Start()
    {
        damage = 2;
        textWasPrompt = false;
        monsterController = GetComponent<MonsterController>();
    }

    private void Launch(DataManager target){
        if (textWasPrompt == false) {
                CombatTextManager.combatTextManager.ManageText("Slime launches itself at " + target.theName + "!");
                CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
                target.health = target.health - damage;
                textWasPrompt = true;
            }
    }
    private void JumpMenancingly(){
        if (textWasPrompt == false) {
                CombatTextManager.combatTextManager.ManageText("Slime Jumps Menancingly!");
                CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
                textWasPrompt = true;
            }
    }
    //TODO: Choose Target
    public override void Attack(DataManager target) {
        whichAttack = Random.Range(1, 3);
        if (whichAttack == 1) {
            Launch(target);
        }
        else if(whichAttack == 2) {
            JumpMenancingly();
        }
    }
    public override void DisplayDamage(DataManager target) {
        if (whichAttack == 1 && CombatTextManager.combatTextManager.textIsFinished) {
            CombatTextManager.combatTextManager.ManageText(target.theName + " takes " + damage + " damage!");
            CombatTextManager.combatTextManager.enemyDamageText.text = "-" + damage.ToString();
            CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.FadeText(CombatTextManager.combatTextManager.enemyDamageText));
            CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
        }
        displayedDamage = true;
    }
    private void OnCollisionEnter2D(Collision2D collision) { 
        if (collision.gameObject.tag == "Player" && !monsterController.player.isInvincible) {
            EnemyDataManager.EnemyManager.theScene = monsterController.scene;
            EnemyDataManager.EnemyManager.currentSprite = monsterController.combatSprite;
            EnemyDataManager.EnemyManager.currentName = monsterController.monsterName;
            EnemyDataManager.EnemyManager.health = monsterController.health;
            EnemyDataManager.EnemyManager.experienceGives = monsterController.experienceToGive;    
            EnemyDataManager.EnemyManager.speed = monsterController.speed;
            EnemyDataManager.EnemyManager.currentType = monsterController.monsterType;
            EnemyDataManager.EnemyManager.theMonster = this;  
            monsterController.canMove = false;
            monsterController.player.State = States.CannotMove;
            Transitions.screenTransition.StartCoroutine(Transitions.screenTransition.FadeOut("CombatScene", .33f));
        }    
    }

    void Update()
    {
        
    }
}
