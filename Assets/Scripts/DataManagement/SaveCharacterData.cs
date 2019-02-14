/*using UnityEngine;

public class SaveCharacterData : MonoBehaviour
{
    public DataManager characterData;

    void Update()
    {
 
    }

    public static void SaveCharacter(DataManager data, int characterSlot)
    {
        PlayerPrefs.SetInt("Health_CharacterSlot" + characterSlot, data.health);
        PlayerPrefs.SetInt("Experience_CharacterSlot" + characterSlot, data.experience);
        PlayerPrefs.SetString("CurrentScene_CharacterSlot" + characterSlot, data.currentScene);
        PlayerPrefs.Save();
    }

    public static DataManager LoadCharacter(int characterSlot)
    {
        DataManager loadedCharacter = new DataManager();
        loadedCharacter.health = PlayerPrefs.GetInt("Healh_CharacterSlot" + characterSlot);
        loadedCharacter.experience = PlayerPrefs.GetInt("Experience_CharacterSlot" + characterSlot);
        loadedCharacter.currentScene = PlayerPrefs.GetString("CurrentScene_CharacterSlot" + characterSlot);

        return loadedCharacter;
    }
}
*/