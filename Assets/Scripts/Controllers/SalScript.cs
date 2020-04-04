using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalScript : MonoBehaviour
{
    Animator anim;
    private ActivateTextAtLine activatedText;
    // Use this for initialization
    void Start() {    
        anim = GetComponent<Animator>();
        activatedText = GetComponent<ActivateTextAtLine>();
    }

    void OnTriggerStay2D(Collider2D collision) {     
        if (collision.gameObject.tag == "Player" && Input.GetKeyDown(KeyCode.Space)) {
            /*if (JunakDataManager.Junak.itemManager.aquiredItems.Contains("Health Potion")) {
                activatedText.startLine = 3;
                activatedText.endLine = 4;
            }*/ //THIS IS HOW YOU CHANGE WHAT IS SAID BASED OFF OF CIRCUMSTANCES (IN THIS CASE WHETHER JUNAK HAS A POTION)
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

