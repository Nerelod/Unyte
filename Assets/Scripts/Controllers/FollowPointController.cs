using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPointController : MonoBehaviour
{
    public PlayerController player;
    public Transform target;
    public float yMover;
    [SerializeField] private float distance;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    
    void Update()
    {
        if(player.direction == 1) { // if going down
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y + distance); 
        }
        else if(player.direction == 2) { // if going up
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y - distance);
        }
        else if(player.direction == 3) { // if going left
            transform.position = new Vector3(player.transform.position.x + distance, player.transform.position.y + yMover);
        }
        else if(player.direction == 4) { // if going right
            transform.position = new Vector3(player.transform.position.x - distance, player.transform.position.y + yMover);
        }
        else if(player.direction == 5) { // if going up left
            transform.position = new Vector3(player.transform.position.x + distance, player.transform.position.y - distance);
        }
        else if(player.direction == 6) { // if going up right
            transform.position = new Vector3(player.transform.position.x - distance, player.transform.position.y - distance);
        }
        else if(player.direction == 7) { // if going down left
            transform.position = new Vector3(player.transform.position.x + distance, player.transform.position.y + distance);
        }
        else if(player.direction == 8) { // if going down right
            transform.position = new Vector3(player.transform.position.x - distance, player.transform.position.y + distance);
        }

    }
}
