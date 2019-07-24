using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    public CutsceneTextManager textManager;

    public bool waitIsFinished;

    void Start()
    {
      
    }

    public void moveObject(GameObject objectToMove, GameObject destination, float moveSpeed) {
        objectToMove.transform.position = Vector2.MoveTowards(objectToMove.transform.position, destination.transform.position, moveSpeed * Time.fixedDeltaTime);
    }
    // Waits a few seconds
    public IEnumerator Wait(int seconds) {
        waitIsFinished = false;
        yield return new WaitForSeconds(seconds);
        waitIsFinished = true;
    }

    void Update()
    {

    }
}
