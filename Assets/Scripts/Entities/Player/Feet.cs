using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feet : MonoBehaviour
{
    private Player player;

    void Start()
    {
        player = GetComponentInParent<Player>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
       
        if (collision.transform.tag == "Wall" || collision.transform.tag == "Market" || collision.transform.tag == "Floor")
        {
            player.ResetJump();
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Wall" || collision.transform.tag == "Market" || collision.transform.tag == "Floor")
        {
            if (player.canJump == true) // this triggers only if the player is not touching the ground anymore
            {
                player.canJump = false;
            }
        }
    }

}

