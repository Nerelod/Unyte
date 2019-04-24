using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatMenuManager : MonoBehaviour
{
    public static CombatMenuManager combatMenuManager;
    public GameObject abilitySelectPanel;
    public Button abilityReturnButton;

    public GameObject itemSelectPanel;
    public Button itemReturnButton;

    void Start()
    {
        abilitySelectPanel.SetActive(false);
        itemSelectPanel.SetActive(false);
    }
    void Update()
    {
        
    }
}
