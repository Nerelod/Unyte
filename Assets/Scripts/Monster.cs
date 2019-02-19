using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster { 


    public static Monster Slime = new Monster(5, "Slime jumps menancingly!");

    public int damage;
    public string attackMessage;
    public bool textWasPrompt = false;
    public Monster(int dmg, string atkmsg) { 
        damage = dmg;
        attackMessage = atkmsg;
    }

    public void Attack(int damage, string message, DataManager target) { 
        if (textWasPrompt == false) { 
            CombatTextManager.combatTextManager.ManageText(message);
            CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
            textWasPrompt = true;
        }
        if (damage > 0 && CombatTextManager.combatTextManager.pressedSpace) { 
            CombatTextManager.combatTextManager.ManageText(target.theName + " takes " + damage + " damage");
            target.health = target.health - damage;
        }
    }


    
}
