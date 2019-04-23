using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public List<System.Action> abilities = new List<System.Action>();
    public List<string> aquiredAbilities = new List<string>();

    void Start()
    {
        
    }

    public void Ivestigate(EnemyDataManager enemy){
        CombatTextManager.combatTextManager.ManageText(enemy.health.ToString());
    }
    void Update()
    {
        
    }
}
