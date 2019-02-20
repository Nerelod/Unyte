using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster { 


    public static Monster Slime = new Monster(2, 3, "Slime launches itself at you!", "Slime jumps menancingly!", "", "", 0);

    private int damage;
    private int damageTwo;
    private int damageThree;
    private int damageFour;
    private string attackMessage;
    private string attackMessageTwo;
    private string attackMessageThree;
    private string attackMessageFour;
    public bool textWasPrompt = false;
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
                textWasPrompt = true;
            }
            if (damage > 0 && CombatTextManager.combatTextManager.pressedSpace) {
                CombatTextManager.combatTextManager.ManageText(target.theName + " takes " + damage + " damage");
                target.health = target.health - damage;
            }
        }
        else if(whichAttack == 2) {
            if (textWasPrompt == false) {
                CombatTextManager.combatTextManager.ManageText(attackMessageTwo);
                CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
                textWasPrompt = true;
            }
            if (damage > 0 && CombatTextManager.combatTextManager.pressedSpace) {
                CombatTextManager.combatTextManager.ManageText(target.theName + " takes " + damageTwo + " damage");
                target.health = target.health - damage;
            }
        }
    }


    
}
