using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDataManager : DataManager
{
    // Reference to EnemyDataManager
    public static EnemyDataManager EnemyManager;

    public int healthTwo;
    public int experienceGives, experienceGivesTwo;
    public int speedTwo;
    public int assignedOrderInCombatTwo;
    // Sprite of enemy in combat scene
    public Sprite currentSpriteTwo;
    public string currentName, currentNameTwo;
    public string currentType, currentTypeTwo;
    public Monster theMonster, theMonsterTwo;
    public string theScene;

    public int amountOfEnemies;

    

    //List of enemies that were defeated in combat
    public List<string> defeatedEnemies = new List<string>();

    private void Awake() { 
    
        if (EnemyManager == null) {         
            DontDestroyOnLoad(gameObject);
            EnemyManager = this;
        }
        else if (EnemyManager != this) {        
            Destroy(gameObject);
        }
    }
    void Start() {    
    }

    
    void Update() {       
    }
}
