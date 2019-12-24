using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemScript
{
    public string name;
    public virtual void execute() { }
    public virtual void execute(DataManager player) { }
}
