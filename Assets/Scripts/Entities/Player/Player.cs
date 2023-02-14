using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    public bool canJump = true;
    private Animator animationController;
    private Vector2 currentDirection = Vector2.right;
    /* Attack Variables */
    private float attackRange = 5f;
    private float attackDelay = 0.25f;
    /* PREFABS */
    [SerializeField]
    private GameObject swordCollider;
    /* EDITABLE VARIABLES */
    [SerializeField]
    public float jumpVelocity = 5f;
    [SerializeField]
    public float movementSpeed = 5f;
    private float slashDuration = 0.3f;
    [SerializeField]
    public int gold = 0;
    public Sword[] swords;
    public Item[] items;
    private int swordInt = 1;
    private int itemInt = 0;
    [SerializeField]
    private int health = 0;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animationController = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Handles jump
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            rb.velocity = Vector2.up * jumpVelocity;
            canJump = false;
        }
       
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            animationController.SetTrigger("Attack");
            StartCoroutine(Slash());
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            useItem(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            useItem(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            useItem(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            useItem(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            useItem(4);
        }

        CalculateMovement();
    }

    IEnumerator Slash()
    {
        yield return new WaitForSeconds(attackDelay);
        Debug.Log("Attack range: " + Vector2.one * attackRange);
        RaycastHit2D[] enemiesHit = Physics2D.BoxCastAll(rb.position + currentDirection, Vector2.one * attackRange, 0f, Vector2.zero);
        foreach (RaycastHit2D hit in enemiesHit)
        {
            if (hit.transform.tag == "Enemy")
            {
                Enemy enemy = hit.transform.GetComponent<Enemy>();
                enemy.TakeDamage(1);
            }
        }
    }

    private void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 direction = new Vector3(horizontalInput, 0, 0);

        transform.Translate(direction * movementSpeed * Time.deltaTime);

        if (horizontalInput > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            animationController.SetBool("isMoving", true);
            currentDirection = Vector2.right;
        } else if (horizontalInput < 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1, transform.localScale.y, transform.localScale.z);
            animationController.SetBool("isMoving", true);
            currentDirection = Vector2.left;
        } else
        {
            animationController.SetBool("isMoving", false);
        }
    }

    public void addSword(Sword newSword)
    {
        swords[swordInt] = newSword;
        swordInt += 1;
        if (swordInt > swords.Length - 1)
        {
            swordInt = 0;
        } 
    }

    public void addItem(Item newItem)
    {
        items[itemInt] = newItem;
        itemInt += 1;
        if (itemInt > items.Length - 1)
        {
            itemInt = 0;
        }
    }

    public void useItem(int itemIndex)
    {
        //int itemIndex = System.Array.IndexOf(items, item);
        Item item = items[itemIndex];
        if (item != null)
        {
            item.useAbility();
            items[itemIndex] = null;
            for (int i = itemIndex; i < items.Length - 1; i++)
            {
                items[i] = items[i + 1];
            }
            itemInt -= 1;
        }
    }

    public void takeDamage(int amount)
    {
        health -= amount;
        if (health < 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void ResetJump()
    {
        canJump = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {   
        if (other.transform.tag == "Spikes")
        {
            Debug.Log("killed");
            Destroy(this.gameObject);
        }
    }
}
