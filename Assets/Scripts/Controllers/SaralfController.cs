using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaralfController : PartyMemberController
{
    
    void Start()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<PlayerController>();
        anim.speed = 1.5f;

        trueRight = trueLeft = goingLeft = goingDown = goingRight = goingUp = false;
        SaralfDataManager.Saralf.isInParty = false; // TODO: Make condition for when to add member to party
        rigid.freezeRotation = true;

        transform.position = target.transform.position;
        
    }

    
    void Update()
    {
        oldPos = transform.position;
        if (SaralfDataManager.Saralf.isInParty) { Move(); Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>()); }
        else { Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false); }
    }
}
