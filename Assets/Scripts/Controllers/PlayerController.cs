using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public enum States {CanMove, CannotMove, IsDead}

public class PlayerController : MonoBehaviour {


    public float moveSpeed = 3;
    private Animator anim;
    SpriteRenderer render;
    Rigidbody2D rigid;
    float moveHorizontal;
    float moveVertical;
    public VectorValue startingPosition;
    public States State;
    public GameObject gameMenu;
    private bool gameMenuIsActive;

    

    [SerializeField]

    int direction; //1 =  down, 2 = up, 3 = left, 4 = right, 5 = up left, 6 = up right, 7 = down left, 8 = down right

    void Start () {
        gameMenuIsActive = false;
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        rigid.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rigid.interpolation = RigidbodyInterpolation2D.Extrapolate;
        if (DataManager.manager.isBeingLoaded == true) { 
        
            transform.position = new Vector2(DataManager.manager.xpos, DataManager.manager.ypos);
            DataManager.manager.isBeingLoaded = false;
        }
        else { 
        
            transform.position = startingPosition.initialValue;
        }
    }

    private void Move(){
  
        if (Input.GetAxisRaw("Horizontal") > 0){
            if (Input.GetKey(KeyCode.UpArrow)) {             
                anim.Play("PlayerWalkingUpRight");
                anim.speed = 1.5f;
                direction = 6;
            }
            else if (Input.GetKey(KeyCode.DownArrow)) { 
                anim.Play("PlayerWalkingDownRight");
                anim.speed = 1.5f;
                direction = 8;
            }
            else { 
                anim.Play("PlayerWalkingRight");
                anim.speed = 1.5f;
                direction = 4;
            }
        }
        else if (Input.GetAxisRaw("Horizontal") < 0){
            if (Input.GetKey(KeyCode.UpArrow)) {           
                anim.Play("PlayerWalkingUpLeft");
                anim.speed = 1.5f;
                direction = 5;
            }
            else if (Input.GetKey(KeyCode.DownArrow)) {            
                anim.Play("PlayerWalkingDownLeft");
                anim.speed = 1.5f;
                direction = 7;
            }
            else {           
                anim.Play("PlayerWalkingLeft");
                anim.speed = 1.5f;
                direction = 3;
            }
        }
        else if (Input.GetAxisRaw("Vertical") < 0) {        
            anim.Play("PlayerWalkingDown");
            anim.speed = 1.5f;
            direction = 1;
        }
        else if (Input.GetAxisRaw("Vertical") > 0) {        
            anim.Play("PlayerWalkingUp");
            anim.speed = 1.5f;
            direction = 2;
        }
        else {         
            anim.speed = 0;
            if (direction == 1) {            
                anim.Play("PlayerDown");
            }
            else if(direction == 2) { 
                anim.Play("PlayerUp");
            }
            else if(direction == 3) {           
                anim.Play("PlayerLeft");
            }
            else if(direction == 4) {           
                anim.Play("PlayerRight");
            }
            else if(direction == 5) {            
                anim.Play("PlayerUpLeft");
            }
            else if(direction == 6) {            
                anim.Play("PlayerUpRight");
            }
            else if(direction == 7) {           
                anim.Play("PlayerDownLeft");
            }
            else {           
                anim.Play("PlayerDownRight");
            }
        }
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");
        var moveVector = new Vector3(moveHorizontal, moveVertical, 0);
        rigid.MovePosition(new Vector2((transform.position.x + moveVector.x * moveSpeed * Time.fixedDeltaTime),
                   transform.position.y + moveVector.y * moveSpeed * Time.fixedDeltaTime));

        DataManager.manager.xpos = transform.position.x;
        DataManager.manager.ypos = transform.position.y;

        controlGameMenu();
    }

    void controlGameMenu() { 
    
        if (Input.GetKeyDown(KeyCode.X) && gameMenuIsActive) { //If the gameMenu is active, pressing space turns it off        
            State = States.CanMove;
            gameMenuIsActive = false;
            gameMenu.SetActive(false);
        }
        else if(Input.GetKeyDown(KeyCode.X) && gameMenuIsActive == false) {//If the gameMenu is unactive, pressing space turns it on       
            State = States.CannotMove;
            gameMenuIsActive = true;
            gameMenu.SetActive(true);
        }
    }

    void Update() {

        if (State == States.CanMove) {       
            Move();
        }
        else if(State == States.CannotMove) {        
            controlGameMenu();
            //return;
        }
        else {         
            return;
        }
    }
}
