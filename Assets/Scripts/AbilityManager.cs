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
    public void turnOffAbilitySelect(GameObject panel){
        panel.SetActive(false);
    }
    void Update()
    {
        
    }
}
