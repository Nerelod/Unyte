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
    public GameObject ItemPanel;
    public GameObject mainPanel;

    public Button abilityReturnButton;
    public Button itemReturnButton;
    public Button abilitiesButton;
    public Button itemsButton;
    public Button loadButton;

    void Start() {
        // Set the gameMenu to not activate at the start
        gameMenu.SetActive(false);
        // Make the AbilityPanel not active at the start
        AbilityPanel.SetActive(false);
        // Make the mainPanel Active
        mainPanel.SetActive(true);

    }


    public void whenTurnedOn(){

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
        healthText.text = "Health: " + DataManager.manager.health;
        experienceText.text = "Experience: " + DataManager.manager.experience;
        /* 
        if(gameMenu.activeSelf){
            if(mainPanel.activeSelf){
                loadButton.Select();
            }
            else if(AbilityPanel.activeSelf){
                abilityReturnButton.Select();
            }
        }*/
    }
}
