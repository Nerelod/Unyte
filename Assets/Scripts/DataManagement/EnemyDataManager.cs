using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDataManager : MonoBehaviour
{
    // Reference to EnemyDataManager
    public static EnemyDataManager EnemyManager;

    public int health, healthTwo;
    public int experienceGives, experienceGivesTwo;
    public int speed, speedTwo;
    public int assignedOrderInCombat, assignedOrderInCombatTwo;
    // Sprite of enemy in combat scene
    public Sprite currentSprite, currentSpriteTwo = null;
    public string currentName, currentNameTwo;
    public string currentType, currentTypeTwo;
    public Monster theMonster, theMonsterTwo;
    public string theScene;

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
