using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Slime : Monster
{
    private int damage;
    private string monsterName;
    void Start()
    {
        damage = 2;
        textWasPrompt = false;
        monsterController = GetComponent<MonsterController>();
    }

    private void Launch(DataManager target){
        if (textWasPrompt == false) {
            CombatTextManager.combatTextManager.ManageText(monsterName + " launches itself at " + target.theName + "!");
            CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
            target.health = target.health - damage;
            textWasPrompt = true;
        }
    }
    private void JumpMenancingly(){
        if (textWasPrompt == false) {
            CombatTextManager.combatTextManager.ManageText(monsterName + " Jumps Menancingly!");
            CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
            textWasPrompt = true;
        }
    }

    public override void Attack(DataManager target, EnemyDataManager monster) {
        whichAttack = Random.Range(1, 3);
        textWasPrompt = false;
        displayedDamage = false;
        monsterName = monster.currentName;
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

    void Update()
    {
        
    }
}
