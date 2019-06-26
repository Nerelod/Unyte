using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMemberController : MonoBehaviour
{
    public PlayerController player;
    public Transform target;
    public float moveSpeed;
    private Animator anim;

    private int direction;

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
        if(player.transform.position.x - .6 < transform.position.x &&  transform.position.x < player.transform.position.x + .6) {
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
        
        Debug.Log("Going Right: " + goingRight.ToString() + " Going Up: " + goingUp.ToString() + " Going True Right: " + trueRight.ToString() + " Going Down: " + goingDown.ToString() + player.transform.position.y.ToString() + " " + transform.position.y.ToString());

        if (trueRight) { anim.Play("Saralf Walking Right"); }
        else if (trueLeft) { anim.Play("Saralf Walking Left"); }
        else if (goingUp && !goingLeft && !goingRight) { anim.Play("Saralf Walking Up"); }
        else if (goingDown && !goingLeft && !goingRight) { anim.Play("Saralf Walking Down"); }
        else if (goingLeft && goingUp) { anim.Play("Saralf Walking Up Left"); }
        else if (goingLeft && goingDown) { anim.Play("Saralf Walking Down Left"); }
        else if (goingRight && goingUp) { anim.Play("Saralf Walking Up Right"); }
        else if (goingRight && goingDown) { anim.Play("Saralf Walking Down Right"); }


    }

    void Update()
    {
        Move();
    }
}
