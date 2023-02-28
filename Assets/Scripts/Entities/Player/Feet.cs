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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Floor")
        {
            player.ResetJump();
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (player.canJump == true) // this triggers only if the player is not touching the ground anymore
        {
            player.canJump = false;
        }
    }

}

