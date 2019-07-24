using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    public CutsceneTextManager textManager;

    void Start()
    {
      
    }

    public void moveObject(GameObject objectToMove, GameObject destination, float moveSpeed) {
        objectToMove.transform.position = Vector2.MoveTowards(objectToMove.transform.position, destination.transform.position, moveSpeed * Time.fixedDeltaTime);
    }

    void Update()
    {

    }
}
