using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenFlameBozz : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    /* PREFABS */
    /* EDITABLE VARIABLES */
    [SerializeField]
    private int _lives = 10;
    [SerializeField]
    private float strafeAmount = 1.5f;
    [SerializeField]
    private float strafeDuration = 0.5f;
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
        StartCoroutine(Strafe());
    }

    public GameObject bulletPrefab;
    public float shootSpeed = 300;

    private bool playerInRange = false;
    private float lastAttackTime = 0f;
    private float fireRate = 0.5f;

    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.tag);
        if (other.tag == "Player")
        {
            playerInRange = true;
            Debug.Log("Taking damage");
            player.GetComponent<Player>().takeDamage(1);
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "player")
        {
            playerInRange = false;
        }
    }

    void Update()
    {
        if (playerInRange)
        {
            if (Time.time - lastAttackTime >= 1f / fireRate)
            {
                Debug.Log("Shooting");
                ShootBullet();
                lastAttackTime = Time.time;
            }
        }
    }

    void ShootBullet()
    {
        var projectile = Instantiate(bulletPrefab, transform.position, transform.rotation);
        projectile.GetComponent<Rigidbody>().velocity = transform.forward * shootSpeed;
    }


    /* Moves object left and right by offset strafeAmount */
    IEnumerator Strafe()
    {
        yield return new WaitForSeconds(1f);
        while (true)
        {
            Vector3 startPos = transform.position;
            Vector3 targetPos = new Vector3(transform.position.x, transform.position.y + strafeAmount, transform.position.z);
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
            player.gold += 1;
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
        }
        else
        {
            spriteRenderer.color = Color.white;
        }

        yield return new WaitForSeconds(0.25f);
        if (animationControl != null)
        {
            animationControl.SetBool("Hit", false);
            spriteRenderer.color = Color.white;
        }
        else
        {
            spriteRenderer.color = Color.red;
        }
    }

}
