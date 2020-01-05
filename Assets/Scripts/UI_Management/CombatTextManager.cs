using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CombatTextManager : MonoBehaviour { 


    public static CombatTextManager combatTextManager;

    // The text shown in the text box
    public Text combatText;
    // The text that fades and shows how much player does
    public Text damageText;
    // Text that fades and shows how much damage enemy does
    public Text enemyDamageText;
    // Displays health of combat participants
    public Text junakHealthText;
    public Text saralfHealthText;
    public Text enemyHealthText;
    // Text showing the current order in combat
    public Text orderText;
    // Variables used to make text work and display when needed
    public string combattext;
    public bool textIsFinished;
    public bool pressedSpace;
    public bool textHasBeenPrompt;
    public float textFadeTime = 2;


    
    // Text used for displaying text in textbox
    public void ManageText(string txt) { 
        combattext = txt;
        combatText.text = "";
        StartCoroutine(PlayText());
    }
    // Called in ManageText, plays the text in a typewriter way
    public IEnumerator PlayText() { 
        textIsFinished = false;
        foreach (char c in combattext) { 
            combatText.text += c;
            yield return new WaitForSeconds(0.03f);
        }
        textIsFinished = true;
    }
    // Waits until the user presses space
    public IEnumerator WaitForKeyDown() { 
        pressedSpace = false;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        pressedSpace = true;
    }
    // Makes the text fade, used for damage
    public IEnumerator FadeText(Text theText) { 
        Text text = theText;
        text.color = Color.red;
        Color originalColor = text.color;
        for (float t = 0.01f; t < textFadeTime; t += Time.deltaTime) {
            if (text != null) {
                text.color = Color.Lerp(originalColor, Color.clear, Mathf.Min(1, t / textFadeTime));
            }
            yield return null;
        }
    }
}

