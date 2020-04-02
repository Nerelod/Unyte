using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedController : PartyMemberController
{
    void Start() {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<PlayerController>();
        saralf = FindObjectOfType<SaralfController>();
        anim.speed = 1.5f;
        xOffSet = .4f;
        yOffSet = 0.2f;
        orderOffSet = 2;
        trueRight = trueLeft = goingLeft = goingDown = goingRight = goingUp = false;
        RedDataManager.Red.isInParty = true; // TODO: Make condition for when to add member to party
        rigid.freezeRotation = true;

        transform.position = target.transform.position;

    }


    void Update() {
        oldPos = transform.position;
        if (RedDataManager.Red.isInParty) {
            Move();
            Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            Physics2D.IgnoreCollision(saralf.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
        else { Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false); }
    }
}
