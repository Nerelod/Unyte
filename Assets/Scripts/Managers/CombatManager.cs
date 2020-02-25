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
    // Reference to sprite representing who is going in combat
    public GameObject combatCharSprite;
    // Reference to the enemy sprite
    public GameObject enemySprite;
    public GameObject enemySpriteTwo;
    // Reference to the CombatStates enum, used for deciding what part of combat it is
    CombatStates combatState;
    // Reference to the playerOneOption enum, used for assigning the chosen option
    //CombatOptions junakOption;
    // Saralf's Option
    //CombatOptions saralfOption;
    // The current order or turn of the combat
    private int order;
    // order to reset values
    private int orderToReset;
    // The order playerOne chose to act
    private int junakChosenOrder;
    // The Order Saralf will act
    private int saralfChosenOrder;
    // Boolean that represents whether the combatant has attacked/been handled
    private bool enemyOneHasAttacked;
    private bool enemyTwoHasAttacked;
    [SerializeField] private bool junakHandled;
    private bool saralfHandled;
    // Boolean that represents whether the win text was prompt
    private bool winTextHasBeenPrompt;

    private List<DataManager> partymembers = new List<DataManager>();
    private List<DataManager> livingPartyMembers;

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

    private List<DataManager> combatants = new List<DataManager>();


    private int amountDead;
    private int deadEnemies;

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
        amountDead = 0;
        deadEnemies = 0;
        JunakDataManager.Junak.isTurnInCombat = false;
        SaralfDataManager.Saralf.isTurnInCombat = false;
        junakHandled = saralfHandled = false;
        JunakDataManager.Junak.itemManager.isInCombat = true;
        JunakDataManager.Junak.abilityManager.choseAbilityInCombat = false;
        JunakDataManager.Junak.itemManager.itemToUse = null;
        JunakDataManager.Junak.itemManager.choseItemInCombat = false;
        SaralfDataManager.Saralf.itemManager.isInCombat = true;
        SaralfDataManager.Saralf.abilityManager.choseAbilityInCombat = false;
        SaralfDataManager.Saralf.itemManager.itemToUse = null;
        SaralfDataManager.Saralf.itemManager.choseItemInCombat = false;
        // Set the abilityselectpanel off
        CombatMenuManager.combatMenuManager.abilitySelectPanel.SetActive(false);
        // Win text is false, has not been displayed
        winTextHasBeenPrompt = false;
        // Make the damage text empty
        CombatTextManager.combatTextManager.damageText.text = "";
        // Begin the order at 1
        order = 1;
        // No text has been prompt
        CombatTextManager.combatTextManager.textHasBeenPrompt = false;
        // Space has not been pressed
        CombatTextManager.combatTextManager.pressedSpace = false;
        // PlayerOne has not chosen an order to act
        junakChosenOrder = 0;
        saralfChosenOrder = 0;
        // PlayerOne has not chosen an action
        JunakDataManager.Junak.combatOption = CombatOptions.HasNotChosen;
        SaralfDataManager.Saralf.combatOption = CombatOptions.HasNotChosen;
        // Display the text that is shown at the beginning of an encounter and wait for key press to continue
        CombatTextManager.combatTextManager.ManageText("A " + EnemyDataManager.EnemyManager.currentType + " appeared!");
        CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
        // Assign the enemySprite
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
        // The enemy has not attacked
        enemyOneHasAttacked = false;
        enemyTwoHasAttacked = false;
        // Get the order that combtatants will act on
        getCombatMembers();
        determineOrder();
        getPartyMembers();
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
    // Method for the Run option
    private void Run(DataManager player)
    {
        // run away animation
        if (player.speed > EnemyDataManager.EnemyManager.speed)
        {
            JunakDataManager.Junak.isBeingLoaded = true;
            JunakDataManager.Junak.ranFromCombat = true;
            SceneManager.LoadScene(EnemyDataManager.EnemyManager.theScene);
        }
        else
        {
            CombatTextManager.combatTextManager.ManageText("Failed to run");
            CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
            order += 1;
        }
    }
    private void resetJunakValues()
    {
        JunakDataManager.Junak.combatOption = CombatOptions.HasNotChosen;
        CombatTextManager.combatTextManager.textHasBeenPrompt = false;
        junakChosenOrder = 0;
        junakHandled = false;
        saralfHandled = false;
        JunakDataManager.Junak.abilityManager.abilityToUse = null;
        JunakDataManager.Junak.abilityManager.choseAbilityInCombat = false;
        JunakDataManager.Junak.itemManager.itemToUse = null;
        JunakDataManager.Junak.itemManager.choseItemInCombat = false;
    }
    private void resetSaralfOptions()
    {
        SaralfDataManager.Saralf.combatOption = CombatOptions.HasNotChosen;
        CombatTextManager.combatTextManager.textHasBeenPrompt = false;
        saralfChosenOrder = 0;
        saralfHandled = false;
        junakHandled = false;
        SaralfDataManager.Saralf.abilityManager.choseAbilityInCombat = false;
        SaralfDataManager.Saralf.itemManager.itemToUse = null;
        SaralfDataManager.Saralf.itemManager.choseItemInCombat = false;
    }
    // Returns amount of combat Members
    private int getCombatantsAmount()
    {
        int members = 1;
        if (SaralfDataManager.Saralf.isInParty) { members += 1; }
        members += EnemyDataManager.EnemyManager.amountOfEnemies;
        return members;
    }
    // Adds all combatants to combatants
    private void getCombatMembers()
    {
        combatants.Add(JunakDataManager.Junak);
        combatants.Add(EnemyDataManager.EnemyManager);
        if (SaralfDataManager.Saralf.isInParty) { combatants.Add(SaralfDataManager.Saralf); }
        if (EnemyDataManager.EnemyManager.amountOfEnemies >= 2) { combatants.Add(EnemyDataManagerTwo.EnemyManagerTwo); }
    }
    // Adds party members to partymembers
    private void getPartyMembers()
    {
        partymembers.Add(JunakDataManager.Junak);
        if (SaralfDataManager.Saralf.isInParty)
        {
            partymembers.Add(SaralfDataManager.Saralf);
        }
        livingPartyMembers = partymembers;
    }
    // Returns amount of every living combatant
    private int getLivingCombatantsAmount() {
        int amountAlive = 0;
        foreach(DataManager combatant in combatants) {
            if(combatant.health > 0) {
                amountAlive += 1;
            }
        }
        return amountAlive;
    }
    private void dealWithDead(DataManager dead)
    {
        if (dead.health <= 0)
        {
            if (livingPartyMembers.Contains(dead)) { livingPartyMembers.Remove(dead); }
            Debug.Log("Dealing with dead enemy");
            Debug.Log("Dead Enemy Name: " + dead.theName);
            Debug.Log("Dead assigned order in combat: " + dead.assignedOrderInCombat);
            //order += 1;
            orderToReset -= 1;
            Debug.Log("New order to reset: " + orderToReset.ToString());
            dead.combatText.text = "";
            if (dead == EnemyDataManager.EnemyManager) { enemySprite.GetComponent<SpriteRenderer>().sprite = null; }
            else if (dead == EnemyDataManagerTwo.EnemyManagerTwo) { enemySpriteTwo.GetComponent<SpriteRenderer>().sprite = null; }
            dead.combatIconObject.GetComponent<SpriteRenderer>().sprite = null;
            foreach (DataManager combatant in combatants)
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
            icons[getCombatantsAmount() - 1].GetComponent<SpriteRenderer>().sprite = null;
            iconTexts[getCombatantsAmount() - 1].text = "";
            dead.assignedOrderInCombat = 0;
        }
    }
    // Order is determined by speed
    private void determineOrder()
    {
        int[] speeds = new int[getCombatantsAmount()];
        int i = 0;
        foreach (DataManager combatant in combatants)
        {
            speeds[i] = combatant.speed;
            i++;
        }

        Array.Sort(speeds);
        Array.Reverse(speeds);
        JunakDataManager.Junak.assignedOrderInCombat = Array.IndexOf(speeds, JunakDataManager.Junak.speed) + 1;
        if (SaralfDataManager.Saralf.isInParty) { SaralfDataManager.Saralf.assignedOrderInCombat = Array.IndexOf(speeds, SaralfDataManager.Saralf.speed) + 1; }
        EnemyDataManager.EnemyManager.assignedOrderInCombat = Array.IndexOf(speeds, EnemyDataManager.EnemyManager.speed) + 1;
        if (EnemyDataManager.EnemyManager.amountOfEnemies >= 2) { EnemyDataManagerTwo.EnemyManagerTwo.assignedOrderInCombat = Array.IndexOf(speeds, EnemyDataManagerTwo.EnemyManagerTwo.speed) + 1; }

        foreach (DataManager combatant in combatants)
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
        orderToReset = getCombatantsAmount() + 1;
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
                Run(player);
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
        if (character == JunakDataManager.Junak) { junakChosenOrder = 0; }
        else if (character == SaralfDataManager.Saralf) { saralfChosenOrder = 0; }
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
        else if (Input.GetKeyDown(KeyCode.Alpha3) && CombatTextManager.combatTextManager.textIsFinished && getCombatantsAmount() >= 3)
        {
            CombatTextManager.combatTextManager.ManageText("Order is Three");
            CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
            theOrder = 3;
        }
        if (player == JunakDataManager.Junak)
        {
            junakChosenOrder = theOrder;
        }
        else if (player == SaralfDataManager.Saralf)
        {
            saralfChosenOrder = theOrder;
        }
    }

    // TODO: Get Target Method

    // Handles all actions that happen on the order
    private void HandleOrder(DataManager player)
    {
        if (player == JunakDataManager.Junak && isTextManagerDone())
        {
            if (junakChosenOrder == order && player.combatOption != CombatOptions.HasNotChosen && junakChosenOrder != 0 && player.health > 0)
            {
                if (player.combatOption == CombatOptions.Attack)
                {
                    Attack(player.enemyToTarget, JunakDataManager.Junak);
                }
                else if (player.combatOption == CombatOptions.Ability)
                {
                    if (SaralfDataManager.Saralf.combatOption == CombatOptions.Ability && saralfChosenOrder == junakChosenOrder && SaralfDataManager.Saralf.isInParty)
                    {
                        if (JunakDataManager.Junak.abilityManager.checkCombo(JunakDataManager.Junak, SaralfDataManager.Saralf))
                        {
                            JunakDataManager.Junak.abilityManager.executeCombo(JunakDataManager.Junak, SaralfDataManager.Saralf);
                            saralfHandled = true;
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
                    dealWithDead(player.enemyToTarget);
                    CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
                }

            }
            junakHandled = true;
        }
        else if (player == SaralfDataManager.Saralf && isTextManagerDone())
        {
            if (saralfChosenOrder == order && isTextManagerDone() && player.combatOption != CombatOptions.HasNotChosen && saralfChosenOrder != 0)
            {
                if (player.combatOption == CombatOptions.Attack)
                {
                    Attack(player.enemyToTarget, SaralfDataManager.Saralf);
                }
                else if (player.combatOption == CombatOptions.Ability)
                {
                    if (JunakDataManager.Junak.combatOption == CombatOptions.Ability && junakChosenOrder == saralfChosenOrder)
                    {
                        if (SaralfDataManager.Saralf.abilityManager.checkCombo(SaralfDataManager.Saralf, JunakDataManager.Junak))
                        {
                            SaralfDataManager.Saralf.abilityManager.executeCombo(SaralfDataManager.Saralf, JunakDataManager.Junak);
                            junakHandled = true;
                        }
                        else { SaralfDataManager.Saralf.abilityManager.useAbility(); }
                    }
                    else { SaralfDataManager.Saralf.abilityManager.useAbility(); }
                }
                else if (player.combatOption == CombatOptions.Item)
                {
                    SaralfDataManager.Saralf.itemManager.useItem(SaralfDataManager.Saralf);
                    CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
                }

            }
            saralfHandled = true;

        }
        if (SaralfDataManager.Saralf.isInParty)
        {
            if (saralfHandled && junakHandled) { order += 1; }
        }
        else
        {
            if (junakHandled) { order += 1; }
        }
    }
    private void checkWinner()
    {
        amountDead = 0;
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
                amountDead += 1;
            }
        }
        deadEnemies = 0;
        for (int i = 0; i < EnemyDataManager.EnemyManager.amountOfEnemies; i++)
        {
            if (enemies[i].health <= 0)
            {
                deadEnemies += 1;
            }
        }

        if (amountDead == (getCombatantsAmount() - EnemyDataManager.EnemyManager.amountOfEnemies))
        {
            Debug.Log(amountDead);
            combatState = CombatStates.EnemyWon;
        }
        if (deadEnemies == EnemyDataManager.EnemyManager.amountOfEnemies)
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
                if (JunakDataManager.Junak.combatOption != CombatOptions.HasNotChosen && junakChosenOrder == 0)
                {
                    GetOrder(JunakDataManager.Junak);
                }
                if (JunakDataManager.Junak.combatOption != CombatOptions.HasNotChosen && junakChosenOrder != 0 && !junakHandled && isTextManagerDone())
                {
                    HandleOrder(JunakDataManager.Junak);
                }
                if (!saralfHandled && isTextManagerDone() && junakHandled && SaralfDataManager.Saralf.isInParty)
                {
                    HandleOrder(SaralfDataManager.Saralf);
                }

                if (order == EnemyDataManager.EnemyManager.assignedOrderInCombat)
                {
                    junakHandled = false;
                    saralfHandled = false;
                    JunakDataManager.Junak.isTurnInCombat = false;
                    combatState = CombatStates.EnemyAttacking;
                }
                else if (order == EnemyDataManagerTwo.EnemyManagerTwo.assignedOrderInCombat)
                {
                    junakHandled = false;
                    saralfHandled = false;
                    JunakDataManager.Junak.isTurnInCombat = false;
                    combatState = CombatStates.EnemyTwoAttacking;
                }
                else if (order == SaralfDataManager.Saralf.assignedOrderInCombat)
                {
                    resetSaralfOptions();
                    JunakDataManager.Junak.isTurnInCombat = false;
                    combatState = CombatStates.SaralfAttacking;
                }
                else if (order == orderToReset)
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
                if (SaralfDataManager.Saralf.combatOption != CombatOptions.HasNotChosen && saralfChosenOrder == 0)
                {
                    GetOrder(SaralfDataManager.Saralf);
                }
                if (SaralfDataManager.Saralf.combatOption != CombatOptions.HasNotChosen && saralfChosenOrder != 0 && !saralfHandled && isTextManagerDone())
                {
                    HandleOrder(SaralfDataManager.Saralf);
                }
                if (!junakHandled && isTextManagerDone() && saralfHandled)
                {
                    HandleOrder(JunakDataManager.Junak);
                }
                if (order == EnemyDataManager.EnemyManager.assignedOrderInCombat)
                {
                    junakHandled = false;
                    saralfHandled = false;
                    SaralfDataManager.Saralf.isTurnInCombat = false;
                    combatState = CombatStates.EnemyAttacking;
                }

                if (order == EnemyDataManagerTwo.EnemyManagerTwo.assignedOrderInCombat)
                {
                    junakHandled = false;
                    saralfHandled = false;
                    SaralfDataManager.Saralf.isTurnInCombat = false;
                    combatState = CombatStates.EnemyTwoAttacking;
                }
                else if (order == orderToReset)
                {
                    SaralfDataManager.Saralf.isTurnInCombat = false;
                    combatState = CombatStates.ResetValues;
                }
                else if (order == JunakDataManager.Junak.assignedOrderInCombat)
                {
                    SaralfDataManager.Saralf.isTurnInCombat = false;
                    combatState = CombatStates.JunakAttacking;
                }
                break;
            case CombatStates.EnemyAttacking: // On enemy's turn
                checkWinner();
                if (CombatTextManager.combatTextManager.textIsFinished && CombatTextManager.combatTextManager.pressedSpace && !enemyOneHasAttacked)
                {
                    //TODO: Choose LIVING target
                    whichtarget = UnityEngine.Random.Range(0, getLivingCombatantsAmount() - EnemyDataManager.EnemyManager.amountOfEnemies);
                    EnemyDataManager.EnemyManager.theMonster.Attack(livingPartyMembers[whichtarget], EnemyDataManager.EnemyManager);
                    dealWithDead(partymembers[whichtarget]);
                    enemyOneHasAttacked = true;
                }
                if (isTextManagerDone() && !EnemyDataManager.EnemyManager.theMonster.displayedDamage)
                {
                    EnemyDataManager.EnemyManager.theMonster.DisplayDamage(partymembers[whichtarget]);
                }


                if (enemyOneHasAttacked)
                {
                    if (!junakHandled) { HandleOrder(JunakDataManager.Junak); }
                    if (!saralfHandled && SaralfDataManager.Saralf.isInParty) { HandleOrder(SaralfDataManager.Saralf); }
                }


                if (order == JunakDataManager.Junak.assignedOrderInCombat && CombatTextManager.combatTextManager.textIsFinished)
                {
                    resetJunakValues();
                    combatState = CombatStates.JunakAttacking;
                }
                else if (order == SaralfDataManager.Saralf.assignedOrderInCombat && CombatTextManager.combatTextManager.textIsFinished)
                {
                    resetSaralfOptions();
                    combatState = CombatStates.SaralfAttacking;
                }
                else if (order == EnemyDataManagerTwo.EnemyManagerTwo.assignedOrderInCombat && CombatTextManager.combatTextManager.textIsFinished)
                {
                    junakHandled = false;
                    saralfHandled = false;
                    combatState = CombatStates.EnemyTwoAttacking;
                }
                else if (order == orderToReset)
                {
                    combatState = CombatStates.ResetValues;
                }

                break;
            case CombatStates.EnemyTwoAttacking: // On enemy two's turn
                checkWinner();
                if (CombatTextManager.combatTextManager.textIsFinished && CombatTextManager.combatTextManager.pressedSpace && !enemyTwoHasAttacked)
                {
                    whichtarget = UnityEngine.Random.Range(0, getCombatantsAmount() - EnemyDataManager.EnemyManager.amountOfEnemies);
                    EnemyDataManagerTwo.EnemyManagerTwo.theMonster.Attack(partymembers[whichtarget], EnemyDataManagerTwo.EnemyManagerTwo);
                    dealWithDead(partymembers[whichtarget]);
                    enemyTwoHasAttacked = true;
                }
                if (isTextManagerDone() && !EnemyDataManagerTwo.EnemyManagerTwo.theMonster.displayedDamage)
                {
                    EnemyDataManagerTwo.EnemyManagerTwo.theMonster.DisplayDamage(partymembers[whichtarget]);
                }
                if (enemyTwoHasAttacked)
                {
                    if (!junakHandled) { HandleOrder(JunakDataManager.Junak); }
                    if (!saralfHandled && SaralfDataManager.Saralf.isInParty) { HandleOrder(SaralfDataManager.Saralf); }
                }
                if (order == JunakDataManager.Junak.assignedOrderInCombat && CombatTextManager.combatTextManager.textIsFinished)
                {
                    resetJunakValues();
                    combatState = CombatStates.JunakAttacking;
                }
                else if (order == SaralfDataManager.Saralf.assignedOrderInCombat && CombatTextManager.combatTextManager.textIsFinished)
                {
                    resetSaralfOptions();
                    combatState = CombatStates.SaralfAttacking;
                }
                else if (order == orderToReset)
                {
                    combatState = CombatStates.ResetValues;
                }
                break;
            case CombatStates.ResetValues:
                Debug.Log("RESET");
                order = 1;
                junakHandled = false;
                saralfHandled = false;
                enemyOneHasAttacked = false;
                enemyTwoHasAttacked = false;
                CombatTextManager.combatTextManager.textHasBeenPrompt = false;
                EnemyDataManager.EnemyManager.theMonster.displayedDamage = false;
                EnemyDataManager.EnemyManager.theMonster.textWasPrompt = false;
                if (EnemyDataManager.EnemyManager.amountOfEnemies >= 2)
                {
                    EnemyDataManagerTwo.EnemyManagerTwo.theMonster.displayedDamage = false;
                    EnemyDataManagerTwo.EnemyManagerTwo.theMonster.textWasPrompt = false;
                }
                if (order == JunakDataManager.Junak.assignedOrderInCombat)
                {
                    resetJunakValues();
                    combatState = CombatStates.JunakAttacking;
                }
                else if (order == SaralfDataManager.Saralf.assignedOrderInCombat)
                {
                    resetSaralfOptions();
                    combatState = CombatStates.SaralfAttacking;
                }
                else if (order == EnemyDataManager.EnemyManager.assignedOrderInCombat)
                {
                    combatState = CombatStates.EnemyAttacking;
                }
                else if (order == EnemyDataManagerTwo.EnemyManagerTwo.assignedOrderInCombat)
                {
                    combatState = CombatStates.EnemyTwoAttacking;
                }
                break;
            case CombatStates.PlayerWon:
                if (CombatTextManager.combatTextManager.textIsFinished && CombatTextManager.combatTextManager.pressedSpace && !winTextHasBeenPrompt)
                {
                    CombatTextManager.combatTextManager.ManageText("You Win!");
                    CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
                    winTextHasBeenPrompt = true;
                }
                if (CombatTextManager.combatTextManager.pressedSpace && winTextHasBeenPrompt && CombatTextManager.combatTextManager.textIsFinished)
                {
                    JunakDataManager.Junak.isBeingLoaded = true;
                    EnemyDataManager.EnemyManager.defeatedEnemies.Add(EnemyDataManager.EnemyManager.currentName);
                    JunakDataManager.Junak.addExperience(EnemyDataManager.EnemyManager.experienceGives);
                    if (SaralfDataManager.Saralf.isInParty) { SaralfDataManager.Saralf.addExperience(EnemyDataManager.EnemyManager.experienceGives); }
                    JunakDataManager.Junak.itemManager.isInCombat = false;
                    SaralfDataManager.Saralf.itemManager.isInCombat = false;
                    SceneManager.LoadScene(EnemyDataManager.EnemyManager.theScene);
                }
                break;
            case CombatStates.EnemyWon:
                if (CombatTextManager.combatTextManager.textIsFinished && CombatTextManager.combatTextManager.pressedSpace && !winTextHasBeenPrompt)
                {
                    CombatTextManager.combatTextManager.ManageText("You Lost!");
                    CombatTextManager.combatTextManager.StartCoroutine(CombatTextManager.combatTextManager.WaitForKeyDown());
                    winTextHasBeenPrompt = true;
                }
                if (CombatTextManager.combatTextManager.pressedSpace && winTextHasBeenPrompt)
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
        if (order != orderToReset) { CombatTextManager.combatTextManager.orderText.text = order.ToString(); }
        //if (EnemyDataManager.EnemyManager.health > 0) { CombatTextManager.combatTextManager.enemyHealthText.text = "?"; }
        //if (EnemyDataManager.EnemyManager.amountOfEnemies >= 2 && EnemyDataManagerTwo.EnemyManagerTwo.health > 0) { CombatTextManager.combatTextManager.enemyHealthTextTwo.text = "?"; }
    }
}
