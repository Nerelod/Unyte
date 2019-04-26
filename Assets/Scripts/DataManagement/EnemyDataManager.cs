using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDataManager : MonoBehaviour
{
    // Reference to EnemyDataManager
    public static EnemyDataManager EnemyManager;

    public int health;
    public int experienceGives;
    public int speed = 3;
    public int assignedOrderInCombat;
    // Sprite of enemy in combat scene
    public Sprite currentSprite = null;
    public string currentName;
    public string currentType;
    public Monster theMonster;
    public string theScene;

    public List<Monster> monsterTypes = new List<Monster>();

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
        monsterTypes.Add(Monster.Slime); // Slime is 0
        
    }

    
    void Update() { 
    
        
    }
}
