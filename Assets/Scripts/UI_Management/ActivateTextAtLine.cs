using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTextAtLine : MonoBehaviour {

    public TextAsset theText;

    public int startLine;
    public int endLine;

    public TextBoxManager theTextManager;

    public bool destroyWhenActivated;
    public bool requireButtonPress;
    private bool waitForPress;
	
	void Start () {
        theTextManager = FindObjectOfType<TextBoxManager>();
	}

    void OnTriggerEnter2D(Collider2D collision) { 
        if(collision.name == "Player") {
            if (requireButtonPress) {
                waitForPress = true;
                return;
            }
            theTextManager.ReloadScript(theText);
            theTextManager.currentLine = startLine;
            theTextManager.endAtLine = endLine;
            theTextManager.EnableTextBox();
            if (destroyWhenActivated) {
                Destroy(gameObject);
            }
        }
    }
    void OnTriggerExit2D(Collider2D collision) { 
        if(collision.name == "Player") {
            waitForPress = false;
        }
    }
    void Update () {
        if (waitForPress && Input.GetKeyDown(KeyCode.Space) && theTextManager.boxActive == false) {
            theTextManager.ReloadScript(theText);
            theTextManager.currentLine = startLine;
            theTextManager.endAtLine = endLine;
            theTextManager.EnableTextBox();
            if (destroyWhenActivated) { 
                Destroy(gameObject);
            }
        }
	}  
}
