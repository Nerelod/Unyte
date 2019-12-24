using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public List<Ability> aquiredAbilities = new List<Ability>();
    public List<ComboAbility> aquiredComboAbilities = new List<ComboAbility>();
    public Ability abilityToUse;

    public bool choseAbilityInCombat;

    void Start()
    {

    }
    public void junakSelectAbility(string abilityName)
    {
        int abilityIndex = DataManager.Junak.abilityManager.aquiredAbilities.FindIndex(a => a.name == abilityName);
        DataManager.Junak.abilityManager.abilityToUse = DataManager.Junak.abilityManager.aquiredAbilities[abilityIndex];
        DataManager.Junak.abilityManager.choseAbilityInCombat = true;
        CombatMenuManager.combatMenuManager.abilitySelectPanel.SetActive(false);
    }
    public void saralfSelectAbility(string abilityName)
    {
        int abilityIndex = SaralfDataManager.Saralf.abilityManager.aquiredAbilities.FindIndex(a => a.name == abilityName);
        SaralfDataManager.Saralf.abilityManager.abilityToUse = SaralfDataManager.Saralf.abilityManager.aquiredAbilities[abilityIndex];
        SaralfDataManager.Saralf.abilityManager.choseAbilityInCombat = true;
        CombatMenuManager.combatMenuManager.saralfAbilitySelectPanel.SetActive(false);
    }

    public void useAbility()
    {
        abilityToUse.execute();
    }
    public void turnOffAbilitySelect()
    {
        CombatMenuManager.combatMenuManager.abilitySelectPanel.SetActive(false);
        CombatMenuManager.combatMenuManager.saralfAbilitySelectPanel.SetActive(false);
        CombatTextManager.combatTextManager.textHasBeenPrompt = false;
    }

    public void CheckForComboAbility(DataManager player)
    {

    }
    void Update()
    {

    }
}
