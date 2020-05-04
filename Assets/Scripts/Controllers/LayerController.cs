using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerController : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private int layerOffset;
    void Start()
    {
        
    }


    void Update()
    {
        if (transform.position.y < player.transform.position.y) { // if below
            sprite.sortingOrder = player.render.sortingOrder + layerOffset;
        }
        else { // if above
            sprite.sortingOrder = player.render.sortingOrder - layerOffset;
        }
    }
}
