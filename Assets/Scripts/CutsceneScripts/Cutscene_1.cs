using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene_1 : CutsceneManager
{

    [SerializeField] private GameObject targetOne;
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerController playerController;

    private bool reachedTargetOne;

    void Start()
    {
        playerController.State = States.CannotMove;
        playerController.direction = 1;

        reachedTargetOne = false;
    }

    void Update()
    {
        if (!reachedTargetOne) { moveObject(player, targetOne, 1.5f); playerController.anim.Play("PlayerWalkingDownLeft"); }
        if(player.transform.position == targetOne.transform.position) {
            reachedTargetOne = true;
            playerController.anim.Play("PlayerDown");
            textManager.ManageText("What Now");
        }
    }
}
