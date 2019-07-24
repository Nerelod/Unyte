using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Cutscene_1 : CutsceneManager
{

    [SerializeField] private GameObject targetOne;
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerController playerController;

    private bool reachedTargetOne;
    private bool textHasEnded;

    void Start()
    {
        playerController.State = States.CannotMove;
        playerController.anim.Play("PlayerDown");

        reachedTargetOne = false;
        waitIsFinished = false;
        textHasEnded = false;
        StartCoroutine(Wait(1));
    }

    void Update()
    {
        if (waitIsFinished) {
            if (!reachedTargetOne) { moveObject(player, targetOne, 1.5f); playerController.anim.Play("PlayerWalkingDownLeft"); }
            if (player.transform.position == targetOne.transform.position && !textHasEnded) {
                reachedTargetOne = true;
                playerController.anim.Play("PlayerDown");
                textManager.ManageText("What Now");
                StartCoroutine(textManager.WaitForKeyDown());
                textHasEnded = true;
            }
            if (textManager.pressedSpace) {
                DataManager.playerOne.xpos = -0.3f;
                DataManager.playerOne.ypos = -1.07f;
                DataManager.playerOne.isBeingLoaded = true;
                SceneManager.LoadScene("Player'sHouseScene");
            }
        }
    }
}
