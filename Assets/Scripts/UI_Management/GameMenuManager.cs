using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class GameMenuManager : MonoBehaviour
{

    public static GameMenuManager gameMenuManager;
    public bool canInteractWith;

    public GameObject gameMenu;
    // Main Panel Texts
    public Text healthText;
    public Text experienceText;
    public Text junakLevelText;
    public Text saralfHealthText;
    public Text saralfExperienceText;
    public Text saralfLevelText;
    // Ability Texts
    public Text investigateText;
    // Item Texts
    public Text healthPotionText, stoneText;
    // Panels
    public GameObject AbilityPanel;
    public GameObject ItemPanel;
    public GameObject mainPanel;
    public GameObject allySelectOutsideCombatPanel;
    // Buttons
    public Button abilityReturnButton;
    public Button itemReturnButton;
    public Button healthPotionButton;
    public Button abilitiesButton;
    public Button itemsButton;
    public Button loadButton;
    public Button JunakButton;
    public Button SaralfButton;
    // Images
    public GameObject saralfImage;

    private bool itemMode;

    private void Awake()
    {
        if (gameMenuManager == null)
        {
            DontDestroyOnLoad(gameObject);
            gameMenuManager = this;
        }
        else if (gameMenuManager != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Set the gameMenu to not activate at the start
        gameMenuManager.gameMenu.SetActive(false);
        // Make the AbilityPanel not active at the start
        AbilityPanel.SetActive(false);
        // Make the mainPanel Active
        mainPanel.SetActive(true);
        canInteractWith = true;
        itemMode = false;
    }

    private void checkPlayerOneAbilities()
    {
        if (DataManager.Junak.abilityManager.aquiredAbilities.Contains(InvestigateAbility.investigateAbility)) { investigateText.text = "Investigate"; } else { investigateText.text = ""; }
    }
    private void checkContainedItems(string itemName, Text textBox)
    {
        if (DataManager.Junak.itemManager.aquiredItems.Contains(itemName))
        {
            textBox.text = itemName + " " + DataManager.Junak.itemManager.getAmountOfItem(itemName);
        }
        else
        {
            textBox.text = "";
        }
    }
    private void checkPlayerOneItems()
    {
        healthPotionText.text = "Health Potion " + DataManager.Junak.itemManager.getAmountOfItem("Health Potion");
        checkContainedItems("Stone", stoneText);

        if (!DataManager.Junak.itemManager.aquiredItems.Any() && ItemPanel.activeSelf)
        {
            itemReturnButton.Select();
        }
    }
    public void whenTurnedOn()
    {
        AbilityPanel.SetActive(false);
        ItemPanel.SetActive(false);
        mainPanel.SetActive(true);
        if (DataManager.Junak.hasSaved)
        {
            loadButton.interactable = true;
            loadButton.Select();
        }
        else
        {
            loadButton.interactable = false;
        }
        checkPlayerOneAbilities();
        checkParty();
    }

    public void showAbilities()
    {
        // Show AbilityPanel
        AbilityPanel.SetActive(true);
        // Deactivate mainPanel
        mainPanel.SetActive(false);

        // Make Return the default Button 
        abilityReturnButton.Select();
    }
    public void hideAbilities()
    {
        // Deactivate Abilities Panel
        AbilityPanel.SetActive(false);
        // Activate mainPanel
        mainPanel.SetActive(true);

        // Make Abilities Button default button
        abilitiesButton.Select();
    }

    public void showItems()
    {
        // Show Item Panel
        ItemPanel.SetActive(true);
        // Deactivate mainPanel
        mainPanel.SetActive(false);

        // make Return default button
        itemReturnButton.Select();
    }
    public void hideItems()
    {
        //Deactivate item panel
        ItemPanel.SetActive(false);
        // activate mainPanel
        mainPanel.SetActive(true);

        // Make Items default button
        itemsButton.Select();
    }
    // Activate drop mode, where the user can drop items instead of use them
    public void dropMode()
    {
        itemMode = !itemMode;
    }
    private void checkParty()
    {
        if (SaralfDataManager.Saralf.isInParty)
        {
            saralfImage.SetActive(true);
        }
        else
        {
            saralfImage.SetActive(false);
            saralfHealthText.text = "";
            saralfExperienceText.text = "";
            saralfLevelText.text = "";
        }
    }

    public void allySelectPanelWhenTurnedOn()
    {
        SaralfButton.gameObject.SetActive(SaralfDataManager.Saralf.isInParty);
        allySelectOutsideCombatPanel.SetActive(true);
        JunakButton.Select();

    }
    public void selectAllyOutsideCombat(string allyName)
    {
        if (allyName == "Junak")
        {
            DataManager.Junak.itemManager.allyToTarget = DataManager.Junak;
        }
        else if (allyName == "Saralf")
        {
            DataManager.Junak.itemManager.allyToTarget = SaralfDataManager.Saralf;
        }
        DataManager.Junak.itemManager.aquiredItems.Remove(DataManager.Junak.itemManager.itemToUse);
        DataManager.Junak.itemManager.useItem(DataManager.Junak);
        allySelectOutsideCombatPanel.SetActive(false);
        canInteractWith = true;
    }

    void Update()
    {
        // Things that can change while in the gameMenu
        healthText.text = "Health: " + DataManager.Junak.health;
        experienceText.text = "Experience: " + DataManager.Junak.experience;
        junakLevelText.text = "Level: " + DataManager.Junak.level.ToString();
        checkPlayerOneItems();
        if (SaralfDataManager.Saralf.isInParty)
        {
            saralfHealthText.text = "Health: " + SaralfDataManager.Saralf.health;
            saralfExperienceText.text = "Experience: " + SaralfDataManager.Saralf.experience;
            saralfLevelText.text = "Level: " + SaralfDataManager.Saralf.level.ToString();
        }
    }
}
