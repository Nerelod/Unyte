using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{

    public bool textWasPrompt = false;
    public bool displayedDamage = false;
    protected MonsterController monsterController;

    public int whichAttack;

    public bool hasSecondMonster;
    public bool hasThirdMonster;

    public Monster()
    {

    }

    private void assignMonsterValues(SecondaryMonster monster, EnemyDataManager enemyManager)
    {
        enemyManager.combatIcon = monster.iconSprite;
        enemyManager.combatSprite = monster.combatSprite;
        enemyManager.currentName = monster.monsterName;
        enemyManager.health = monster.health;
        enemyManager.experienceGives = monster.experienceToGive;
        enemyManager.speed = monster.speed;
        EnemyDataManager.EnemyManager.amountOfEnemies = 2;
        enemyManager.theMonster = this;
    }

    public virtual void Attack(DataManager target, EnemyDataManager monster) { }
    public virtual void DisplayDamage(DataManager target) { }
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && !monsterController.player.isInvincible)
        {
            EnemyDataManager.EnemyManager.theScene = monsterController.scene;
            EnemyDataManager.EnemyManager.combatIcon = monsterController.combatSprite;
            EnemyDataManager.EnemyManager.combatSprite = monsterController.combatSprite;
            EnemyDataManager.EnemyManager.currentName = monsterController.monsterName;
            EnemyDataManager.EnemyManager.health = monsterController.health;
            EnemyDataManager.EnemyManager.experienceGives = monsterController.experienceToGive;
            EnemyDataManager.EnemyManager.speed = monsterController.speed;
            EnemyDataManager.EnemyManager.currentType = monsterController.monsterType;
            EnemyDataManager.EnemyManager.theMonster = this;
            monsterController.canMove = false;
            EnemyDataManager.EnemyManager.amountOfEnemies = 1;
            monsterController.player.State = States.CannotMove;
            if (hasSecondMonster)
            {
                SecondaryMonster secondMonster = GetComponent<SecondaryMonster>();
                assignMonsterValues(secondMonster, EnemyDataManagerTwo.EnemyManagerTwo);
            }
            if (hasThirdMonster)
            {
                ThirdMonster thirdMonster = GetComponent<ThirdMonster>();
                assignMonsterValues(thirdMonster, EnemyDataManagerThree.EnemyManagerThree);
            }
            Transitions.screenTransition.StartCoroutine(Transitions.screenTransition.FadeOut("CombatScene", .33f));
        }
    }
}


