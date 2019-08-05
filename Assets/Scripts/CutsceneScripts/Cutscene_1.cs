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
    private bool readyToSwitchScenes;

    void Start()
    {
        playerController.State = States.CannotMove;
        playerController.anim.Play("PlayerDown");

        readyToSwitchScenes = false;
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
                textManager.ManageText("Junak: *It's already noon.*");
                StartCoroutine(textManager.WaitForKeyDown());
                textHasEnded = true;
            }
            if(textManager.pressedSpace && textManager.textIsFinished && !readyToSwitchScenes && textHasEnded) {
                textManager.ManageText("Junak: *I should go visit Paul in town.*");
                StartCoroutine(textManager.WaitForKeyDown());
                readyToSwitchScenes = true;
            }
            if (textManager.pressedSpace && textManager.textIsFinished && readyToSwitchScenes) {
                DataManager.playerOne.xpos = -0.3f;
                DataManager.playerOne.ypos = -1.07f;
                DataManager.playerOne.isBeingLoaded = true;
                SceneManager.LoadScene("Player'sHouseScene");
            }
        }
    }
}
