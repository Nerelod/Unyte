﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ability
{

    public int experience;
    public int comboNumber;
    public string name;

    public virtual void execute() { }
}
