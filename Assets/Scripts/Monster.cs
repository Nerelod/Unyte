using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour{ 

    public bool textWasPrompt = false;
    public bool displayedDamage = false;
    
    public int whichAttack;    

    public Monster() {

    }

    public virtual void Attack(DataManager target){}
    public virtual void DisplayDamage(DataManager target){}
}
    

