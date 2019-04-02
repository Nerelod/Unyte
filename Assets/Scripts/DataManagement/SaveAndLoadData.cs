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
        if(loadButton != null){
            if(DataManager.playerOne.hasSaved){
                loadButton.SetActive(true);
            }
            else{
                loadButton.SetActive(false);
            }
        }
    }



    // Save data here. Use the data instance of the PlayerData class
    // to store what needs to be saved.
    public void save() {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/UnyteGameData.dat");
        PlayerData data = new PlayerData();
        data.deadEnemies = EnemyDataManager.EnemyManager.defeatedEnemies;
        data.health = DataManager.playerOne.health;
        data.experience = DataManager.playerOne.experience;
        data.xpos = DataManager.playerOne.xpos;
        data.ypos = DataManager.playerOne.ypos;
        data.currentScene = DataManager.playerOne.currentScene;

        bf.Serialize(file, data);
        file.Close();

        DataManager.playerOne.hasSaved = true;
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

            SceneManager.LoadScene(DataManager.playerOne.currentScene);           

            DataManager.playerOne.isBeingLoaded = true;
        }
    }
    // Called when making a new game
    public void newGame() {

        EnemyDataManager.EnemyManager.defeatedEnemies.Clear();
        DataManager.playerOne.health = 10;
        DataManager.playerOne.experience = 0;
        DataManager.playerOne.xpos = 0;
        DataManager.playerOne.ypos = 0;

        SceneManager.LoadScene("Player'sHouseScene");
        
        DataManager.playerOne.isBeingLoaded = true;

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
        public List<string> deadEnemies = new List<string>();
    }
}
