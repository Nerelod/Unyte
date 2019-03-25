using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour {
    public int width;
    public int height;
    void Start() { 
        Screen.SetResolution(width, height, true);
    }

    // Update is called once per frame
    void Update() { 
        
    }
}
