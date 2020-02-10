using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterController : MonoBehaviour
{

    // Reference to the player position
    public Transform target;
    // References to the points the enemy walks to 
    public Transform pointOne;
    public Transform pointTwo;
    // Speed of the monster when traveling to points. Can be changed in editor
    public int speed;
    // Speed to be used in combat
    public int combatSpeed;
    // Speed when monster chases player. Can be changed in editor
    public float chaseSpeed;
    // Determines if the player is in the range of the monsters rangeBox
    private bool inRange;
    // Boolean for if the monster is going towards pointA
    private bool pointA;
    // Boolean for if the monster is traveling to the right
    private bool goingRight;

    public bool canMove;

    public string monsterType;

    private Animator anim;
    // Reference to the player
    public PlayerController player;
    // The files for the animation sprites
    public string animLeft;
    public string animRight;
    // the name of the monster
    public string monsterName;
    // reference to the monster class
    public int monsterIdentifier;
    // The Monster's health 
    public int health;
    // The amount of experience the monster gives when defeated 
    public int experienceToGive;
    // The sprite of the monster when in combat scene
    public Sprite combatSprite;
    // The scene the monster is in
    public string scene;

    void Start()
    {
        canMove = true;
        // Get the scene the monster is in
        scene = SceneManager.GetActiveScene().name;
        // Get the kind of monster
        // Make inRange false so the monster does not immediatley chase
        inRange = false;
        // Start moving to pointA
        pointA = true;

        anim = GetComponent<Animator>();
        player = FindObjectOfType<PlayerController>();
        // If the monster is in the list of defeated monsters, 
        // kill it.
        if (EnemyDataManager.EnemyManager.defeatedEnemies.Contains(monsterName))
        {
            this.gameObject.SetActive(false);
        }

    }
    private void move()
    {
        // Chase the player if inRange is true
        if (inRange)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            // If the player is to the right of the monster, set goingRight true
            if (player.transform.position.x > transform.position.x)
            {
                goingRight = true;
            }
            // If the player is to the left of the monster, set goingRight false
            else
            {
                goingRight = false;
            }
        }
        // if not in range
        else
        {
            // if pointA is true, go to pointA
            if (pointA)
            {
                // if reaches pointOne, set pointA false
                if (transform.position == pointOne.transform.position)
                {
                    pointA = false;
                }
                transform.position = Vector2.MoveTowards(transform.position, pointOne.position, speed * Time.deltaTime);
                goingRight = true;

            }
            // if pointA is false, go to pointTwo
            else
            {
                //If reaches pointTwo, set pointA true
                if (transform.position == pointTwo.transform.position)
                {
                    pointA = true;
                }
                transform.position = Vector2.MoveTowards(transform.position, pointTwo.position, speed * Time.deltaTime);
                goingRight = false;
            }
        }
        // if going right, play the going right animation
        if (goingRight)
        {

            anim.Play(animRight);
        }
        //if going left, play the going left animation
        else
        {
            anim.Play(animLeft);
        }
    }
    void Update()
    {
        if (canMove) { move(); }
    }

    // If something collides in trigegrbox, set inRange true and move toward 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!player.isInvincible)
        {
            inRange = true;
            transform.position = Vector2.MoveTowards(transform.position, target.position, chaseSpeed * Time.deltaTime);
        }
    }
    // If something leaves the triggerbox, set inRange false
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!player.isInvincible)
        {
            inRange = false;
        }
    }
}
