using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveAndLoadData : MonoBehaviour {

    public GameObject loadButton;
    
    private void Start() {
    }



    // Save data here. Use the data instance of the PlayerData class
    // to store what needs to be saved.
    public void save() {
        DataManager.playerOne.hasSaved = true;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/UnyteGameData.dat");
        PlayerData data = new PlayerData();
        data.items = DataManager.playerOne.itemManager.aquiredItems;
        data.abilities = DataManager.playerOne.abilityManager.aquiredAbilities;
        data.deadEnemies = EnemyDataManager.EnemyManager.defeatedEnemies;
        data.health = DataManager.playerOne.health;
        data.experience = DataManager.playerOne.experience;
        data.xpos = DataManager.playerOne.xpos;
        data.ypos = DataManager.playerOne.ypos;
        data.currentScene = DataManager.playerOne.currentScene;
        data.saved = DataManager.playerOne.hasSaved;
        data.removedItems = DataManager.playerOne.itemManager.itemsThatWereRemoved;
        bf.Serialize(file, data);
        file.Close();

        
    }
    // Load data here. Use the data instance of PlayerData to 
    // set equal what needs to be loaded.
    public void load() {
        if (File.Exists(Application.persistentDataPath + "/UnyteGameData.dat")) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/UnyteGameData.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            EnemyDataManager.EnemyManager.defeatedEnemies = data.deadEnemies;
            DataManager.playerOne.health = data.health;
            DataManager.playerOne.experience = data.experience;
            DataManager.playerOne.xpos = data.xpos;
            DataManager.playerOne.ypos = data.ypos;
            DataManager.playerOne.currentScene = data.currentScene;
            DataManager.playerOne.itemManager.aquiredItems = data.items;
            DataManager.playerOne.abilityManager.aquiredAbilities = data.abilities;
            DataManager.playerOne.hasSaved = data.saved;
            DataManager.playerOne.itemManager.itemsThatWereRemoved = data.removedItems;

            SceneManager.LoadScene(DataManager.playerOne.currentScene);           

            DataManager.playerOne.isBeingLoaded = true;
            
        }
    }
    // Called when making a new game
    public void newGame() {

        EnemyDataManager.EnemyManager.defeatedEnemies.Clear();
        DataManager.playerOne.health = 10;
        DataManager.playerOne.experience = 0;
        DataManager.playerOne.qDamage = 3;
        SaralfDataManager.Saralf.health = 12;
        SaralfDataManager.Saralf.experience = 0; 
        DataManager.playerOne.theName = "Junak";
        DataManager.playerOne.abilityManager.aquiredAbilities.Clear();
        DataManager.playerOne.itemManager.aquiredItems.Clear();
        DataManager.playerOne.itemManager.itemsThatWereRemoved.Clear();
        DataManager.playerOne.abilityManager.aquiredAbilities.Add("Investigate");
        DataManager.playerOne.itemManager.aquiredItems.Add("Health Potion");
        DataManager.playerOne.isBeingLoaded = true;
        DataManager.playerOne.xpos = 1.4f;
        DataManager.playerOne.ypos = .39f;
        SceneManager.LoadScene("Cutscene_1");
        
        

    }
    /* PlayerData class. This is the class that stores the data 
     * when saving and loading. Add variables to be saved here and 
     * set them in the save and load methods. */
    [Serializable]
    class PlayerData {
        public int experience;
        public int health;
        public float xpos, ypos;
        public string currentScene;

        public bool saved; 
        public List<string> deadEnemies = new List<string>();
        public List<string> items = new List<string>();
        public List<string> abilities = new List<string>();
        public List<string> removedItems = new List<string>();
    }
    
}
