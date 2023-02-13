using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    public bool canJump = true;
    private Animator controller;
    private int facingRight = -1;
    private float checkRadius = 10;
    [SerializeField]
    private LayerMask floorObjects;
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

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<Animator>();
        swords[0] = swordCollider.GetComponent<Sword>();
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
       //Handles attack
        if (Input.GetKey(KeyCode.Mouse0))
        {
            controller.Play("Slash");
            controller.SetBool("isHitting", true);
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
    //Handles the sword slash attack
    IEnumerator Slash()
    {
        swordCollider.SetActive(true);
        float timeElapsed = 0;
        Quaternion startRotation = swordCollider.transform.rotation;
        Quaternion targetRotation = startRotation * Quaternion.Euler(0, 0, facingRight * 120);
   
        while (timeElapsed < slashDuration)
        {
            swordCollider.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / slashDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        swordCollider.transform.rotation = Quaternion.Euler(0, 0, 0);
        controller.SetBool("isHitting", false);
        swordCollider.SetActive(false);
    }


    private void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 direction = new Vector3(horizontalInput, 0, 0);

        transform.Translate(direction * movementSpeed * Time.deltaTime);

        if (horizontalInput > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            controller.SetBool("isMoving", true);
            facingRight = -1;
        } else if (horizontalInput < 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1, transform.localScale.y, transform.localScale.z);
            controller.SetBool("isMoving", true);
            facingRight = 1;
        } else
        {
            controller.SetBool("isMoving", false);
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

    public void ResetJump()
    {
        Debug.Log("rests jmp");
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
