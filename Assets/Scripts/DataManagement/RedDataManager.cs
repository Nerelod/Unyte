using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedDataManager : DataManager
{
    public static RedDataManager Red;

    private void Awake() {
        if (Red == null) {
            DontDestroyOnLoad(gameObject);
            Red = this;
        }
        else if (Red != this) {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Red.theName = "Red";
        itemManager = gameObject.AddComponent<ItemManager>() as ItemManager;
        abilityManager = gameObject.AddComponent<AbilityManager>() as AbilityManager;
        experienceNeeded = 10;
    }


    void Update()
    {
        
    }
}
