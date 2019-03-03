using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CombatTextManager : MonoBehaviour { 


    public static CombatTextManager combatTextManager;

    public Text combatText;
    public Text damageText;
    public Text enemyDamageText;
    public string combattext;
    public bool textIsFinished;
    public bool pressedSpace;
    public bool textHasBeenPrompt;
    public float textFadeTime = 2;


    

    public void ManageText(string txt) { 
        combattext = txt;
        combatText.text = "";
        StartCoroutine(PlayText());
    }

    public IEnumerator PlayText() { 
        textIsFinished = false;
        foreach (char c in combattext) { 
            combatText.text += c;
            yield return new WaitForSeconds(0.03f);
        }
        textIsFinished = true;
    }
    public IEnumerator Wait(int seconds) { 
        textIsFinished = false;
        yield return new WaitForSeconds(seconds);
        textIsFinished = true;
    }
    public IEnumerator WaitForKeyDown() { 
        pressedSpace = false;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        pressedSpace = true;
    }
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

