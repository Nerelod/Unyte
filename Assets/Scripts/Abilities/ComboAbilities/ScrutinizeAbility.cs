using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]

public class ScrutinizeAbility : ComboAbility
{
    public static ScrutinizeAbility scrutinizeAbility = new ScrutinizeAbility();
    public ScrutinizeAbility()
    {
        requiredAbilities = new List<Ability>();
        requiredAbilities.Add(InvestigateAbility.investigateAbility);
        requiredAbilities.Add(AnalyzeAbility.analyzeAbility);
        name = "Scrutinize";
    }
    public override void execute() {
        Debug.Log("Yeet");
    }
}
