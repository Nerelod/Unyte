using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDataManager : DataManager
{
    // Reference to EnemyDataManager
    public static EnemyDataManager EnemyManager;


    public int experienceGives;
    public string currentName;
    public string currentType;
    public int currentID;
    public Monster theMonster;
    public string theScene;

    public int amountOfEnemies;



    //List of enemies that were defeated in combat
    public List<int> defeatedEnemies = new List<int>();

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
