using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transitions : MonoBehaviour
{
    public static Transitions screenTransition;

    public GameObject fadeInPanel; //Fade from black
    public GameObject fadeOutPanel; //Fade to black

    void Start()
    {
    }

    private void Awake() {   
        if (screenTransition == null) {        
            DontDestroyOnLoad(gameObject);
            screenTransition = this;
        }
        else if (screenTransition != this) {       
            Destroy(gameObject);
        }    
        if(fadeInPanel != null) {       
            GameObject panel = Instantiate(fadeInPanel, Vector3.zero, Quaternion.identity) as GameObject;
            Destroy(panel, 1);
        }
    }

    public IEnumerator FadeOut(string scene, float wait) {   
        if (fadeOutPanel != null) {         
            Instantiate(fadeOutPanel, Vector3.zero, Quaternion.identity);
        }
        yield return new WaitForSeconds(wait);
        if(scene != ""){
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene);
            while (!asyncOperation.isDone) { yield return null; }
        }
    }
    public IEnumerator FadeIn(string scene, float wait){
        if (fadeInPanel != null) {         
            Instantiate(fadeInPanel, Vector3.zero, Quaternion.identity);
        }
        yield return new WaitForSeconds(wait);
        if(scene != ""){
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene);
            while (!asyncOperation.isDone) { yield return null; }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
