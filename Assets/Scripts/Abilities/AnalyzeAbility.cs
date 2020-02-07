using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnalyzeAbility : Ability
{
    public static AnalyzeAbility analyzeAbility = new AnalyzeAbility();
    public AnalyzeAbility()
    {
        name = "Analyze";
        compatibleAbilities.Add("Investigate");
        requiresEnemyTarget = true;
    }
    public override void execute()
    {
        CombatTextManager.combatTextManager.ManageText("analyzed");
        CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
    }
}
