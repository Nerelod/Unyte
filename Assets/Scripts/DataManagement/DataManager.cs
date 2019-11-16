using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

[Serializable]

public class DataManager : MonoBehaviour { 

    // reference to itself
    public static DataManager Junak;

    public AbilityManager abilityManager;
    public ItemManager itemManager;
    // amount of experience
    public int experience;
    public int experienceNeeded;
    public int level;
    // amount of health
    public int health;
    // total amount of health possible
    public int totalHealth;
    // amount of speed
    public int speed = 5;
    // the damage dealt with basic attack Q
    public int qDamage;
    // holder for the current scene
    public string currentScene;
    // the x and y locations of the sprite
    public float xpos, ypos;
    // name of the character 
    public string theName;
    // the number assigned to them for combat order
    public int assignedOrderInCombat;
    // Boolean for if there is a scene change or the 
    // game is being loaded
    public bool isBeingLoaded = false;
    // If the character has ever saved before
    public bool hasSaved = false;
    // if the character just ran from a combat encounter
    public bool ranFromCombat = false;

    public bool isTurnInCombat = false;

    public bool isInParty;

    public int directionHolder;

    // Happens before start, makes sure there is only one instance of DataManager
    private void Awake() {    
        if (Junak == null) {        
            DontDestroyOnLoad(gameObject);
            Junak = this;
        }
        else if (Junak != this) {       
            Destroy(gameObject);
        }
    }
    private void Start() {
        abilityManager = gameObject.AddComponent<AbilityManager>() as AbilityManager;
        itemManager = gameObject.AddComponent<ItemManager>() as ItemManager;

        Junak.theName = "Junak";

        Junak.abilityManager.aquiredAbilities.Add("Investigate");
        experienceNeeded = 10;

    }
    // Method for adding experience to total experience
    public void addExperience(int experienceToAdd) {    
        experience = experience + experienceToAdd;
        if (experience >= experienceNeeded) {
            levelUp();
        }
    }
    public void levelUp() {
        level += 1;
        experience = experience - experienceNeeded;
        experienceNeeded = (experienceNeeded * 2) - (experienceNeeded/2);
    }
    // Resets the experience back to 0
    public void resetExperience() {    
        experience = 0;
    }
    // Adds or subtracts health
    public void manageHealth(int healthToAdd) {     
        health = health + healthToAdd;
    }

    private void Update() {
     
        currentScene = SceneManager.GetActiveScene().name; //TODO: instead of getting the scene every frame, update the scene when switching scenes
        
    }
}



