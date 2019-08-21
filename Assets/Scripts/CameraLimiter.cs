using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLimiter : MonoBehaviour
{
    public float cameraXpos;
    public float cameraYpos;

    public float yLimit;
    public float xLimit;

    public PlayerController player;
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        cameraXpos = transform.position.x;
        cameraYpos = transform.position.y;
    }
}
