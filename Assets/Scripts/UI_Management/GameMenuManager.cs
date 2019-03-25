using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameMenuManager : MonoBehaviour {

    public GameObject gameMenu;


    public Text healthText;
    public Text experienceText;

    public GameObject AbilityPanel;
    public GameObject mainPanel;

    public Button returnButton;
    public Button abilitiesButton;

    void Start() {
        // Set the gameMenu to not activate at the start
        gameMenu.SetActive(false);
        // Make the AbilityPanel not active at the start
        AbilityPanel.SetActive(false);
        // Make the mainPanel Active
        mainPanel.SetActive(true);
    }

    public void showAbilities() {
        // Show AbilityPanel
        AbilityPanel.SetActive(true);
        // Deactivate mainPanel
        mainPanel.SetActive(false);
        
        // Make Return the default Button 
        returnButton.Select();      
    }
    public void hideAbilities() {
        // Deactivate Abilities Panel
        AbilityPanel.SetActive(false);
        // Activate mainPanel
        mainPanel.SetActive(true);

        // Make Abilities Button default button
        abilitiesButton.Select();
    }

    void Update() {
        healthText.text = "Health: " + DataManager.manager.health;
        experienceText.text = "Experience: " + DataManager.manager.experience;
    }

}
