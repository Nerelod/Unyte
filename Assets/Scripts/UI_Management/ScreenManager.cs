using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour {
    public int width;
    public int height;
    public bool isFullScreen;
    void Start() { 
        Screen.SetResolution(width, height, isFullScreen);
    }

    // Update is called once per frame
    void Update() { 
        
    }
}
