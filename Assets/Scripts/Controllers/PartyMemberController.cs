using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMemberController : MonoBehaviour {
    protected PlayerController player;
    protected SaralfController saralf;
    protected RedController red;
    public Transform target;
    public float moveSpeed = 1.5f;
    public float xOffSet;
    public float yOffSet;
    public int orderOffSet;
    protected Animator anim;
    protected Rigidbody2D rigid;
    public int followDistance;
    protected List<Vector3> storedPositions;
    protected Vector3 oldPos;

    [SerializeField] protected string LeftAnimation;
    [SerializeField] protected string RightAnimation;
    [SerializeField] protected string UpAnimation;
    [SerializeField] protected string DownAnimation;
    [SerializeField] protected string UpLeftAnimation;
    [SerializeField] protected string UpRightAnimation;
    [SerializeField] protected string DownRightAnimation;
    [SerializeField] protected string DownLeftAnimation;
    [SerializeField] protected string walkingLeftAnimation;
    [SerializeField] protected string walkingRightAnimation;
    [SerializeField] protected string walkingUpAnimation;
    [SerializeField] protected string walkingDownAnimation;
    [SerializeField] protected string walkingUpLeftAnimation;
    [SerializeField] protected string walkingUpRightAnimation;
    [SerializeField] protected string walkingDownRightAnimation;
    [SerializeField] protected string walkingDownLeftAnimation;

    public bool goingRight, goingUp, goingLeft, goingDown, trueRight, trueLeft;
    public SpriteRenderer sprite;

    void Start()
    {
        
    }


    public void Move() {
        anim.speed = player.anim.speed;
        moveSpeed = player.moveSpeed;
        // Determine the direction and animation to play 
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.fixedDeltaTime);


        if (player.transform.position.x - xOffSet < transform.position.x &&  transform.position.x < player.transform.position.x + xOffSet) {
            goingRight = false;
            goingLeft = false;
            trueRight = false;
            trueLeft = false;
        }
        else if (player.transform.position.x > transform.position.x) {
            if (player.transform.position.y - yOffSet < transform.position.y && transform.position.y < player.transform.position.y + yOffSet) {
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
            if (player.transform.position.y - yOffSet < transform.position.y && transform.position.y < player.transform.position.y + yOffSet) {
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
            sprite.sortingOrder = player.render.sortingOrder + orderOffSet;
        }
        else { // if above
            sprite.sortingOrder = player.render.sortingOrder - orderOffSet; 
        }

        if (transform.position != oldPos) { // if moving
            if (trueRight) { anim.Play(walkingRightAnimation); }
            else if (trueLeft) { anim.Play(walkingLeftAnimation); } // going left
            else if (goingUp && !goingLeft && !goingRight) { anim.Play(walkingUpAnimation);} // going up
            else if (goingDown && !goingLeft && !goingRight) { anim.Play(walkingDownAnimation);} // going down
            else if (goingLeft && goingUp) { anim.Play(walkingUpLeftAnimation); } // going up left
            else if (goingLeft && goingDown) { anim.Play(walkingDownLeftAnimation);  }// going down left
            else if (goingRight && goingUp) { anim.Play(walkingUpRightAnimation); } // going up right
            else if (goingRight && goingDown) { anim.Play(walkingDownRightAnimation);} // going down right
        }
        
    }

    

    void Update()
    {
        
    }
}
