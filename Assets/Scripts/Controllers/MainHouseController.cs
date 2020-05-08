using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainHouseController : MonoBehaviour {
    
    SpriteRenderer render;
    Rigidbody2D rigid;
    [SerializeField] private string animationOnEnter;
    [SerializeField] private string animationOnExit;
    Animator anim;
    

    void Start () {
        anim = GetComponent<Animator>();
    }

    

    private void OnTriggerEnter2D(Collider2D collision) {   
        if (collision.CompareTag("Player")) {        
            anim.Play(animationOnEnter);
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            anim.Play(animationOnExit);
        }
    }




    void Update () {
       
    }
}
