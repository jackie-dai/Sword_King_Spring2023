using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    /* PREFABS */
    /* EDITABLE VARIABLES */
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private float strafeAmount = 2f;
    [SerializeField]
    private float strafeDuration = 1f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        StartCoroutine(Strafe());
    }

    /* Moves object left and right by offset strafeAmount */
    IEnumerator Strafe()
    {
        yield return new WaitForSeconds(1f);
        while (true)
        {
            Vector3 startPos = transform.position;
            Vector3 targetPos = new Vector3(transform.position.x + strafeAmount, transform.position.y, transform.position.z);
            float elapsedTime = 0;

            while (elapsedTime < strafeDuration)
            {
                transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / strafeDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            strafeAmount *= -1;
        }
    }
    
    /* Reduces health and flashes white -> red on hit */
    public void TakeDamage()
    {
        _lives -= 1;
        StartCoroutine(Flash());
        if (_lives < 1)
        {
            Destroy(this.gameObject);
        }
    }

    /* Coroutine for flashing white & red */
    IEnumerator Flash()
    {
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.25f);
        spriteRenderer.color = Color.red;
    }

}
