using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    /* PREFABS */
    [SerializeField]
    private Transform player;
    /* EDITABLE VARIABLES */
    [SerializeField]
    private float offsetX = 2;
    [SerializeField]
    private float offsetY = -2;

    void Update()
    {
        transform.position = new Vector3(player.position.x + offsetX, player.position.y + offsetY, -10);
    }
}
