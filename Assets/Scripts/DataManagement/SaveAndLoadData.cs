using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveAndLoadData : MonoBehaviour
{

    public GameObject loadButton;

    private void Start()
    {
    }



    // Save data here. Use the data instance of the PlayerData class
    // to store what needs to be saved.
    public void save()
    {
        DataManager.Junak.hasSaved = true;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/UnyteGameData.dat");
        PlayerData data = new PlayerData();
        data.items = DataManager.Junak.itemManager.aquiredItems;
        data.JuankAbilities = DataManager.Junak.abilityManager.aquiredAbilities;
        data.SaralfAbilities = SaralfDataManager.Saralf.abilityManager.aquiredAbilities;
        data.JunakComboAbilities = DataManager.Junak.abilityManager.aquiredComboAbilities;
        data.SaralfComboAbilities = SaralfDataManager.Saralf.abilityManager.aquiredComboAbilities;
        data.deadEnemies = EnemyDataManager.EnemyManager.defeatedEnemies;
        data.health = DataManager.Junak.health;
        data.experience = DataManager.Junak.experience;
        data.level = DataManager.Junak.level;
        data.xpos = DataManager.Junak.xpos;
        data.ypos = DataManager.Junak.ypos;
        data.currentScene = DataManager.Junak.currentScene;
        data.saved = DataManager.Junak.hasSaved;
        data.removedItems = DataManager.Junak.itemManager.itemsThatWereRemoved;
        bf.Serialize(file, data);
        file.Close();


    }
    // Load data here. Use the data instance of PlayerData to 
    // set equal what needs to be loaded.
    public void load()
    {
        if (File.Exists(Application.persistentDataPath + "/UnyteGameData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/UnyteGameData.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            EnemyDataManager.EnemyManager.defeatedEnemies = data.deadEnemies;
            DataManager.Junak.health = data.health;
            DataManager.Junak.experience = data.experience;
            DataManager.Junak.level = data.level;
            DataManager.Junak.xpos = data.xpos;
            DataManager.Junak.ypos = data.ypos;
            DataManager.Junak.currentScene = data.currentScene;
            DataManager.Junak.itemManager.aquiredItems = data.items;
            DataManager.Junak.abilityManager.aquiredAbilities = data.JuankAbilities;
            DataManager.Junak.abilityManager.aquiredComboAbilities = data.JunakComboAbilities;
            SaralfDataManager.Saralf.abilityManager.aquiredAbilities = data.SaralfAbilities;
            SaralfDataManager.Saralf.abilityManager.aquiredComboAbilities = data.SaralfComboAbilities;
            DataManager.Junak.hasSaved = data.saved;
            DataManager.Junak.itemManager.itemsThatWereRemoved = data.removedItems;

            SceneManager.LoadScene(DataManager.Junak.currentScene);

            DataManager.Junak.isBeingLoaded = true;

        }
    }
    // Called when making a new game
    public void newGame()
    {

        EnemyDataManager.EnemyManager.defeatedEnemies.Clear();
        DataManager.Junak.health = 10;
        DataManager.Junak.experience = 0;
        DataManager.Junak.qDamage = 3;
        SaralfDataManager.Saralf.health = 12;
        SaralfDataManager.Saralf.experience = 0;
        DataManager.Junak.theName = "Junak";
        DataManager.Junak.abilityManager.aquiredAbilities.Clear();
        DataManager.Junak.abilityManager.aquiredComboAbilities.Clear();
        SaralfDataManager.Saralf.abilityManager.aquiredAbilities.Clear();
        SaralfDataManager.Saralf.abilityManager.aquiredComboAbilities.Clear();
        DataManager.Junak.itemManager.aquiredItems.Clear();
        DataManager.Junak.itemManager.itemsThatWereRemoved.Clear();
        DataManager.Junak.abilityManager.aquiredAbilities.Add("Investigate");
        SaralfDataManager.Saralf.abilityManager.aquiredAbilities.Add("Analyze");
        DataManager.Junak.itemManager.aquiredItems.Add("Health Potion");
        DataManager.Junak.isBeingLoaded = true;
        DataManager.Junak.xpos = 1.4f;
        DataManager.Junak.ypos = .39f;
        SceneManager.LoadScene("Cutscene_1");



    }
    /* PlayerData class. This is the class that stores the data 
     * when saving and loading. Add variables to be saved here and 
     * set them in the save and load methods. */
    [Serializable]
    class PlayerData
    {
        public int experience;
        public int level;
        public int health;
        public float xpos, ypos;
        public string currentScene;

        public bool saved;
        public List<string> deadEnemies = new List<string>();
        public List<string> items = new List<string>();
        public List<string> JuankAbilities = new List<string>();
        public List<string> SaralfAbilities = new List<string>();
        public List<string> removedItems = new List<string>();
        public List<ComboAbility> JunakComboAbilities = new List<ComboAbility>();
        public List<ComboAbility> SaralfComboAbilities = new List<ComboAbility>();
    }

}
