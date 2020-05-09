using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaralfController : PartyMemberController
{
    private void Awake() {
        storedPositions = new List<Vector3>(); //create a blank list
    }
    void Start()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<PlayerController>();
        red = FindObjectOfType<RedController>();
        anim.speed = 1.5f;
        xOffSet = 0.4f;
        yOffSet = 0.2f;
        orderOffSet = 1;
        trueRight = trueLeft = goingLeft = goingDown = goingRight = goingUp = false;
        SaralfDataManager.Saralf.isInParty = true; // TODO: Make condition for when to add member to party
        rigid.freezeRotation = true;

        transform.position = target.transform.position;

    }


    void FixedUpdate()
    {
        oldPos = transform.position;
        if (SaralfDataManager.Saralf.isInParty) {
            Move();
            Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            if (RedDataManager.Red.isInParty) { Physics2D.IgnoreCollision(red.GetComponent<Collider2D>(), GetComponent<Collider2D>()); }
        }
        else { Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false); }
    }
}
