using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

[Serializable]

public class DataManager : MonoBehaviour
{

    public static DataManager manager;
    public int experience;
    public int health;
    public int speed = 5;
    public int qDamage;
    public string currentScene;
    public float xpos, ypos;
    public string theName;
    public Boolean isBeingLoaded = false;

    private void Awake()
    {

        if (manager == null)
        {
            DontDestroyOnLoad(gameObject);
            manager = this;
        }
        else if (manager != this)
        {
            Destroy(gameObject);
        }
        
    }
    private void Start() {
        theName = "Player One";
    }
    void addExperience(int experienceToAdd)
    {
        experience = experience + experienceToAdd;
    }
    void resetExperience()
    {
        experience = 0;
    }
    void manageHealth(int healthToAdd)
    {
        health = health + healthToAdd;
    }

    private void Update()
    {
        currentScene = SceneManager.GetActiveScene().name;
    }
}



