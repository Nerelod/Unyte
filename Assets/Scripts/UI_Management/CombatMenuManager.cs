using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatMenuManager : MonoBehaviour
{
    public static CombatMenuManager combatMenuManager;
    // Ability Panel shenanigans
    public GameObject abilitySelectPanel;
    public Button abilityReturnButton;
    public GameObject saralfAbilitySelectPanel;
    public Button saralfAbilityReturnButton;
    // Item Panel shenanigans
    public GameObject itemSelectPanel;
    public Button itemReturnButton;
    public Button healthPotionButton;
    public Button stoneButton;

    public Text healthPotionText;
    public Text stoneButtonText;
    // Ally Select Panel shenanigans
    public GameObject allySelectPanel;
    public Button junakButton;
    public Button saralfButton;
    // Enemy Select Panel shenanigans
    public GameObject enemySelectPanel;
    public Button enemyOneButton;
    public Text enemyOneButtonText;
    public Button enemyTwoButton;
    public Text enemyTwoButtonText;
    void Start()
    {
        abilitySelectPanel.SetActive(false);
        saralfAbilitySelectPanel.SetActive(false);
        itemSelectPanel.SetActive(false);
        allySelectPanel.SetActive(false);
    }

    private void checkItemInCombat(string itemName, Button button, Text textBox)
    {
        if (JunakDataManager.Junak.itemManager.aquiredItems.Contains(itemName))
        {
            button.gameObject.SetActive(true);
            textBox.text = itemName + " " + JunakDataManager.Junak.itemManager.getAmountOfItem(itemName);
        }
        else
        {
            button.gameObject.SetActive(false);
        }
    }

    public void itemPanelWhenTurnedOn()
    {
        itemSelectPanel.SetActive(true);
        itemReturnButton.Select();
        // activate or deactivate x item button if there are/aren't x items
        checkItemInCombat("Health Potion", healthPotionButton, healthPotionText);
        checkItemInCombat("Stone", stoneButton, stoneButtonText);

    }
    public void junakAbilityPanelWhenTurnedOn()
    {
        abilitySelectPanel.SetActive(true);
        abilityReturnButton.Select();
    }
    public void saralfAbilityPanelWhenTurnedOn()
    {
        saralfAbilitySelectPanel.SetActive(true);
        saralfAbilityReturnButton.Select();
    }
    public void allySelectPanelWhenTurnedOn()
    {
        saralfButton.gameObject.SetActive(SaralfDataManager.Saralf.isInParty);
        allySelectPanel.SetActive(true);
        junakButton.Select();
    }
    public void enemySelectPanelWhenTurnedOn()
    {
        enemySelectPanel.gameObject.SetActive(true);
        if (EnemyDataManager.EnemyManager.health > 0)
        {
            enemyOneButton.gameObject.SetActive(true);
            enemyOneButton.Select();
            enemyOneButtonText.text = EnemyDataManager.EnemyManager.currentName;
        }
        else
        {
            enemyOneButton.gameObject.SetActive(false);
        }
        if (EnemyDataManager.EnemyManager.amountOfEnemies >= 2)
        {
            if (EnemyDataManagerTwo.EnemyManagerTwo.health > 0)
            {
                enemyTwoButton.gameObject.SetActive(true);
                if (EnemyDataManager.EnemyManager.health < 0) { enemyTwoButton.Select(); }
                enemyTwoButtonText.text = EnemyDataManagerTwo.EnemyManagerTwo.currentName;
                if (EnemyDataManager.EnemyManager.health <= 0) { enemyTwoButton.Select(); }
            }
            else
            {
                enemyTwoButton.gameObject.SetActive(false);
            }
        }
    }

    public void selectEnemy(string enemy)
    {
        EnemyDataManager chosenEnemy = null;
        if (enemy == "EnemyOne")
        {
            chosenEnemy = EnemyDataManager.EnemyManager;
        }
        else if (enemy == "EnemyTwo")
        {
            chosenEnemy = EnemyDataManagerTwo.EnemyManagerTwo;
        }
        if (JunakDataManager.Junak.isTurnInCombat) { JunakDataManager.Junak.enemyToTarget = chosenEnemy; }
        else if (SaralfDataManager.Saralf.isTurnInCombat) { SaralfDataManager.Saralf.enemyToTarget = chosenEnemy; }
        CombatMenuManager.combatMenuManager.enemySelectPanel.SetActive(false);
    }

    void Update()
    {

    }
}
