using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMemberController : MonoBehaviour
{
    public PlayerController player;
    public Transform target;
    public float moveSpeed;
    private Animator anim;

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

    void Start()
    {
        anim = GetComponent<Animator>();
        player = FindObjectOfType<PlayerController>();
        anim.speed = 1.5f;

        trueRight = trueLeft = goingLeft = goingDown = goingRight = goingUp = false;

        
    }

    private void Move() {
        
        // Determine the direction and animation to play 
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        if(player.transform.position.x - .4 < transform.position.x &&  transform.position.x < player.transform.position.x + .4) {
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

        if (player.isStill && transform.position == oldPos) {
            if (player.direction == 1) { anim.Play(DownAnimation); }
            else if (player.direction == 2) { anim.Play(UpAnimation); }
            else if (player.direction == 3) { anim.Play(LeftAnimation); }
            else if (player.direction == 4) { anim.Play(RightAnimation); }
            else if (player.direction == 5) { anim.Play(UpLeftAnimation); }
            else if (player.direction == 6) { anim.Play(UpRightAnimation); }
            else if (player.direction == 7) { anim.Play(DownLeftAnimation); }
            else if (player.direction == 8) { anim.Play(DownRightAnimation); }

        }

        if (transform.position != oldPos) {
            if (trueRight) { anim.Play(walkingRightAnimation); }
            else if (trueLeft) { anim.Play(walkingLeftAnimation); }
            else if (goingUp && !goingLeft && !goingRight) { anim.Play(walkingUpAnimation); }
            else if (goingDown && !goingLeft && !goingRight) { anim.Play(walkingDownAnimation); }
            else if (goingLeft && goingUp) { anim.Play(walkingUpLeftAnimation); }
            else if (goingLeft && goingDown) { anim.Play(walkingDownLeftAnimation); }
            else if (goingRight && goingUp) { anim.Play(walkingUpRightAnimation); }
            else if (goingRight && goingDown) { anim.Play(walkingDownRightAnimation); }
        }


    }

   

    void Update()
    {
        oldPos = transform.position;
        Move();
    }
}
