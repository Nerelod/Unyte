using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxManager : MonoBehaviour {

    public GameObject theBox;

    public Text theText;

    public TextAsset textFile;

    public string[] textLines;

    public int currentLine;
    public int endAtLine;

    public PlayerController player;

    public bool boxActive;
    public bool stopPlayerMovement;
    private bool isTyping = false;
    private bool cancelTyping = false;

    public float typeSpeed;

    void Start() { 

        player = FindObjectOfType<PlayerController>();

        if (textFile != null) { 
            textLines = (textFile.text.Split('\n'));
        }
        if(endAtLine == 0) { 
            endAtLine = textLines.Length - 1;
        }

        if (boxActive) { 
            EnableTextBox();
        }
        else {
            Debug.Log("Disabeling TextBox");
            DisableTextBox();
        }
    }

    
    void Update () {

        if (!boxActive) { 
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && boxActive == true) { 
            if (!isTyping) { 
                currentLine += 1;

                if (currentLine > endAtLine) { 
                    DisableTextBox();
                }
                else { 
                    StartCoroutine(TextScroll(textLines[currentLine]));
                }
            }
            else if(isTyping && !cancelTyping) { 
                cancelTyping = true;
            }
        }
        

	}

    private IEnumerator TextScroll(string lineOfText) { 
        int letter = 0;
        theText.text = "";
        isTyping = true;
        cancelTyping = false;
        while (isTyping && !cancelTyping && (letter < lineOfText.Length - 1)) { 
            theText.text += lineOfText[letter];
            letter += 1;
            yield return new WaitForSeconds(typeSpeed);
        }
        theText.text = lineOfText;
        isTyping = false;
        cancelTyping = false;
    }

    public void EnableTextBox() { 
        boxActive = true;
        theBox.SetActive(true);
        if (stopPlayerMovement) { 
            player.State = States.CannotMove;
        }
        StartCoroutine(TextScroll(textLines[currentLine]));
    }
    public void DisableTextBox() {         
        boxActive = false;
        theBox.SetActive(false);
        player.State = States.CanMove;
        
    }
    public void ReloadScript(TextAsset theText) { 
        if(theText != null) { 
            textLines = new string[1];
            textLines = (theText.text.Split('\n'));
        }
    }
}
