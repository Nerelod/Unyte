using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ability
{
    public int experience;
    public string name;
    public List<string> compatibleAbilities = new List<string>();
    public virtual void execute() { }
}
