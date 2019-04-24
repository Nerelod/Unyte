using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatMenuManager : MonoBehaviour
{
    public static CombatMenuManager combatMenuManager;
    public GameObject abilitySelectPanel;

    void Start()
    {
        abilitySelectPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
