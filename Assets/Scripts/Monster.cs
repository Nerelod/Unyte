using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster
{

    public static Monster Slime = new Monster(5, "Slime jumps menancingly!");

    public int damage;
    public string attackMessage;
    public Monster(int dmg, string atkmsg)
    {
        damage = dmg;
        attackMessage = atkmsg;
    }

    public void Attack(int damage, string message)
    {
        CombatTextManager.combatTextManager.ManageText(message);
    }


    
}
