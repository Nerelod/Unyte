using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster { 


    public static Monster Slime = new Monster(2, 2, "Slime launches itself at you!", "Slime jumps menancingly!", "", "", 0);

    private int damage;
    private int damageTwo;
    private int damageThree;
    private int damageFour;
    private string attackMessage;
    private string attackMessageTwo;
    private string attackMessageThree;
    private string attackMessageFour;
    public bool textWasPrompt = false;
    public bool displayedDamage = false;
    private int range;
    private int whichAttack;    

    public Monster(int numberOfAttacks, int dmg, string message, string messageTwo = "", string messageThree = "", string messageFour = "", int dmgTwo = 0, int dmgThree = 0, int dmgFour = 0) {
        range = numberOfAttacks + 1;
        damage = dmg;
        damageTwo = dmgTwo;
        damageThree = dmgThree;
        damageFour = dmgFour;
        attackMessage = message;
        attackMessageTwo = messageTwo;
        attackMessageThree = messageThree;
        attackMessageFour = messageFour;
    }

    public void Attack(DataManager target) {
        whichAttack = Random.Range(1, range);
        if (whichAttack == 1) {
            if (textWasPrompt == false) {
                CombatTextManager.combatTextManager.ManageText(attackMessage);
                CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
                target.health = target.health - damage;
                textWasPrompt = true;
            }
        }
        else if(whichAttack == 2) {
            if (textWasPrompt == false) {
                CombatTextManager.combatTextManager.ManageText(attackMessageTwo);
                CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
                target.health = target.health - damageTwo;
                textWasPrompt = true;
            }
        }
        if (whichAttack == 3) {
            if (textWasPrompt == false) {
                CombatTextManager.combatTextManager.ManageText(attackMessageThree);
                CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
                target.health = target.health - damageThree;
                textWasPrompt = true;
            }
        }
        else if (whichAttack == 4) {
            if (textWasPrompt == false) {
                CombatTextManager.combatTextManager.ManageText(attackMessageFour);
                CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
                target.health = target.health - damageFour;
                textWasPrompt = true;
            }
        }
    }

    public void DisplayDamage(DataManager target) {
        if (whichAttack == 1) {
            if (damage > 0 && CombatTextManager.combatTextManager.textIsFinished) {
                CombatTextManager.combatTextManager.ManageText(target.theName + " takes " + damage + " damage!");
                CombatTextManager.combatTextManager.enemyDamageText.text = "-" + damage.ToString();
                CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.FadeText(CombatTextManager.combatTextManager.enemyDamageText));
                CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
            }
        }
        else if (whichAttack == 2) {
            if (damageTwo > 0 && CombatTextManager.combatTextManager.textIsFinished) {
                CombatTextManager.combatTextManager.ManageText(target.theName + " takes " + damageTwo + " damage!");
                CombatTextManager.combatTextManager.enemyDamageText.text = "-" + damageTwo.ToString();
                CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.FadeText(CombatTextManager.combatTextManager.enemyDamageText));
                CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
            }
        }
        else if (whichAttack == 3) {
            if (damageThree > 0 && CombatTextManager.combatTextManager.textIsFinished) {
                CombatTextManager.combatTextManager.ManageText(target.theName + " takes " + damageThree + " damage!");
                CombatTextManager.combatTextManager.enemyDamageText.text = "-" + damageThree.ToString();
                CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.FadeText(CombatTextManager.combatTextManager.enemyDamageText));
                CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
            }
        }
        else if (whichAttack == 4) {
            if (damageFour > 0 && CombatTextManager.combatTextManager.textIsFinished) {
                CombatTextManager.combatTextManager.ManageText(target.theName + " takes " + damageFour + " damage!");
                CombatTextManager.combatTextManager.enemyDamageText.text = "-" + damageFour.ToString();
                CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.FadeText(CombatTextManager.combatTextManager.enemyDamageText));
                CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
            }
        }
        displayedDamage = true;
    }
}
    

