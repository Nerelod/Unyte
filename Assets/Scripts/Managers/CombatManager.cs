using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// The states of combat, the phases
public enum CombatStates { JunakAttacking, SaralfAttacking, EnemyAttacking, EnemyTwoAttacking, ResetValues, PlayerWon, EnemyWon }
// The actions a player can take on their turn/order of combat
public enum CombatOptions { Attack, Ability, Item, Run, HasNotChosen }

public class CombatManager : MonoBehaviour
{
    CombatManagerTools Tools = new CombatManagerTools();
    // Reference to sprite representing who is going in combat
    public GameObject combatCharSprite;
    // Reference to the enemy sprite
    public GameObject enemySprite;
    public GameObject enemySpriteTwo;
    // Reference to the CombatStates enum, used for deciding what part of combat it is
    CombatStates combatState;

    private int whichtarget;

    public Text textIconOne;
    public Text textIconTwo;
    public Text textIconThree;
    public Text textIconFour;

    private List<Text> iconTexts = new List<Text>();

    // IconOne image
    public GameObject iconOne;
    // IconTwo image
    public GameObject iconTwo;
    //Icon Three image
    public GameObject iconThree;
    //Icon Four image
    public GameObject iconFour;

    private List<GameObject> icons = new List<GameObject>();

    // Runs before Awake
    private void Awake()
    {
        // Get a reference to the CombatTextManager, so it exists
        CombatTextManager.combatTextManager = GameObject.Find("CombatTextManager").GetComponent<CombatTextManager>();
        // Get a reference to the combatMenuManager
        CombatMenuManager.combatMenuManager = GameObject.Find("CombatMenuManager").GetComponent<CombatMenuManager>();
    }

