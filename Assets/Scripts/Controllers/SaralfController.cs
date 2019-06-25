using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaralfController : MonoBehaviour
{
    public PlayerController player;
    public Transform target;
    public int moveSpeed;
    private Animator anim;
    Rigidbody2D rigid;

    private int direction;

    void Start()
    {
        anim = GetComponent<Animator>();
        player = FindObjectOfType<PlayerController>();
        anim.speed = 1.5f;
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Move() {
        // Determine the direction and animation to play 
        // Based on input
        if (Input.GetAxisRaw("Horizontal") > 0) {
            if (Input.GetKey(KeyCode.UpArrow)) {
                anim.Play("Saralf Walking Up Right");
                anim.speed = 1.5f;
                direction = 6;
            }
            else if (Input.GetKey(KeyCode.DownArrow)) {
                anim.Play("Saralf Walking Down Right");
                anim.speed = 1.5f;
                direction = 8;
            }
            else {
                anim.Play("Saralf Walking Right");
                anim.speed = 1.5f;
                direction = 4;
            }
        }
        else if (Input.GetAxisRaw("Horizontal") < 0) {
            if (Input.GetKey(KeyCode.UpArrow)) {
                anim.Play("Saralf Walking Up Left");
                anim.speed = 1.5f;
                direction = 5;
            }
            else if (Input.GetKey(KeyCode.DownArrow)) {
                anim.Play("Saralf Walking Down Left");
                anim.speed = 1.5f;
                direction = 7;
            }
            else {
                anim.Play("Saralf Walking Left");
                anim.speed = 1.5f;
                direction = 3;
            }
        }
        else if (Input.GetAxisRaw("Vertical") < 0) {
            anim.Play("Saralf Walking Down");
            anim.speed = 1.5f;
            direction = 1;
        }
        else if (Input.GetAxisRaw("Vertical") > 0) {
            anim.Play("Saralf Walking Up");
            anim.speed = 1.5f;
            direction = 2;
        }
        // Determine the sprite animation pose
        // When standing still (no input)
        else {
            anim.speed = 0;
            if (direction == 1) {
                anim.Play("Saralf Down");
            }
            else if (direction == 2) {
                anim.Play("Saralf Up");
            }
            else if (direction == 3) {
                anim.Play("Saralf Left");
            }
            else if (direction == 4) {
                anim.Play("Saralf Right");
            }
            else if (direction == 5) {
                anim.Play("Saralf Up Left");
            }
            else if (direction == 6) {
                anim.Play("Saralf Up Right");
            }
            else if (direction == 7) {
                anim.Play("Saralf Down Left");
            }
            else {
                anim.Play("Saralf Down Right");
            }
        }


        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");
        var moveVector = new Vector3(moveHorizontal, moveVertical, 0);
        rigid.MovePosition(new Vector2((transform.position.x + moveVector.x * moveSpeed * Time.fixedDeltaTime),
                   transform.position.y + moveVector.y * moveSpeed * Time.fixedDeltaTime));
    }

    void Update()
    {
        Move();
    }
}
