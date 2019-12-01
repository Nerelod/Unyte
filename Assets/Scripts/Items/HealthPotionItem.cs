using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotionItem : ItemScript { 
    public static HealthPotionItem healthPotionItem = new HealthPotionItem();
    public override void execute(DataManager player) {
        player.health = player.health + 5;
        if (player.health > player.totalHealth) { player.health = player.totalHealth; }
        if (player.itemManager.isInCombat) {
            CombatTextManager.combatTextManager.ManageText(player.theName + " Used Health Potion!");
        }
    }
}
