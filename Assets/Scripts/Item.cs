using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemString;
    public string identifier;
    public PlayerController player;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        // Set item to false if it was aquired by the player
        if (DataManager.playerOne.itemManager.itemsThatWereRemoved.Contains(identifier)){
            this.gameObject.SetActive(false);
        }
    }

    void OnTriggerStay2D(Collider2D collision) {     
        if (collision.gameObject.tag == "Player" && Input.GetKeyDown(KeyCode.Space) && player.State == States.CanMove) {         
            DataManager.playerOne.itemManager.aquiredItems.Add(itemString);
            DataManager.playerOne.itemManager.itemsThatWereRemoved.Add(identifier);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
