using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvestigateAbility : Ability {

    public static InvestigateAbility investigateAbility = new InvestigateAbility();
    public override void execute() {
        CombatTextManager.combatTextManager.ManageText("Investigate Reveals The Enemy Has " + EnemyDataManager.EnemyManager.health.ToString() + " health");
        experience++;
        CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
    }
}
