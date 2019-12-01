using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneItem : ItemScript {
    public static StoneItem stoneItem = new StoneItem();
    public override void execute(DataManager player) {
        if (player.itemManager.isInCombat) {
            CombatTextManager.combatTextManager.ManageText(player.theName + " Used Stone!");
            EnemyDataManager.EnemyManager.health -= 2;
            CombatTextManager.combatTextManager.damageText.text = "-2";
            CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.FadeText(CombatTextManager.combatTextManager.damageText));
        }
    }
}
