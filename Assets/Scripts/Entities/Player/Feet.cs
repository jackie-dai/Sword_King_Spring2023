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
        Debug.Log("triggering feet");
        if (other.tag == "Floor")
        {
            Debug.Log("triggering feet inside if statment");
            player.ResetJump();
        }
    }
}
