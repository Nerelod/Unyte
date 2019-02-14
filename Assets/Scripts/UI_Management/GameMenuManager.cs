using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuManager : MonoBehaviour
{

    public GameObject gameMenu;

    public Font theFont;
    public int fontSize;
    private GUIStyle guiStyle = new GUIStyle(); 

    void Start()
    {
        gameMenu.SetActive(false);
        
    }

    
    void Update()
    {
        
    }

    private void OnGUI()
    {
        guiStyle.fontSize = fontSize;
        guiStyle.font = theFont;
        guiStyle.normal.textColor = Color.white;
        GUI.Label(new Rect(150, 250, 100, 100), "Health: " + DataManager.manager.health, guiStyle);
        GUI.Label(new Rect(150, 300, 100, 100), "Experience: " + DataManager.manager.experience, guiStyle);
    }
}
