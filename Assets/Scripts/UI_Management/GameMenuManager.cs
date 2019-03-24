using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuManager : MonoBehaviour {

    public GameObject gameMenu;


    public Text healthText;
    public Text experienceText;

    public GameObject AbilityPanel = null;

    void Start() {
        gameMenu.SetActive(false);
        AbilityPanel.SetActive(false);
    }

    public void showAbilities() {
        AbilityPanel.SetActive(true);
    }

    void Update() {
        healthText.text = "Health: " + DataManager.manager.health;
        experienceText.text = "Experience: " + DataManager.manager.experience;
    }

}
