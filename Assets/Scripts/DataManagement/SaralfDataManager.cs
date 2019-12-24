using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaralfDataManager : DataManager
{
    public static SaralfDataManager Saralf;

    private void Awake()
    {
        if (Saralf == null)
        {
            DontDestroyOnLoad(gameObject);
            Saralf = this;
        }
        else if (Saralf != this)
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        Saralf.theName = "Saralf";
        itemManager = gameObject.AddComponent<ItemManager>() as ItemManager;
        abilityManager = gameObject.AddComponent<AbilityManager>() as AbilityManager;
        Saralf.abilityManager.aquiredAbilities.Add(AnalyzeAbility.analyzeAbility);
        Saralf.abilityManager.aquiredComboAbilities.Add(ScrutinizeAbility.scrutinizeAbility);
        experienceNeeded = 10;
    }


    void Update()
    {

    }
}
