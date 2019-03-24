using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public class SaveAndLoadData : MonoBehaviour {

    private void Start() {
    }

    // Save data here. Use the data instance of the PlayerData class
    // to store what needs to be saved.
    public void save() {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/UnyteGameData.dat");
        PlayerData data = new PlayerData();
        data.deadEnemies = EnemyDataManager.EnemyManager.defeatedEnemies;
        data.health = DataManager.manager.health;
        data.experience = DataManager.manager.experience;
        data.xpos = DataManager.manager.xpos;
        data.ypos = DataManager.manager.ypos;
        data.currentScene = DataManager.manager.currentScene;

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
            DataManager.manager.health = data.health;
            DataManager.manager.experience = data.experience;
            DataManager.manager.xpos = data.xpos;
            DataManager.manager.ypos = data.ypos;
            DataManager.manager.currentScene = data.currentScene;

            SceneManager.LoadScene(DataManager.manager.currentScene);
            DataManager.manager.isBeingLoaded = true;
        }
    }
    // Called when making a new game
    public void newGame() {

        EnemyDataManager.EnemyManager.defeatedEnemies.Clear();
        DataManager.manager.health = 10;
        DataManager.manager.experience = 0;
        DataManager.manager.xpos = 0;
        DataManager.manager.ypos = 0;

        SceneManager.LoadScene("Player'sHouseScene");
        DataManager.manager.isBeingLoaded = true;

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
