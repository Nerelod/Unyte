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
    public static DataManager manager;

    // amount of experience
    public int experience;
    // amount of health
    public int health;
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
    public Boolean isBeingLoaded = false;

    // Happens before start, makes sure there is only one instance of DataManager
    private void Awake() {    
        if (manager == null) {        
            DontDestroyOnLoad(gameObject);
            manager = this;
        }
        else if (manager != this) {       
            Destroy(gameObject);
        }      
    }
    private void Start() {
        theName = "Player One";
    }
    // Method for adding experience to total experience
    public void addExperience(int experienceToAdd) {    
        experience = experience + experienceToAdd;
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



