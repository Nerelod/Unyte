using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameMenuManager : MonoBehaviour {

    public static GameMenuManager gameMenuManager;

    public GameObject gameMenu;


    public Text healthText;
    public Text experienceText;

    public GameObject AbilityPanel;
    public GameObject ItemPanel;
    public GameObject mainPanel;

    public Button abilityReturnButton;
    public Button itemReturnButton;
    public Button abilitiesButton;
    public Button itemsButton;
    public Button loadButton;

    private void Awake() {
        if (gameMenuManager == null) {
            DontDestroyOnLoad(gameObject);
            gameMenuManager = this;
        }
        else if (gameMenuManager != this) {
            Destroy(gameObject);
        }
    }

    void Start() {
        // Set the gameMenu to not activate at the start
        gameMenuManager.gameMenu.SetActive(false);
        // Make the AbilityPanel not active at the start
        AbilityPanel.SetActive(false);
        // Make the mainPanel Active
        mainPanel.SetActive(true);

    }


    public void whenTurnedOn(){
        AbilityPanel.SetActive(false);
        ItemPanel.SetActive(false);
        mainPanel.SetActive(true);
        if(DataManager.playerOne.hasSaved){
            loadButton.interactable = true; 
            loadButton.Select();
        }
        else{
            loadButton.interactable = false;
        }
    }

    public void showAbilities() {
        // Show AbilityPanel
        AbilityPanel.SetActive(true);
        // Deactivate mainPanel
        mainPanel.SetActive(false);
        
        // Make Return the default Button 
        abilityReturnButton.Select();      
    }
    public void hideAbilities() {
        // Deactivate Abilities Panel
        AbilityPanel.SetActive(false);
        // Activate mainPanel
        mainPanel.SetActive(true);

        // Make Abilities Button default button
        abilitiesButton.Select();
    }

    public void showItems(){
        // Show Item Panel
        ItemPanel.SetActive(true);
        // Deactivate mainPanel
        mainPanel.SetActive(false);
        
        // make Return default button
        itemReturnButton.Select();
    }
    public void hideItems(){
        //Deactivate item panel
        ItemPanel.SetActive(false);
        // activate mainPanel
        mainPanel.SetActive(true);

        // Make Items default button
        itemsButton.Select();
    }

    void Update() {
        healthText.text = "Health: " + DataManager.playerOne.health;
        experienceText.text = "Experience: " + DataManager.playerOne.experience;
        
    }
}
