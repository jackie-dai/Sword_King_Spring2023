using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hp : MonoBehaviour
{

        
    
    public Image healthBarImage;
    public Player player;

    public void Update()
    {
        healthBarImage.fillAmount = Mathf.Clamp(Player.health/player.maxhp, 0, 1f);
    }
    
}