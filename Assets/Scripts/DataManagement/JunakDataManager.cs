using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

[Serializable]

public class JunakDataManager : DataManager
{

    // reference to itself
    public static JunakDataManager Junak;


    // Happens before start, makes sure there is only one instance of DataManager
    private void Awake()
    {
        if (Junak == null)
        {
            DontDestroyOnLoad(gameObject);
            Junak = this;
        }
        else if (Junak != this)
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        abilityManager = gameObject.AddComponent<AbilityManager>() as AbilityManager;
        itemManager = gameObject.AddComponent<ItemManager>() as ItemManager;

        Junak.theName = "Junak";

        Junak.abilityManager.aquiredAbilities.Add(InvestigateAbility.investigateAbility);
        Junak.abilityManager.aquiredComboAbilities.Add(ScrutinizeAbility.scrutinizeAbility);
        experienceNeeded = 10;

    }
    

    private void Update()
    {

        currentScene = SceneManager.GetActiveScene().name; //TODO: instead of getting the scene every frame, update the scene when switching scenes

    }
}