    // Used to set all the values to default at the start of combat
    void Start()
    {
        Tools.initiateTools();
        enemySprite = GameObject.Find("Enemy");
        enemySprite.GetComponent<SpriteRenderer>().sprite = EnemyDataManager.EnemyManager.combatSprite;
        if (EnemyDataManager.EnemyManager.amountOfEnemies >= 2)
        {
            enemySpriteTwo = GameObject.Find("EnemyTwo");
            enemySprite.transform.position = new Vector3(-1.7f, 0, 0);
            enemySpriteTwo.transform.position = new Vector3(1.7f, 0, 0);
            enemySpriteTwo.GetComponent<SpriteRenderer>().sprite = EnemyDataManagerTwo.EnemyManagerTwo.combatSprite;
        }

        // Assign the icons
        iconOne = GameObject.Find("IconOne");
        iconTwo = GameObject.Find("IconTwo");
        iconThree = GameObject.Find("IconThree");
        textIconOne.text = "";
        textIconTwo.text = "";
        textIconThree.text = "";
        textIconFour.text = "";
        iconTexts.Add(textIconOne);
        iconTexts.Add(textIconTwo);
        iconTexts.Add(textIconThree);
        iconTexts.Add(textIconFour);
        icons.Add(iconOne);
        icons.Add(iconTwo);
        icons.Add(iconThree);
        icons.Add(iconFour);
        // Get the order that combtatants will act on
        determineOrder();
    }
    // Method for checking if CombatTextManager finished what it does
    private bool isTextManagerDone()
    {
        if (CombatTextManager.combatTextManager.textIsFinished && CombatTextManager.combatTextManager.pressedSpace)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void resetJunakValues()
    {
        JunakDataManager.Junak.combatOption = CombatOptions.HasNotChosen;
        CombatTextManager.combatTextManager.textHasBeenPrompt = false;
        Tools.junakChosenOrder = 0;
        JunakDataManager.Junak.handled = false;
        SaralfDataManager.Saralf.handled = false;
        JunakDataManager.Junak.abilityManager.abilityToUse = null;
        JunakDataManager.Junak.abilityManager.choseAbilityInCombat = false;
        JunakDataManager.Junak.itemManager.itemToUse = null;
        JunakDataManager.Junak.itemManager.choseItemInCombat = false;
    }
    private void resetSaralfOptions()
    {
        SaralfDataManager.Saralf.combatOption = CombatOptions.HasNotChosen;
        CombatTextManager.combatTextManager.textHasBeenPrompt = false;
        Tools.saralfChosenOrder = 0;
        JunakDataManager.Junak.handled = false;
        SaralfDataManager.Saralf.handled = false;
        SaralfDataManager.Saralf.abilityManager.choseAbilityInCombat = false;
        SaralfDataManager.Saralf.itemManager.itemToUse = null;
        SaralfDataManager.Saralf.itemManager.choseItemInCombat = false;
    }

    private void dealWithDead(DataManager dead)
    {
        if (dead.health <= 0)
        {
            Tools.oldDeadCombatantOrder = dead.assignedOrderInCombat;
            Tools.thereWasDeath = true;
            if (Tools.livingPartyMembers.Contains(dead)) { Tools.livingPartyMembers.Remove(dead); }
            Debug.Log("Dealing with dead enemy");
            Debug.Log("Dead Enemy Name: " + dead.theName);
            Debug.Log("Dead assigned order in combat: " + dead.assignedOrderInCombat);
            Tools.orderToReset -= 1;
            Debug.Log("New order to reset: " + Tools.orderToReset.ToString());
            dead.combatText.text = "";
            if (dead == EnemyDataManager.EnemyManager) { enemySprite.GetComponent<SpriteRenderer>().sprite = null; }
            else if (dead == EnemyDataManagerTwo.EnemyManagerTwo) { enemySpriteTwo.GetComponent<SpriteRenderer>().sprite = null; }
            dead.combatIconObject.GetComponent<SpriteRenderer>().sprite = null;
            foreach (DataManager combatant in Tools.combatants)
            {
                if (combatant != dead && combatant.assignedOrderInCombat > dead.assignedOrderInCombat)
                {
                    Debug.Log(combatant.theName);
                    Debug.Log("Old assigned order: " + combatant.assignedOrderInCombat);
                    combatant.assignedOrderInCombat -= 1;
                    Debug.Log("New assigned order: " + combatant.assignedOrderInCombat);
                    combatant.combatIconObject = icons[combatant.assignedOrderInCombat - 1];
                    Debug.Log("New Icon: " + combatant.assignedOrderInCombat);
                    combatant.combatText = iconTexts[combatant.assignedOrderInCombat - 1];
                    combatant.combatIconObject.GetComponent<SpriteRenderer>().sprite = combatant.combatIcon;

                    CombatTextManager.combatTextManager.junakHealthText = JunakDataManager.Junak.combatText;
                    CombatTextManager.combatTextManager.enemyHealthText = EnemyDataManager.EnemyManager.combatText;
                    if (SaralfDataManager.Saralf.isInParty) { CombatTextManager.combatTextManager.saralfHealthText = SaralfDataManager.Saralf.combatText; }
                    if (EnemyDataManager.EnemyManager.amountOfEnemies >= 2) { CombatTextManager.combatTextManager.enemyHealthTextTwo = EnemyDataManagerTwo.EnemyManagerTwo.combatText; }
                    CombatTextManager.combatTextManager.enemyHealthText.text = "?";
                    if (EnemyDataManager.EnemyManager.amountOfEnemies >= 2) { CombatTextManager.combatTextManager.enemyHealthTextTwo.text = "?"; }
                }
            }
            icons[Tools.getCombatantsAmount() - 1].GetComponent<SpriteRenderer>().sprite = null;
            iconTexts[Tools.getCombatantsAmount() - 1].text = "";
            dead.assignedOrderInCombat = 0;
        }
    }
    // Order is determined by speed
    private void determineOrder()
    {
        int[] speeds = new int[Tools.getCombatantsAmount()];
        int i = 0;
        foreach (DataManager combatant in Tools.combatants)
        {
            speeds[i] = combatant.speed;
            i++;
            Debug.Log(combatant.theName + combatant.speed);
        }

        Array.Sort(speeds);
        Array.Reverse(speeds);
        JunakDataManager.Junak.assignedOrderInCombat = Array.IndexOf(speeds, JunakDataManager.Junak.speed) + 1;
        if (SaralfDataManager.Saralf.isInParty) { SaralfDataManager.Saralf.assignedOrderInCombat = Array.IndexOf(speeds, SaralfDataManager.Saralf.speed) + 1; }
        EnemyDataManager.EnemyManager.assignedOrderInCombat = Array.IndexOf(speeds, EnemyDataManager.EnemyManager.speed) + 1;
        if (EnemyDataManager.EnemyManager.amountOfEnemies >= 2) { EnemyDataManagerTwo.EnemyManagerTwo.assignedOrderInCombat = Array.IndexOf(speeds, EnemyDataManagerTwo.EnemyManagerTwo.speed) + 1; }

        foreach (DataManager combatant in Tools.combatants)
        {
            if (combatant.assignedOrderInCombat == 1)
            {
                combatant.combatIconObject = iconOne;
                combatant.combatText = textIconOne;
            }
            else if (combatant.assignedOrderInCombat == 2)
            {
                combatant.combatIconObject = iconTwo;
                combatant.combatText = textIconTwo;
            }
            else if (combatant.assignedOrderInCombat == 3)
            {
                combatant.combatIconObject = iconThree;
                combatant.combatText = textIconThree;
            }
            else if (combatant.assignedOrderInCombat == 4)
            {
                combatant.combatIconObject = iconFour;
                combatant.combatText = textIconFour;
            }
            combatant.combatIconObject.GetComponent<SpriteRenderer>().sprite = combatant.combatIcon;
        }

        CombatTextManager.combatTextManager.junakHealthText = JunakDataManager.Junak.combatText;
        CombatTextManager.combatTextManager.enemyHealthText = EnemyDataManager.EnemyManager.combatText;
        if (SaralfDataManager.Saralf.isInParty) { CombatTextManager.combatTextManager.saralfHealthText = SaralfDataManager.Saralf.combatText; }
        if (EnemyDataManager.EnemyManager.amountOfEnemies >= 2) { CombatTextManager.combatTextManager.enemyHealthTextTwo = EnemyDataManagerTwo.EnemyManagerTwo.combatText; }

        CombatTextManager.combatTextManager.enemyHealthText.text = "?";
        if (EnemyDataManager.EnemyManager.amountOfEnemies >= 2) { CombatTextManager.combatTextManager.enemyHealthTextTwo.text = "?"; }
        Tools.orderToReset = Tools.getCombatantsAmount() + 1;
    }

    // Gets the player's chosen action 
    private void GetPlayerAction(DataManager player)
    {
        // If the previous text is finished, the text has not been prompt, and the user pressed space, display "Choose an Action" 
        if (CombatTextManager.combatTextManager.textIsFinished && !CombatTextManager.combatTextManager.textHasBeenPrompt && CombatTextManager.combatTextManager.pressedSpace)
        {
            CombatTextManager.combatTextManager.ManageText("Choose an Action");
            CombatTextManager.combatTextManager.textHasBeenPrompt = true;
        }
        // Assign playerOneOption if it is HasNotChosen based on input, followed by displaying "Choose Order To Act"
        if (CombatTextManager.combatTextManager.textHasBeenPrompt && CombatTextManager.combatTextManager.textIsFinished)
        {
            if (Input.GetKeyDown(KeyCode.Q) && player.combatOption == CombatOptions.HasNotChosen)
            {
                player.combatOption = CombatOptions.Attack;
                CombatMenuManager.combatMenuManager.enemySelectPanelWhenTurnedOn();
                CombatTextManager.combatTextManager.ManageText("Choose Order To Act");
            }
            if (Input.GetKeyDown(KeyCode.W) && player.combatOption == CombatOptions.HasNotChosen)
            {
                if (player == JunakDataManager.Junak) { CombatMenuManager.combatMenuManager.junakAbilityPanelWhenTurnedOn(); }
                else if (player == SaralfDataManager.Saralf) { CombatMenuManager.combatMenuManager.saralfAbilityPanelWhenTurnedOn(); }
                CombatTextManager.combatTextManager.ManageText("Choose Order To Act");
            }
            if (Input.GetKeyDown(KeyCode.E) && player.combatOption == CombatOptions.HasNotChosen)
            {
                CombatMenuManager.combatMenuManager.itemPanelWhenTurnedOn();
                CombatTextManager.combatTextManager.ManageText("Choose Order To Act");
            }
            if (Input.GetKeyDown(KeyCode.R) && player.combatOption == CombatOptions.HasNotChosen)
            {
                Tools.Run(player);
            }

            if (player.abilityManager.choseAbilityInCombat == true)
            {
                player.combatOption = CombatOptions.Ability;
            }
            if (player.itemManager.choseItemInCombat == true)
            {
                player.combatOption = CombatOptions.Item;
            }
        }
    }

    private void Attack(EnemyDataManager enemy, DataManager character)
    {
        CombatTextManager.combatTextManager.ManageText(character.theName + " does " + character.qDamage.ToString() + " damage!");
        //TODO: play enemy sprite damaged animation
        CombatTextManager.combatTextManager.damageText.text = "-" + character.qDamage.ToString();
        CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.FadeText(CombatTextManager.combatTextManager.damageText));
        CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
        character.enemyToTarget = null;
        if (enemy.health > 0) { enemy.health -= character.qDamage; }
        if (character == JunakDataManager.Junak) { Tools.junakChosenOrder = 0; }
        else if (character == SaralfDataManager.Saralf) { Tools.saralfChosenOrder = 0; }
        dealWithDead(enemy);
    }

