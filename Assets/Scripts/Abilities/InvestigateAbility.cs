using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InvestigateAbility : Ability
{
    public static InvestigateAbility investigateAbility = new InvestigateAbility();
    public InvestigateAbility()
    {
        name = "Investigate";
        compatibleAbilities.Add("Analyze");
        requiresEnemyTarget = true;
    }
    public override void execute()
    {
        CombatTextManager.combatTextManager.ManageText("Investigate Reveals " + JunakDataManager.Junak.enemyToTarget.currentName + " Has " + JunakDataManager.Junak.enemyToTarget.health.ToString() + " health");
        experience++;
        CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
    }
}
