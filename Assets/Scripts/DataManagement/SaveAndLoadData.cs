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
        JunakDataManager.Junak.hasSaved = true;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/UnyteGameData.dat");
        PlayerData data = new PlayerData();
        data.items = JunakDataManager.Junak.itemManager.aquiredItems;
        data.itemScripts = JunakDataManager.Junak.itemManager.itemScripts;
        data.JuankAbilities = JunakDataManager.Junak.abilityManager.aquiredAbilities;
        foreach (Ability ability in data.JuankAbilities) { Debug.Log(ability.name); }
        data.SaralfAbilities = SaralfDataManager.Saralf.abilityManager.aquiredAbilities;
        data.JunakComboAbilities = JunakDataManager.Junak.abilityManager.aquiredComboAbilities;
        data.SaralfComboAbilities = SaralfDataManager.Saralf.abilityManager.aquiredComboAbilities;
        data.deadEnemies = EnemyDataManager.EnemyManager.defeatedEnemies;
        data.health = JunakDataManager.Junak.health;
        data.experience = JunakDataManager.Junak.experience;
        data.level = JunakDataManager.Junak.level;
        data.xpos = JunakDataManager.Junak.xpos;
        data.ypos = JunakDataManager.Junak.ypos;
        data.currentScene = JunakDataManager.Junak.currentScene;
        data.saved = JunakDataManager.Junak.hasSaved;
        data.removedItems = JunakDataManager.Junak.itemManager.itemsThatWereRemoved;
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
            JunakDataManager.Junak.health = data.health;
            JunakDataManager.Junak.experience = data.experience;
            JunakDataManager.Junak.level = data.level;
            JunakDataManager.Junak.xpos = data.xpos;
            JunakDataManager.Junak.ypos = data.ypos;
            JunakDataManager.Junak.currentScene = data.currentScene;
            JunakDataManager.Junak.itemManager.aquiredItems = data.items;
            JunakDataManager.Junak.itemManager.itemScripts = data.itemScripts;
            JunakDataManager.Junak.abilityManager.aquiredAbilities = data.JuankAbilities;
            foreach (Ability ability in JunakDataManager.Junak.abilityManager.aquiredAbilities) { Debug.Log(ability.name); }
            JunakDataManager.Junak.abilityManager.aquiredComboAbilities = data.JunakComboAbilities;
            SaralfDataManager.Saralf.abilityManager.aquiredAbilities = data.SaralfAbilities;
            SaralfDataManager.Saralf.abilityManager.aquiredComboAbilities = data.SaralfComboAbilities;
            JunakDataManager.Junak.hasSaved = data.saved;
            JunakDataManager.Junak.itemManager.itemsThatWereRemoved = data.removedItems;

            SceneManager.LoadScene(JunakDataManager.Junak.currentScene);

            JunakDataManager.Junak.isBeingLoaded = true;

        }
    }
    // Called when making a new game
    public void newGame()
    {

        EnemyDataManager.EnemyManager.defeatedEnemies.Clear();
        JunakDataManager.Junak.health = 10;
        JunakDataManager.Junak.experience = 0;
        JunakDataManager.Junak.qDamage = 3;
        SaralfDataManager.Saralf.health = 12;
        SaralfDataManager.Saralf.experience = 0;
        JunakDataManager.Junak.theName = "Junak";
        JunakDataManager.Junak.abilityManager.aquiredAbilities.Clear();
        JunakDataManager.Junak.abilityManager.aquiredComboAbilities.Clear();
        SaralfDataManager.Saralf.abilityManager.aquiredAbilities.Clear();
        SaralfDataManager.Saralf.abilityManager.aquiredComboAbilities.Clear();
        JunakDataManager.Junak.itemManager.aquiredItems.Clear();
        JunakDataManager.Junak.itemManager.itemScripts.Clear();
        JunakDataManager.Junak.itemManager.itemsThatWereRemoved.Clear();
        JunakDataManager.Junak.itemManager.itemScripts.Add(HealthPotionItem.healthPotionItem);
        JunakDataManager.Junak.abilityManager.aquiredAbilities.Add(InvestigateAbility.investigateAbility);
        JunakDataManager.Junak.abilityManager.aquiredComboAbilities.Add(ScrutinizeAbility.scrutinizeAbility);
        SaralfDataManager.Saralf.abilityManager.aquiredAbilities.Add(AnalyzeAbility.analyzeAbility);
        SaralfDataManager.Saralf.abilityManager.aquiredComboAbilities.Add(ScrutinizeAbility.scrutinizeAbility);
        JunakDataManager.Junak.itemManager.aquiredItems.Add("Health Potion");
        JunakDataManager.Junak.isBeingLoaded = true;
        JunakDataManager.Junak.xpos = 1.4f;
        JunakDataManager.Junak.ypos = .39f;
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
        public List<ItemScript> itemScripts = new List<ItemScript>();
        public List<Ability> JuankAbilities = new List<Ability>();
        public List<Ability> SaralfAbilities = new List<Ability>();
        public List<string> removedItems = new List<string>();
        public List<ComboAbility> JunakComboAbilities = new List<ComboAbility>();
        public List<ComboAbility> SaralfComboAbilities = new List<ComboAbility>();
    }

}
