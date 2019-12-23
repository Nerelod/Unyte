using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalyzeAbility : Ability
{
    public static AnalyzeAbility analyzeAbility = new AnalyzeAbility();
    public AnalyzeAbility()
    {
        comboNumber = 1;
    }
    public override void execute()
    {
        CombatTextManager.combatTextManager.ManageText("analyzed");
        CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
    }
}
