using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public List<string> aquiredAbilities = new List<string>();

    public string abilityToUse;

    void Start() { 

    }

    public void playerOneSelectAbility(string selectedAbility){
        if(DataManager.Junak.abilityManager.aquiredAbilities.Contains(selectedAbility)){
            DataManager.Junak.abilityManager.abilityToUse = selectedAbility;
            CombatMenuManager.combatMenuManager.abilitySelectPanel.SetActive(false);

        }
    }
    public void saralfSelectAbility(string selectedAbility) {
        if (SaralfDataManager.Saralf.abilityManager.aquiredAbilities.Contains(selectedAbility)) {
            SaralfDataManager.Saralf.abilityManager.abilityToUse = selectedAbility;
            CombatMenuManager.combatMenuManager.saralfAbilitySelectPanel.SetActive(false);
        }
    }

    public void useAbility(){
        if(abilityToUse == "Investigate"){
            InvestigateAbility.investigateAbility.execute();
        }
    }
    public void turnOffAbilitySelect(){
        CombatMenuManager.combatMenuManager.abilitySelectPanel.SetActive(false);
        CombatMenuManager.combatMenuManager.saralfAbilitySelectPanel.SetActive(false);
        CombatTextManager.combatTextManager.textHasBeenPrompt = false;
    }
    void Update()
    {
        
    }
}
