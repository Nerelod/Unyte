using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]

public class ScrutinizeAbility : ComboAbility
{
    public static ScrutinizeAbility scrutinizeAbility = new ScrutinizeAbility();
    public ScrutinizeAbility()
    {
        comboIdentifier = 1;
        name = "Scrutinize";
    }
}
