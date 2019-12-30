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
    }
    public override void execute()
    {
        CombatTextManager.combatTextManager.ManageText("Investigate Reveals The Enemy Has " + EnemyDataManager.EnemyManager.health.ToString() + " health");
        experience++;
        CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
    }
}
