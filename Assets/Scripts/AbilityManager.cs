using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public List<string> aquiredAbilities = new List<string>();

    public string abilityToUse;

    void Start()
    {
        
    }

    public void Investigate(){
        CombatTextManager.combatTextManager.ManageText("Investigate Reveals The Enemy Has " + EnemyDataManager.EnemyManager.health.ToString() + " health");
        Debug.Log("using ability");
        CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
    }

    public void playerOneSelectAbility(string selectedAbility){
        if(DataManager.playerOne.abilityManager.aquiredAbilities.Contains(selectedAbility)){
            DataManager.playerOne.abilityManager.abilityToUse = selectedAbility;
            CombatMenuManager.combatMenuManager.abilitySelectPanel.SetActive(false);

        }
    }

    public void useAbility(){
        if(abilityToUse == "Investigate"){
            Investigate();
        }
    }
    public void turnOffAbilitySelect(){
        CombatMenuManager.combatMenuManager.abilitySelectPanel.SetActive(false);
        CombatTextManager.combatTextManager.textHasBeenPrompt = false;
    }
    void Update()
    {
        
    }
}
