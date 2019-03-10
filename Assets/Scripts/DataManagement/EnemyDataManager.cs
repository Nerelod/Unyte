using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDataManager : MonoBehaviour
{

    public static EnemyDataManager EnemyManager;

    public int health;
    public int experienceGives;
    public int speed = 3;
    public int assignedOrderInCombat;
    public Sprite currentSprite = null;
    public string currentName;
    public Monster theMonster;
    public string theScene;

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
