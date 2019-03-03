using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public class SaveAndLoadData : MonoBehaviour
{
    
    private void Start() {     
    }
    public void save() {   
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/UnyteGameData.dat");
        PlayerData data = new PlayerData();
        data.health = DataManager.manager.health;
        data.experience = DataManager.manager.experience;
        data.xpos = DataManager.manager.xpos;
        data.ypos = DataManager.manager.ypos;
        data.currentScene = DataManager.manager.currentScene;

        bf.Serialize(file, data);
        file.Close();
    }
    public void load() {    
        if (File.Exists(Application.persistentDataPath + "/UnyteGameData.dat")) { 
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/UnyteGameData.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();
            DataManager.manager.health = data.health;
            DataManager.manager.experience = data.experience;
            DataManager.manager.xpos = data.xpos;
            DataManager.manager.ypos = data.ypos;
            DataManager.manager.currentScene = data.currentScene;

            SceneManager.LoadScene(DataManager.manager.currentScene);
            DataManager.manager.isBeingLoaded = true;            
        }
    }
}

[Serializable]
class PlayerData { 
    public int experience;
    public int health;
    public float xpos, ypos;
    public string currentScene;
}

