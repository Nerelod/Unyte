using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterSceneOnCollision : MonoBehaviour {

    public string scene;

    public PlayerController player;
    public Vector2 playerPosition;
    public VectorValue playerStorage;

    private void Start() {     
        player = FindObjectOfType<PlayerController>();
    }


    private void OnTriggerEnter2D(Collider2D collision) {    
        if (collision.CompareTag("Player")) {        
            playerStorage.initialValue = playerPosition;
            player.State = States.CannotMove;
            Transitions.screenTransition.StartCoroutine(Transitions.screenTransition.FadeOut(scene, 0.33f));            
        }       
    }


    
    
}
