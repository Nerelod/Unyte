using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterController : MonoBehaviour { 

    public Transform target;
    public Transform pointOne;
    public Transform pointTwo;
    public float speed;
    public float chaseSpeed;
    private bool inRange;
    private bool pointA;
    private bool goingRight;
    private Animator anim;
    PlayerController player;
    public string animLeft;
    public string animRight;
    public string monsterName;
    public Monster monster;
    public int health;
    public int experienceToGive;
    public Sprite combatSprite;
    public string scene;

    public string kindOfMonster;

    void determineMonster() { 
    
        if (kindOfMonster == "Slime") { 
        
            monster = Monster.Slime;
        }
    }

    void Start() {
        scene = SceneManager.GetActiveScene().name;
        determineMonster();
        inRange = false;
        pointA = true;
        anim = GetComponent<Animator>();
        player = FindObjectOfType<PlayerController>();
        if(EnemyDataManager.EnemyManager.defeatedEnemies.Contains(monsterName)) {
            this.gameObject.SetActive(false);
        }
    }

    void Update() {    
        if (inRange) {         
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            if (player.transform.position.x > transform.position.x) {            
                goingRight = true;
            }
            else {           
                goingRight = false;
            }
        }
        else {       
            if (pointA) { 
                if (transform.position == pointOne.transform.position) {  
                    pointA = false;
                }
                transform.position = Vector2.MoveTowards(transform.position, pointOne.position, speed * Time.deltaTime);
                goingRight = true;

            }
            else { 
            
                if (transform.position == pointTwo.transform.position) { 
                
                    pointA = true;
                }
                transform.position = Vector2.MoveTowards(transform.position, pointTwo.position, speed * Time.deltaTime);
                goingRight = false;
            }
        }

        if (goingRight) { 
        
            anim.Play(animRight);
        }
        else { 
        
            anim.Play(animLeft);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) { 
        inRange = true;      
        transform.position = Vector2.MoveTowards(transform.position, target.position, chaseSpeed * Time.deltaTime);
    }
    private void OnTriggerExit2D(Collider2D collision) { 
        inRange = false;
    }

    private void OnCollisionEnter2D(Collision2D collision) { 
    
        if (collision.gameObject.tag == "Player") {
            EnemyDataManager.EnemyManager.theScene = scene;
            EnemyDataManager.EnemyManager.currentSprite = combatSprite;
            EnemyDataManager.EnemyManager.currentName = monsterName;
            EnemyDataManager.EnemyManager.health = health;
            EnemyDataManager.EnemyManager.experienceGives = experienceToGive;
            EnemyDataManager.EnemyManager.theMonster = monster;            
            SceneManager.LoadScene("CombatScene");
        }
    }
}
