﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemString;
    void Start()
    {
        
    }

    void OnTriggerStay2D(Collider2D collision) {     
        if (collision.gameObject.tag == "Player" && Input.GetKeyDown(KeyCode.Space)) {         
            DataManager.playerOne.itemManager.aquiredItems.Add(itemString);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