    // Method for getting the player's chosen order to act
    private void GetOrder(DataManager player)
    {
        int theOrder = 0;

        if (Input.GetKeyDown(KeyCode.Alpha1) && CombatTextManager.combatTextManager.textIsFinished)
        {
            CombatTextManager.combatTextManager.ManageText("Order is One");
            CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
            theOrder = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && CombatTextManager.combatTextManager.textIsFinished)
        {
            CombatTextManager.combatTextManager.ManageText("Order is Two");
            CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
            theOrder = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && CombatTextManager.combatTextManager.textIsFinished && Tools.getCombatantsAmount() >= 3)
        {
            CombatTextManager.combatTextManager.ManageText("Order is Three");
            CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
            theOrder = 3;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && CombatTextManager.combatTextManager.textIsFinished && Tools.getCombatantsAmount() >= 4)
        {
            CombatTextManager.combatTextManager.ManageText("Order is Four");
            CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
            theOrder = 4;
        }
        if (player == JunakDataManager.Junak)
        {
            Tools.junakChosenOrder = theOrder;
        }
        else if (player == SaralfDataManager.Saralf)
        {
            Tools.saralfChosenOrder = theOrder;
        }
    }

    // TODO: Get Target Method

    // Handles all actions that happen on the order
    private void HandleOrder(DataManager player)
    {
        if (player == JunakDataManager.Junak && isTextManagerDone())
        {
            if (Tools.junakChosenOrder == Tools.order && player.combatOption != CombatOptions.HasNotChosen && Tools.junakChosenOrder != 0 && player.health > 0)
            {
                if (player.combatOption == CombatOptions.Attack)
                {
                    Attack(player.enemyToTarget, JunakDataManager.Junak);
                }
                else if (player.combatOption == CombatOptions.Ability)
                {
                    if (SaralfDataManager.Saralf.combatOption == CombatOptions.Ability && Tools.saralfChosenOrder == Tools.junakChosenOrder && SaralfDataManager.Saralf.isInParty)
                    {
                        if (JunakDataManager.Junak.abilityManager.checkCombo(JunakDataManager.Junak, SaralfDataManager.Saralf))
                        {
                            JunakDataManager.Junak.abilityManager.executeCombo(JunakDataManager.Junak, SaralfDataManager.Saralf);
                            SaralfDataManager.Saralf.handled = true;
                        }
                        else { JunakDataManager.Junak.abilityManager.useAbility(); }
                    }
                    else
                    {
                        JunakDataManager.Junak.abilityManager.useAbility();
                    }
                }
                else if (player.combatOption == CombatOptions.Item)
                {
                    JunakDataManager.Junak.itemManager.useItem(JunakDataManager.Junak);
                    if (player.enemyToTarget != null) { dealWithDead(player.enemyToTarget); }
                    CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
                }

            }
            Debug.Log("Handeled J");
            JunakDataManager.Junak.handled = true;
        }
        else if (player == SaralfDataManager.Saralf && isTextManagerDone())
        {
            if (Tools.saralfChosenOrder == Tools.order && isTextManagerDone() && player.combatOption != CombatOptions.HasNotChosen && Tools.saralfChosenOrder != 0)
            {
                if (player.combatOption == CombatOptions.Attack)
                {
                    Attack(player.enemyToTarget, SaralfDataManager.Saralf);
                }
                else if (player.combatOption == CombatOptions.Ability)
                {
                    if (JunakDataManager.Junak.combatOption == CombatOptions.Ability && Tools.junakChosenOrder == Tools.saralfChosenOrder)
                    {
                        if (SaralfDataManager.Saralf.abilityManager.checkCombo(SaralfDataManager.Saralf, JunakDataManager.Junak))
                        {
                            SaralfDataManager.Saralf.abilityManager.executeCombo(SaralfDataManager.Saralf, JunakDataManager.Junak);
                            JunakDataManager.Junak.handled = true;
                        }
                        else { SaralfDataManager.Saralf.abilityManager.useAbility(); }
                    }
                    else { SaralfDataManager.Saralf.abilityManager.useAbility(); }
                }
                else if (player.combatOption == CombatOptions.Item)
                {
                    SaralfDataManager.Saralf.itemManager.useItem(SaralfDataManager.Saralf);
                    if (player.enemyToTarget != null) { dealWithDead(player.enemyToTarget); }
                    CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
                }

            }
            Debug.Log("Handled S");
            SaralfDataManager.Saralf.handled = true;

        }
        if (Tools.allPartyMembersHandled())
        {
            Debug.Log("adding one to order");
            if (!Tools.thereWasDeath)
            {
                Tools.order += 1;
            }
            else
            {
                Tools.thereWasDeath = false;
                if (Tools.oldDeadCombatantOrder > Tools.order)
                {
                    Tools.order += 1;
                }
            }
        }

    }
    private void checkWinner()
    {
        Tools.amountDead = 0;
        List<DataManager> partyFolk = new List<DataManager>();
        partyFolk.Add(JunakDataManager.Junak);
        if (SaralfDataManager.Saralf.isInParty) { partyFolk.Add(SaralfDataManager.Saralf); }

        List<EnemyDataManager> enemies = new List<EnemyDataManager>();
        enemies.Add(EnemyDataManager.EnemyManager);
        if (EnemyDataManager.EnemyManager.amountOfEnemies >= 2) { enemies.Add(EnemyDataManagerTwo.EnemyManagerTwo); }

        foreach (DataManager member in partyFolk)
        {
            if (member.health <= 0)
            {
                Tools.amountDead += 1;
            }
        }
        Tools.deadEnemies = 0;
        for (int i = 0; i < EnemyDataManager.EnemyManager.amountOfEnemies; i++)
        {
            if (enemies[i].health <= 0)
            {
                Tools.deadEnemies += 1;
            }
        }

        if (Tools.amountDead == (Tools.getCombatantsAmount() - EnemyDataManager.EnemyManager.amountOfEnemies))
        {
            Debug.Log(Tools.amountDead);
            combatState = CombatStates.EnemyWon;
        }
        if (Tools.deadEnemies == EnemyDataManager.EnemyManager.amountOfEnemies)
        {
            combatState = CombatStates.PlayerWon;
        }
    }

