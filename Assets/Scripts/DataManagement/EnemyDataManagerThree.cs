using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDataManagerThree : EnemyDataManager
{
    public static EnemyDataManagerThree EnemyManagerThree;
    private void Awake()
    {
        if (EnemyManagerThree == null)
        {
            DontDestroyOnLoad(gameObject);
            EnemyManagerThree = this;
        }
        else if (EnemyManagerThree != this)
        {
            Destroy(gameObject);
        }
    }
}
