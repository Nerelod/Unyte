using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalScript : MonoBehaviour
{
    Animator anim;
    // Use this for initialization
    void Start() {    
        anim = GetComponent<Animator>();
    }

    void OnTriggerStay2D(Collider2D collision) {     
        if (collision.gameObject.tag == "Player" && Input.GetKeyDown(KeyCode.Space)) {         
            anim.Play("SalExpressionOne");
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {     
        if (collision.gameObject.tag == "Player") { 
            anim.Play("SalExpressionDefault");
        }
    }
    
    void Update(){ 
    

    }
}

