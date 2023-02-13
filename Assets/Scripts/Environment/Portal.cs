using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    private Scene currentScene;

    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
    }

    void Update()
    {
        /* Restarts level if R is pressed */
        /* ONLY WORKS IF PORTAL PREFAB IS IN SCENE */
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(currentScene.name);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            /* PlEASE MAKE SURE TO UPDATE CODE EACH TIME YOU CREATE A NEW LEVEL */

            if (currentScene.name == "Level1")
            {
                SceneManager.LoadScene(0); // 0 is demo level 
            }

            /* Updates to new scene */
            currentScene = SceneManager.GetActiveScene();
        }
    }
}
