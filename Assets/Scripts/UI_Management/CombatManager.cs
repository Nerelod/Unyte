using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CombatStates {PlayerOneAttacking, EnemyAttacking, ResetValues}
public enum CombatOptions { Attack, Ability, Item, Run, HasNotChosen}

public class CombatManager : MonoBehaviour
{

    public GameObject enemySprite;
    CombatStates combatState;
    CombatOptions playerOneOption;
    //CombatOptions for each char in party
    private int order;
    private int playerOneChosenOrder;
    public Text combatText;
    public Text damageText;
    string combattext;
    private bool textIsFinished;
    private bool pressedSpace;
    private bool textHasBeenPrompt;
    private float textFadeTime = 2;
    void Start()
    {
        damageText.text = "";
        order = 1;
        textHasBeenPrompt = false;
        pressedSpace = false;
        playerOneChosenOrder = 0;
        playerOneOption = CombatOptions.HasNotChosen;
        ManageText("A " + EnemyDataManager.EnemyManager.currentName + " appeared!");
        StartCoroutine(WaitForKeyDown());
        enemySprite = GameObject.Find("Enemy");
        enemySprite.GetComponent<SpriteRenderer>().sprite = EnemyDataManager.EnemyManager.currentSprite;

        if(DataManager.manager.speed > EnemyDataManager.EnemyManager.speed)
        {
            combatState = CombatStates.PlayerOneAttacking;
            EnemyDataManager.EnemyManager.assignedOrderInCombat = 2;
        }
        else
        {
            combatState = CombatStates.EnemyAttacking;
            EnemyDataManager.EnemyManager.assignedOrderInCombat = 1;
        }
    }

    void ManageText(string txt)
    {       
        combattext = txt;
        combatText.text = "";
        StartCoroutine(PlayText());
    }

    private void GetPlayerAction(int player)
    {
        if (textIsFinished && !textHasBeenPrompt && pressedSpace)
        {
            ManageText("Choose an Action");
            textHasBeenPrompt = true;
        }
        if(player == 1 && textHasBeenPrompt)
        {
            if (Input.GetKeyDown(KeyCode.Q) && playerOneOption == CombatOptions.HasNotChosen)
            {  
                playerOneOption = CombatOptions.Attack;
                ManageText("Choose Order To Act");
            }
        }
    }

    private int Attack(EnemyDataManager enemy, DataManager character)
    {
        ManageText("Player does " + character.qDamage.ToString() + " damage!");
        //play enemy sprite damaged animation
        damageText.text = "-" + character.qDamage.ToString();
        StartCoroutine(FadeText());
        StartCoroutine(WaitForKeyDown());
        return enemy.health -= character.qDamage;
    }

    private void GetOrder(int player)
    {

        int theOrder = 0;
        
        if (Input.GetKeyDown(KeyCode.Alpha1) && textIsFinished)
        {
           
            
            ManageText("Order is One");
            StartCoroutine(WaitForKeyDown());
            theOrder = 1;                       
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && textIsFinished)
        {
            
            ManageText("Order is Two");
            StartCoroutine(WaitForKeyDown());
            theOrder = 2;
        }
        if(player == 1)
        {
            playerOneChosenOrder = theOrder;
        }
    }
        
    //Get Target Method
    private void HandleOrder(int player)
    {
        if (player == 1)
        {
            if (playerOneChosenOrder != order && textIsFinished && pressedSpace && playerOneOption != CombatOptions.HasNotChosen && playerOneChosenOrder != 0)
            {
                order += 1;
            }
            else if (playerOneChosenOrder == order && textIsFinished && pressedSpace && playerOneOption != CombatOptions.HasNotChosen && playerOneChosenOrder != 0)
            {
                if (playerOneOption == CombatOptions.Attack)
                {                   
                    Attack(EnemyDataManager.EnemyManager, DataManager.manager);
                }
                order += 1;
            }
        }
    }

    IEnumerator PlayText()
    {
        textIsFinished = false;
        foreach (char c in combattext)
        {
            combatText.text += c;
            yield return new WaitForSeconds(0.03f);
        }
        textIsFinished = true;
    }
    /*IEnumerator Wait(int seconds)
    {
        textIsFinished = false;
        yield return new WaitForSeconds(seconds);
        textIsFinished = true;
    }*/
    IEnumerator WaitForKeyDown()
    {
        pressedSpace = false;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        pressedSpace = true;
    }
    IEnumerator FadeText()
    {
        Text text = damageText;
        Color originalColor = text.color;
        for (float t = 0.01f; t < textFadeTime; t += Time.deltaTime)
        {
            text.color = Color.Lerp(originalColor, Color.clear, Mathf.Min(1, t / textFadeTime));
            yield return null;
        }
    }


    void Update()
    {
        switch (combatState)
        {
            case CombatStates.PlayerOneAttacking:
                if (playerOneOption == CombatOptions.HasNotChosen)
                {
                    GetPlayerAction(1);
                }
                if (playerOneOption != CombatOptions.HasNotChosen && playerOneChosenOrder == 0)
                {
                    GetOrder(1);
                }

                HandleOrder(1);
                
                if(order == EnemyDataManager.EnemyManager.assignedOrderInCombat)
                {
                    combatState = CombatStates.EnemyAttacking;
                }

                break;
            case CombatStates.EnemyAttacking:
                /*if (textIsFinished && pressedSpace)
                {
                    ManageText("Enemy's Turn");
                }*/

                break;
            case CombatStates.ResetValues:
                playerOneOption = CombatOptions.HasNotChosen;
                playerOneChosenOrder = 0;
                break;
            default:
                break;

        }
    }
}
