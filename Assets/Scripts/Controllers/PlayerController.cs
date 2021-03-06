﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

// These are the states the player can be in
public enum States { CanMove, CannotMove, IsDead }

public class PlayerController : MonoBehaviour {

    // The player movement speed, can be changed in the editor
    public float moveSpeed = 1.5f;
    public float runSpeed = 2.5f;
    public float walkSpeed = 1.5f;
    public float walkAnimSpeed = 1.5f;
    public float runAnimSpeed = 1.8f;
    public float animSpeed = 1.5f;
    private bool running = false;
    private Vector3 change;
    // A reference to the animator, used for animating
    public Animator anim;
    // The sprite image
    public SpriteRenderer render;
    // A reference to the sprite's rigid body, used for detecting collision
    Rigidbody2D rigid;
    // The x part of the vector that determines movement
    float moveHorizontal;
    // The y part of the vector that determines movement
    float moveVertical;
    // Where the player starts
    public VectorValue startingPosition;
    // An instance of the enum States
    public States State;
    // The in-game menu
    //public GameObject gameMenu;
    // Boolean that determines when the in-game menu is open
    private bool gameMenuIsActive;
    public bool isInvincible;
    public bool isStill;

    [SerializeField]

    public int direction; //1 =  down, 2 = up, 3 = left, 4 = right, 5 = up left, 6 = up right, 7 = down left, 8 = down right

    private void Awake() {
        isInvincible = false;
        
        if (JunakDataManager.Junak.isBeingLoaded == true) {
            transform.position = new Vector2(JunakDataManager.Junak.xpos, JunakDataManager.Junak.ypos);
            JunakDataManager.Junak.isBeingLoaded = false;
            GameMenuManager.gameMenuManager.gameMenu.SetActive(false);
            if (JunakDataManager.Junak.ranFromCombat) {
                StartCoroutine(invincibilityTimer());
                JunakDataManager.Junak.ranFromCombat = false;
            }
        }    
        else {
            transform.position = startingPosition.initialValue;
            direction = JunakDataManager.Junak.directionHolder;
        }
    }
    void Start() {

        render = GetComponent<SpriteRenderer>();
        // Make the in-game menu hidden at the start
        gameMenuIsActive = false;
        // Assign the animator
        anim = GetComponent<Animator>();
        // Assign the rigidbody
        rigid = GetComponent<Rigidbody2D>();
        rigid.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rigid.interpolation = RigidbodyInterpolation2D.Extrapolate;
        
    }

    // This method is always run when the player is in the CanMove state
    private void Move() {
        // Determine the direction and animation to play 
        // Based on input
        if (Input.GetKey(KeyCode.Z)) {
            running = !running;
        }
        if (running) {
            moveSpeed = runSpeed;
            animSpeed = runAnimSpeed;
        }
        else {
            moveSpeed = walkSpeed;
            animSpeed = walkAnimSpeed;
        }
        if (Input.GetAxisRaw("Horizontal") > 0) {
            isStill = false;
            if (Input.GetKey(KeyCode.UpArrow)) {
                anim.Play("PlayerWalkingUpRight");
                anim.speed = animSpeed;
                direction = 6;
            }
            else if (Input.GetKey(KeyCode.DownArrow)) {
                anim.Play("PlayerWalkingDownRight");
                anim.speed = animSpeed;
                direction = 8;
            }
            else {
                anim.Play("PlayerWalkingRight");
                anim.speed = animSpeed;
                direction = 4;
            }
        }
        else if (Input.GetAxisRaw("Horizontal") < 0) {
            isStill = false;
            if (Input.GetKey(KeyCode.UpArrow)) {
                anim.Play("PlayerWalkingUpLeft");
                anim.speed = animSpeed;
                direction = 5;
            }
            else if (Input.GetKey(KeyCode.DownArrow)) {
                anim.Play("PlayerWalkingDownLeft");
                anim.speed = animSpeed;
                direction = 7;
            }
            else {
                anim.Play("PlayerWalkingLeft");
                anim.speed = animSpeed;
                direction = 3;
            }
        }
        else if (Input.GetAxisRaw("Vertical") < 0) {
            isStill = false;
            anim.Play("PlayerWalkingDown");
            anim.speed = animSpeed;
            direction = 1;
        }
        else if (Input.GetAxisRaw("Vertical") > 0) {
            isStill = false;
            anim.Play("PlayerWalkingUp");
            anim.speed = animSpeed;
            direction = 2;
        }
        // Determine the sprite animation pose
        // When standing still (no input)
        else {
            isStill = true;
            anim.speed = 0;
            if (direction == 1) {
                anim.Play("PlayerDown");
            }
            else if (direction == 2) {
                anim.Play("PlayerUp");
            }
            else if (direction == 3) {
                anim.Play("PlayerLeft");
            }
            else if (direction == 4) {
                anim.Play("PlayerRight");
            }
            else if (direction == 5) {
                anim.Play("PlayerUpLeft");
            }
            else if (direction == 6) {
                anim.Play("PlayerUpRight");
            }
            else if (direction == 7) {
                anim.Play("PlayerDownLeft");
            }
            else {
                anim.Play("PlayerDownRight");
            }
        }


        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");
        /*var moveVector = new Vector3(moveHorizontal, moveVertical, 0);
        rigid.MovePosition(new Vector2((transform.position.x + moveVector.x * moveSpeed * Time.fixedDeltaTime),
                   transform.position.y + moveVector.y * moveSpeed * Time.fixedDeltaTime));*/
        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
        if(change != Vector3.zero) {
            rigid.MovePosition(transform.position + change.normalized * moveSpeed * Time.fixedDeltaTime);
        }


        JunakDataManager.Junak.xpos = transform.position.x;
        JunakDataManager.Junak.ypos = transform.position.y;

        

        // Determine when to trigger the in-game menu
        controlGameMenu();
    }
    // Called in the Move method
    void controlGameMenu() {
        // If the gameMenu is active, pressing X turns it off
        if (GameMenuManager.gameMenuManager.canInteractWith)
        {
            if (Input.GetKeyDown(KeyCode.X) && gameMenuIsActive)
            {
                State = States.CanMove; // Resume motion after resuming the game
                gameMenuIsActive = false;
                GameMenuManager.gameMenuManager.gameMenu.SetActive(gameMenuIsActive);

            }
            // If the gameMenu is unactive, pressing X turns it on   
            else if (Input.GetKeyDown(KeyCode.X) && gameMenuIsActive == false)
            {
                State = States.CannotMove; // Player cannot move when in-game menu is on
                gameMenuIsActive = true;
                GameMenuManager.gameMenuManager.gameMenu.SetActive(gameMenuIsActive);
                GameMenuManager.gameMenuManager.whenTurnedOn();

            }
        }
    }

    public IEnumerator invincibilityTimer() {
        isInvincible = true;
        yield return new WaitForSeconds(3);
        isInvincible = false;
    }
    void FixedUpdate() {
        // State controller
        if (State == States.CanMove) {
            Move();
        }
        else if (State == States.CannotMove) {
            // controlGameMenu() so that the player can turn the in-game menu off
            controlGameMenu();
        }
        else {
            return;
        }
    }
}