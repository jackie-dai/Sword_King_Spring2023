using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Item : MonoBehaviour
{
    public string name;
    public Player player;
    // Awake is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);   
    }

    /*// called first
    void OnEnable()
    {
        //Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>();
    }
    */
    // Update is called once per frame
    void Update()
    {
        
    }

    public void useAbility()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>();
        if (name == "Health Potion")
        {
            //Add health
            player.takeDamage(-2);
        } else if (name == "Damage Potion")
        {
            player.damageAm = 2;
            player.WaitEffect(4.0f, this);
        } else if (name == "Speed Potion")
        {
            player.movementSpeed = 20f;
            player.WaitEffect(4.0f, this);
        }
        else if (name == "Jump Potion")
        {
            player.jumpVelocity = 800f;
            player.WaitEffect(4.0f, this);
        }
    }

    public void setPlayer(Player new_player)
    {
        player = new_player;
    }
}
