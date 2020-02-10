using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDataManagerTwo : EnemyDataManager
{
    public static EnemyDataManagerTwo EnemyManagerTwo;

    private void Awake()
    {
        if (EnemyManagerTwo == null)
        {
            DontDestroyOnLoad(gameObject);
            EnemyManagerTwo = this;
        }
        else if (EnemyManagerTwo != this)
        {
            Destroy(gameObject);
        }
    }

}
