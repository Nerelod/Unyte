﻿using System.Collections;
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
    // Junak Ability Texts
    public Text investigateText;
    // Saralf Ability Texts
    public Text analyzeText;
    // Item Texts
    public Text healthPotionText, stoneText;
    // Panels
    public GameObject junakAbilityPanel;
    public GameObject saralfAbilityPanel;
    public GameObject ItemPanel;
    public GameObject mainPanel;
    public GameObject allySelectOutsideCombatPanel;
    // Buttons
    public Button junakAbilityReturnButton;
    public Button saralfAbilityReturnButton;
    public Button itemReturnButton;
    public Button healthPotionButton;
    public Button junakAbilitiesButton;
    public Button saralfAbilitiesButton;
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
        junakAbilityPanel.SetActive(false);
        // Make the mainPanel Active
        mainPanel.SetActive(true);
        canInteractWith = true;
        itemMode = false;
    }

    private void checkJunakAbilities()
    {
        if (JunakDataManager.Junak.abilityManager.aquiredAbilities.OfType<InvestigateAbility>().Any()) { investigateText.text = "Investigate"; } else { investigateText.text = ""; }
    }
    private void checkSaralfAbilities()
    {
        if (SaralfDataManager.Saralf.abilityManager.aquiredAbilities.OfType<AnalyzeAbility>().Any()) { analyzeText.text = "Analyze"; } else { analyzeText.text = ""; }
    }
    private void checkContainedItems(string itemName, Text textBox)
    {
        if (JunakDataManager.Junak.itemManager.aquiredItems.Contains(itemName))
        {
            textBox.text = itemName + " " + JunakDataManager.Junak.itemManager.getAmountOfItem(itemName);
        }
        else
        {
            textBox.text = "";
        }
    }
    private void checkJunakItems()
    {
        healthPotionText.text = "Health Potion " + JunakDataManager.Junak.itemManager.getAmountOfItem("Health Potion");
        checkContainedItems("Stone", stoneText);

        if (!JunakDataManager.Junak.itemManager.aquiredItems.Any() && ItemPanel.activeSelf)
        {
            itemReturnButton.Select();
        }
    }
    public void whenTurnedOn()
    {
        junakAbilityPanel.SetActive(false);
        saralfAbilityPanel.SetActive(false);
        ItemPanel.SetActive(false);
        mainPanel.SetActive(true);
        if (JunakDataManager.Junak.hasSaved)
        {
            loadButton.interactable = true;
            loadButton.Select();
        }
        else
        {
            loadButton.interactable = false;
        }
        checkJunakAbilities();
        checkParty();
    }

    public void showAbilities(GameObject panel)
    {
        // Show AbilityPanel
        panel.SetActive(true);
        // Deactivate mainPanel
        mainPanel.SetActive(false);

        // Make Return the default Button 
        if (panel == junakAbilityPanel) { junakAbilityReturnButton.Select(); }
        else if (panel == saralfAbilityPanel) { saralfAbilityReturnButton.Select(); }
    }
    public void hideAbilities(GameObject panel)
    {
        // Deactivate Abilities Panel
        panel.SetActive(false);
        // Activate mainPanel
        mainPanel.SetActive(true);

        // Make Abilities Button default button
        if (panel == junakAbilityPanel) { junakAbilitiesButton.Select(); }
        else if (panel == saralfAbilityPanel) { saralfAbilitiesButton.Select(); }
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
            JunakDataManager.Junak.itemManager.allyToTarget = JunakDataManager.Junak;
        }
        else if (allyName == "Saralf")
        {
            JunakDataManager.Junak.itemManager.allyToTarget = SaralfDataManager.Saralf;
        }
        JunakDataManager.Junak.itemManager.aquiredItems.Remove(JunakDataManager.Junak.itemManager.itemToUse.name);
        JunakDataManager.Junak.itemManager.useItem(JunakDataManager.Junak);
        allySelectOutsideCombatPanel.SetActive(false);
        canInteractWith = true;
    }

    void Update()
    {
        // Things that can change while in the gameMenu
        healthText.text = "Health: " + JunakDataManager.Junak.health;
        experienceText.text = "Experience: " + JunakDataManager.Junak.experience;
        junakLevelText.text = "Level: " + JunakDataManager.Junak.level.ToString();
        checkJunakItems();
        if (SaralfDataManager.Saralf.isInParty)
        {
            saralfHealthText.text = "Health: " + SaralfDataManager.Saralf.health;
            saralfExperienceText.text = "Experience: " + SaralfDataManager.Saralf.experience;
            saralfLevelText.text = "Level: " + SaralfDataManager.Saralf.level.ToString();
        }
    }
}
