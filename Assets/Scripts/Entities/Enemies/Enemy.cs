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
    [SerializeField]
    private Animator animationControl;
    [SerializeField]
    private Player player;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        if (this.gameObject.name != "Mouse")
        {
            StartCoroutine(Strafe());
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            Debug.Log("Taking damage");
            player.GetComponent<Player>().takeDamage(1);
        }
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
        public void TakeDamage(int amount)
    {
        _lives -= amount;
        //Debug.Log("Hit");
        StartCoroutine(Flash());
        if (_lives < 1)
        {
            player.setGold(-1);
            player.displayItems();
            Destroy(this.gameObject);
        }
    }

    /* Coroutine for flashing white & red */
    IEnumerator Flash()
    {

        if (animationControl != null)
        {
            animationControl.SetBool("Hit", true);
            spriteRenderer.color = Color.red;
        } else
        {
            spriteRenderer.color = Color.white;
        }

        yield return new WaitForSeconds(0.25f);
        if (animationControl != null)
        {
           animationControl.SetBool("Hit", false);
            spriteRenderer.color = Color.white;
        } else
        {
            spriteRenderer.color = Color.red;
        }
    }

}
