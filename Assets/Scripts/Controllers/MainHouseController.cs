using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainHouseController : MonoBehaviour {


   
    SpriteRenderer render;
    Rigidbody2D rigid;

    Animator anim;
    

    void Start () {
        anim = GetComponent<Animator>();
    }

    

    private void OnTriggerEnter2D(Collider2D collision) {   
        if (collision.CompareTag("Player")) { 
        
            anim.Play("ClosedDoortoMainHouse");
        }
    }

    


    void Update () {
       
    }
}
