using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    private Scene currentScene;
    /* PlEASE MAKE SURE TO UPDATE NEXTSCENEINDEX IN UNITY INTERFACE */
    /* Default: 0 -> "Demo Level" */
    [SerializeField]
    private int nextSceneIndex = 0;

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
            SceneManager.LoadScene(nextSceneIndex);

            /* Updates to new scene */
            Start();
        }
    }
    public void reload()
    {
        SceneManager.LoadScene(currentScene.name);
    }
}
