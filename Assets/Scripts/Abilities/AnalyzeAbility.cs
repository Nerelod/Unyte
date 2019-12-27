using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnalyzeAbility : Ability
{
    public static AnalyzeAbility analyzeAbility = new AnalyzeAbility();
    public AnalyzeAbility()
    {
        Debug.Log("Analyze Constructor");
        name = "Analyze";
        compatibleAbilities.Add("Investigate");
    }
    public override void execute()
    {
        CombatTextManager.combatTextManager.ManageText("analyzed");
        CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
    }
}
