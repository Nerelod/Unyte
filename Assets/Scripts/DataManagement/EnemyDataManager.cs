using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDataManager : MonoBehaviour
{

    public static EnemyDataManager EnemyManager;

    public int health;
    public int experienceGives;
    public int speed = 3;
    public Sprite currentSprite = null;
    public string currentName;
    

    private void Awake()
    {
        if (EnemyManager == null)
        {
            DontDestroyOnLoad(gameObject);
            EnemyManager = this;
        }
        else if (EnemyManager != this)
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
