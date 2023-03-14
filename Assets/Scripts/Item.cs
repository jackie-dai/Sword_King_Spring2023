using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string name;
    public Player player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void useAbility()
    {
        if (name == "Health Potion")
        {
            //Add health
            player.takeDamage(-2);
        }
    }
}
