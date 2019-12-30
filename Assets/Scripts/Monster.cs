using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour{ 

    public bool textWasPrompt = false;
    public bool displayedDamage = false;
    protected MonsterController monsterController;
    
    public int whichAttack;    

    public Monster() {

    }

    public virtual void Attack(DataManager target){}
    public virtual void DisplayDamage(DataManager target){}
    protected void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player" && !monsterController.player.isInvincible) {
            EnemyDataManager.EnemyManager.theScene = monsterController.scene;
            EnemyDataManager.EnemyManager.currentSprite = monsterController.combatSprite;
            EnemyDataManager.EnemyManager.currentName = monsterController.monsterName;
            EnemyDataManager.EnemyManager.health = monsterController.health;
            EnemyDataManager.EnemyManager.experienceGives = monsterController.experienceToGive;
            EnemyDataManager.EnemyManager.speed = monsterController.speed;
            EnemyDataManager.EnemyManager.currentType = monsterController.monsterType;
            EnemyDataManager.EnemyManager.theMonster = this;
            monsterController.canMove = false;
            monsterController.player.State = States.CannotMove;
            Transitions.screenTransition.StartCoroutine(Transitions.screenTransition.FadeOut("CombatScene", .33f));
        }
    }
}
    

