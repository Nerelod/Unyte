using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMemberController : MonoBehaviour
{
    public PlayerController player;
    public Transform target;
    public float moveSpeed;
    private Animator anim;
    private Rigidbody2D rigid;

    private Vector3 oldPos;

    public string LeftAnimation;
    public string RightAnimation;
    public string UpAnimation;
    public string DownAnimation;
    public string UpLeftAnimation;
    public string UpRightAnimation;
    public string DownRightAnimation;
    public string DownLeftAnimation;
    public string walkingLeftAnimation;
    public string walkingRightAnimation;
    public string walkingUpAnimation;
    public string walkingDownAnimation;
    public string walkingUpLeftAnimation;
    public string walkingUpRightAnimation;
    public string walkingDownRightAnimation;
    public string walkingDownLeftAnimation;

    private bool goingRight, goingUp, goingLeft, goingDown, trueRight, trueLeft;
    private bool isInParty;
    private SpriteRenderer sprite;

    void Start()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<PlayerController>();
        anim.speed = 1.5f;

        trueRight = trueLeft = goingLeft = goingDown = goingRight = goingUp = false;
        isInParty = true; // TODO: Make condition for when to add member to party
        rigid.freezeRotation = true;
    }


    private void Move() {

        // Determine the direction and animation to play 
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.fixedDeltaTime);
        
        if (player.transform.position.x - .4 < transform.position.x &&  transform.position.x < player.transform.position.x + .4) {
            goingRight = false;
            goingLeft = false;
            trueRight = false;
            trueLeft = false;
        }
        else if (player.transform.position.x > transform.position.x) {
            if (player.transform.position.y - .2 < transform.position.y && transform.position.y < player.transform.position.y + .2) {
                trueRight = true;
                trueLeft = false;
                goingRight = false;
                goingLeft = false;
            }
            else {
                trueRight = false;
                goingRight = true;
                goingLeft = false;
            }
        }
        else if (player.transform.position.x < transform.position.x){
            if (player.transform.position.y - .2 < transform.position.y && transform.position.y < player.transform.position.y + .2) {
                trueLeft = true;
                trueRight = false;
                goingRight = false;
                goingLeft = false;
            }
            else {
                trueLeft = false;
                goingRight = false;
                goingLeft = true;
            }
        }

        if (transform.position.y < player.transform.position.y) {
            goingUp = true;
            goingDown = false;
        }
        else if (transform.position.y > player.transform.position.y){
            goingUp = false;
            goingDown = true;
        }
        else {
            goingUp = false;
            goingDown = false;
        }

        if (player.isStill && transform.position == oldPos) { // if still
            if (player.direction == 1) { anim.Play(DownAnimation); }
            else if (player.direction == 2) { anim.Play(UpAnimation); }
            else if (player.direction == 3) { anim.Play(LeftAnimation); }
            else if (player.direction == 4) { anim.Play(RightAnimation); }
            else if (player.direction == 5) { anim.Play(UpLeftAnimation); }
            else if (player.direction == 6) { anim.Play(UpRightAnimation); }
            else if (player.direction == 7) { anim.Play(DownLeftAnimation); }
            else if (player.direction == 8) { anim.Play(DownRightAnimation); }

        }

        if(transform.position.y < player.transform.position.y) { // if below
            sprite.sortingOrder = 2;
        }
        else { // if above
            sprite.sortingOrder = 1; 
        }

        if (transform.position != oldPos) { // if moving
            if (trueRight) { anim.Play(walkingRightAnimation); moveSpeed = 1.5f; } // going right
            else if (trueLeft) { anim.Play(walkingLeftAnimation); moveSpeed = 1.5f; } // going left
            else if (goingUp && !goingLeft && !goingRight) { anim.Play(walkingUpAnimation); moveSpeed = 1.5f; } // going up
            else if (goingDown && !goingLeft && !goingRight) { anim.Play(walkingDownAnimation); moveSpeed = 1.5f; } // going down
            else if (goingLeft && goingUp) { anim.Play(walkingUpLeftAnimation); moveSpeed = 2.0f; } // going up left
            else if (goingLeft && goingDown) { anim.Play(walkingDownLeftAnimation); moveSpeed = 2.0f; }// going down left
            else if (goingRight && goingUp) { anim.Play(walkingUpRightAnimation); moveSpeed = 2.0f; } // going up right
            else if (goingRight && goingDown) { anim.Play(walkingDownRightAnimation); moveSpeed = 2.0f; } // going down right
        }


    }

    

    void Update()
    {
        oldPos = transform.position;
        if (isInParty) { Move(); Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>()); }
        else { Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false); }
    }
}
