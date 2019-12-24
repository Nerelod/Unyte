using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemString;
    public string identifier;
    public PlayerController player;

    public ItemScript itemScript;

    void Start()
    {
        /*if (itemString == "Health Potion") { itemScript = HealthPotionItem.healthPotionItem; }
        else if (itemString == "Stone") { itemScript = StoneItem.stoneItem; }*/
        player = FindObjectOfType<PlayerController>();
        // Set item to false if it was aquired by the player
        if (DataManager.Junak.itemManager.itemsThatWereRemoved.Contains(identifier))
        {
            this.gameObject.SetActive(false);
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
}