    void Update()
    {
        switch (combatState)
        {
            case CombatStates.JunakAttacking: // On Junak's turn
                combatCharSprite.GetComponent<SpriteRenderer>().sprite = JunakDataManager.Junak.combatSprite;
                JunakDataManager.Junak.isTurnInCombat = true;
                checkWinner();
                if (JunakDataManager.Junak.combatOption == CombatOptions.HasNotChosen)
                {
                    GetPlayerAction(JunakDataManager.Junak);
                }
                if (JunakDataManager.Junak.combatOption != CombatOptions.HasNotChosen && Tools.junakChosenOrder == 0)
                {
                    GetOrder(JunakDataManager.Junak);
                }
                if (JunakDataManager.Junak.combatOption != CombatOptions.HasNotChosen && Tools.junakChosenOrder != 0 && !JunakDataManager.Junak.handled && isTextManagerDone())
                {
                    HandleOrder(JunakDataManager.Junak);
                }
                if (!SaralfDataManager.Saralf.handled && isTextManagerDone() && JunakDataManager.Junak.handled && SaralfDataManager.Saralf.isInParty)
                {
                    HandleOrder(SaralfDataManager.Saralf);
                }

                if (Tools.order == EnemyDataManager.EnemyManager.assignedOrderInCombat)
                {
                    JunakDataManager.Junak.handled = false;
                    SaralfDataManager.Saralf.handled = false;
                    JunakDataManager.Junak.isTurnInCombat = false;
                    combatState = CombatStates.EnemyAttacking;
                }
                else if (Tools.order == EnemyDataManagerTwo.EnemyManagerTwo.assignedOrderInCombat)
                {
                    JunakDataManager.Junak.handled = false;
                    SaralfDataManager.Saralf.handled = false;
                    JunakDataManager.Junak.isTurnInCombat = false;
                    combatState = CombatStates.EnemyTwoAttacking;
                }
                else if (Tools.order == SaralfDataManager.Saralf.assignedOrderInCombat)
                {
                    resetSaralfOptions();
                    JunakDataManager.Junak.isTurnInCombat = false;
                    combatState = CombatStates.SaralfAttacking;
                }
                else if (Tools.order == Tools.orderToReset)
                {
                    JunakDataManager.Junak.isTurnInCombat = false;
                    combatState = CombatStates.ResetValues;
                }


                break;
            case CombatStates.SaralfAttacking: // On Saralf's Turn
                checkWinner();
                combatCharSprite.GetComponent<SpriteRenderer>().sprite = SaralfDataManager.Saralf.combatSprite;
                SaralfDataManager.Saralf.isTurnInCombat = true;
                if (SaralfDataManager.Saralf.combatOption == CombatOptions.HasNotChosen)
                {
                    GetPlayerAction(SaralfDataManager.Saralf);
                }
                if (SaralfDataManager.Saralf.combatOption != CombatOptions.HasNotChosen && Tools.saralfChosenOrder == 0)
                {
                    GetOrder(SaralfDataManager.Saralf);
                }
                if (SaralfDataManager.Saralf.combatOption != CombatOptions.HasNotChosen && Tools.saralfChosenOrder != 0 && !SaralfDataManager.Saralf.handled && isTextManagerDone())
                {
                    HandleOrder(SaralfDataManager.Saralf);
                }
                if (!JunakDataManager.Junak.handled && isTextManagerDone() && SaralfDataManager.Saralf.handled)
                {
                    HandleOrder(JunakDataManager.Junak);
                }
                if (Tools.order == EnemyDataManager.EnemyManager.assignedOrderInCombat)
                {
                    JunakDataManager.Junak.handled = false;
                    SaralfDataManager.Saralf.handled = false;
                    SaralfDataManager.Saralf.isTurnInCombat = false;
                    combatState = CombatStates.EnemyAttacking;
                }

                if (Tools.order == EnemyDataManagerTwo.EnemyManagerTwo.assignedOrderInCombat)
                {
                    JunakDataManager.Junak.handled = false;
                    SaralfDataManager.Saralf.handled = false;
                    SaralfDataManager.Saralf.isTurnInCombat = false;
                    combatState = CombatStates.EnemyTwoAttacking;
                }
                else if (Tools.order == Tools.orderToReset)
                {
                    SaralfDataManager.Saralf.isTurnInCombat = false;
                    combatState = CombatStates.ResetValues;
                }
                else if (Tools.order == JunakDataManager.Junak.assignedOrderInCombat)
                {
                    SaralfDataManager.Saralf.isTurnInCombat = false;
                    combatState = CombatStates.JunakAttacking;
                }
                break;
            case CombatStates.EnemyAttacking: // On enemy's turn
                checkWinner();
                if (CombatTextManager.combatTextManager.textIsFinished && CombatTextManager.combatTextManager.pressedSpace && !Tools.enemyOneHasAttacked)
                {
                    //TODO: Choose LIVING target
                    whichtarget = UnityEngine.Random.Range(0, Tools.getLivingCombatantsAmount() - EnemyDataManager.EnemyManager.amountOfEnemies);
                    EnemyDataManager.EnemyManager.theMonster.Attack(Tools.livingPartyMembers[whichtarget], EnemyDataManager.EnemyManager);
                    dealWithDead(Tools.partymembers[whichtarget]);
                    Tools.enemyOneHasAttacked = true;
                }
                if (isTextManagerDone() && !EnemyDataManager.EnemyManager.theMonster.displayedDamage)
                {
                    EnemyDataManager.EnemyManager.theMonster.DisplayDamage(Tools.partymembers[whichtarget]);
                }


                if (Tools.enemyOneHasAttacked)
                {
                    if (!JunakDataManager.Junak.handled) { HandleOrder(JunakDataManager.Junak); }
                    if (!SaralfDataManager.Saralf.handled && SaralfDataManager.Saralf.isInParty) { HandleOrder(SaralfDataManager.Saralf); }
                }


                if (Tools.order == JunakDataManager.Junak.assignedOrderInCombat && CombatTextManager.combatTextManager.textIsFinished)
                {
                    resetJunakValues();
                    combatState = CombatStates.JunakAttacking;
                }
                else if (Tools.order == SaralfDataManager.Saralf.assignedOrderInCombat && CombatTextManager.combatTextManager.textIsFinished)
                {
                    resetSaralfOptions();
                    combatState = CombatStates.SaralfAttacking;
                }
                else if (Tools.order == EnemyDataManagerTwo.EnemyManagerTwo.assignedOrderInCombat && CombatTextManager.combatTextManager.textIsFinished)
                {
                    JunakDataManager.Junak.handled = false;
                    SaralfDataManager.Saralf.handled = false;
                    combatState = CombatStates.EnemyTwoAttacking;
                }
                else if (Tools.order == Tools.orderToReset)
                {
                    combatState = CombatStates.ResetValues;
                }

                break;
            case CombatStates.EnemyTwoAttacking: // On enemy two's turn
                checkWinner();
                if (CombatTextManager.combatTextManager.textIsFinished && CombatTextManager.combatTextManager.pressedSpace && !Tools.enemyTwoHasAttacked)
                {
                    whichtarget = UnityEngine.Random.Range(0, Tools.getCombatantsAmount() - EnemyDataManager.EnemyManager.amountOfEnemies);
                    EnemyDataManagerTwo.EnemyManagerTwo.theMonster.Attack(Tools.partymembers[whichtarget], EnemyDataManagerTwo.EnemyManagerTwo);
                    dealWithDead(Tools.partymembers[whichtarget]);
                    Tools.enemyTwoHasAttacked = true;
                }
                if (isTextManagerDone() && !EnemyDataManagerTwo.EnemyManagerTwo.theMonster.displayedDamage)
                {
                    EnemyDataManagerTwo.EnemyManagerTwo.theMonster.DisplayDamage(Tools.partymembers[whichtarget]);
                }
                if (Tools.enemyTwoHasAttacked)
                {
                    if (!JunakDataManager.Junak.handled) { HandleOrder(JunakDataManager.Junak); }
                    if (!SaralfDataManager.Saralf.handled && SaralfDataManager.Saralf.isInParty) { HandleOrder(SaralfDataManager.Saralf); }
                }
                if (Tools.order == JunakDataManager.Junak.assignedOrderInCombat && CombatTextManager.combatTextManager.textIsFinished)
                {
                    resetJunakValues();
                    combatState = CombatStates.JunakAttacking;
                }
                else if (Tools.order == SaralfDataManager.Saralf.assignedOrderInCombat && CombatTextManager.combatTextManager.textIsFinished)
                {
                    resetSaralfOptions();
                    combatState = CombatStates.SaralfAttacking;
                }
                else if (Tools.order == EnemyDataManager.EnemyManager.assignedOrderInCombat)
                {
                    JunakDataManager.Junak.handled = false;
                    SaralfDataManager.Saralf.handled = false;
                    SaralfDataManager.Saralf.isTurnInCombat = false;
                    combatState = CombatStates.EnemyAttacking;
                }
                else if (Tools.order == Tools.orderToReset)
                {
                    combatState = CombatStates.ResetValues;
                }
                break;
            case CombatStates.ResetValues:
                Debug.Log("RESET");
                Tools.order = 1;
                JunakDataManager.Junak.handled = false;
                SaralfDataManager.Saralf.handled = false;
                Tools.enemyOneHasAttacked = false;
                Tools.enemyTwoHasAttacked = false;
                CombatTextManager.combatTextManager.textHasBeenPrompt = false;
                EnemyDataManager.EnemyManager.theMonster.displayedDamage = false;
                EnemyDataManager.EnemyManager.theMonster.textWasPrompt = false;
                if (EnemyDataManager.EnemyManager.amountOfEnemies >= 2)
                {
                    EnemyDataManagerTwo.EnemyManagerTwo.theMonster.displayedDamage = false;
                    EnemyDataManagerTwo.EnemyManagerTwo.theMonster.textWasPrompt = false;
                }
                if (Tools.order == JunakDataManager.Junak.assignedOrderInCombat)
                {
                    resetJunakValues();
                    combatState = CombatStates.JunakAttacking;
                }
                else if (Tools.order == SaralfDataManager.Saralf.assignedOrderInCombat)
                {
                    resetSaralfOptions();
                    combatState = CombatStates.SaralfAttacking;
                }
                else if (Tools.order == EnemyDataManager.EnemyManager.assignedOrderInCombat)
                {
                    combatState = CombatStates.EnemyAttacking;
                }
                else if (Tools.order == EnemyDataManagerTwo.EnemyManagerTwo.assignedOrderInCombat)
                {
                    combatState = CombatStates.EnemyTwoAttacking;
                }
                break;
            case CombatStates.PlayerWon:
                if (CombatTextManager.combatTextManager.textIsFinished && CombatTextManager.combatTextManager.pressedSpace && !Tools.winTextHasBeenPrompt)
                {
                    CombatTextManager.combatTextManager.ManageText("You Win!");
                    CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
                    Tools.winTextHasBeenPrompt = true;
                }
                if (CombatTextManager.combatTextManager.pressedSpace && Tools.winTextHasBeenPrompt && CombatTextManager.combatTextManager.textIsFinished)
                {
                    JunakDataManager.Junak.isBeingLoaded = true;
                    EnemyDataManager.EnemyManager.defeatedEnemies.Add(EnemyDataManager.EnemyManager.currentID);
                    JunakDataManager.Junak.addExperience(EnemyDataManager.EnemyManager.experienceGives);
                    if (SaralfDataManager.Saralf.isInParty) { SaralfDataManager.Saralf.addExperience(EnemyDataManager.EnemyManager.experienceGives); }
                    JunakDataManager.Junak.itemManager.isInCombat = false;
                    SaralfDataManager.Saralf.itemManager.isInCombat = false;
                    SceneManager.LoadScene(EnemyDataManager.EnemyManager.theScene);
                }
                break;
            case CombatStates.EnemyWon:
                if (CombatTextManager.combatTextManager.textIsFinished && CombatTextManager.combatTextManager.pressedSpace && !Tools.winTextHasBeenPrompt)
                {
                    CombatTextManager.combatTextManager.ManageText("You Lost!");
                    CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
                    Tools.winTextHasBeenPrompt = true;
                }
                if (CombatTextManager.combatTextManager.pressedSpace && Tools.winTextHasBeenPrompt)
                {
                    JunakDataManager.Junak.itemManager.isInCombat = false;
                    SaralfDataManager.Saralf.itemManager.isInCombat = false;
                    SceneManager.LoadScene("MainMenu");
                }
                break;
            default:
                break;

        }
        // Show player's health 
        CombatTextManager.combatTextManager.junakHealthText.text = JunakDataManager.Junak.health.ToString();
        if (SaralfDataManager.Saralf.isInParty)
        {
            CombatTextManager.combatTextManager.saralfHealthText.text = SaralfDataManager.Saralf.health.ToString();
        }
        if (Tools.order != Tools.orderToReset) { CombatTextManager.combatTextManager.orderText.text = Tools.order.ToString(); }
        //if (EnemyDataManager.EnemyManager.health > 0) { CombatTextManager.combatTextManager.enemyHealthText.text = "?"; }
        //if (EnemyDataManager.EnemyManager.amountOfEnemies >= 2 && EnemyDataManagerTwo.EnemyManagerTwo.health > 0) { CombatTextManager.combatTextManager.enemyHealthTextTwo.text = "?"; }
    }
}
