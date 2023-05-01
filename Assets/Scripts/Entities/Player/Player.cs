using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField]
    public bool canJump = true;
    private Animator animationController;
    private float currentDirection = 1;
    private SpriteRenderer sp;
    /* Attack Variables */
    private float attackRange = 5f;
    private float attackDelay = 0.25f;
    /* PREFABS */
    /* EDITABLE VARIABLES */
    #region Dash 
    private float dashVelocity = 400f;
    private float dashCooldown = 5f;
    private bool canDash = true;
    #endregion
    #region Health
    [SerializeField]
    public float maxhp = 10;
    [SerializeField]
    public static float health = 10;
    public int damageAm = 1;

    #endregion
    [SerializeField]
    public float jumpVelocity = 400f;
    [SerializeField]
    public float movementSpeed = 5f;
    private float slashDuration = 0.3f;
    [SerializeField]
    public static int gold = 1;
    public Sword[] swords;
    [SerializeField]
    public static Item[] items = new Item[3];
    private static int swordInt = 1;
    private static int itemInt = 0;
    [SerializeField]
    private bool inMarket = false;
    public MarketScript currMarket;
    public Portal portal;
    public Text player_text;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animationController = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();
        maxhp = 10;
        displayItems();
    }

    // Update is called once per frame
    void Update()
    {
        //Handles jump
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            rb.AddForce(new Vector2(0f, jumpVelocity));
            //canJump = false;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            Dash();
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            animationController.SetTrigger("Attack");
            StartCoroutine(Slash());
        }
        if (Input.GetKeyDown(KeyCode.B) && currMarket != null)
        {
            currMarket.buy(this);
            displayItems();
        }
        if (Input.GetKeyDown(KeyCode.S) && currMarket != null)
        {
            currMarket.sell(this);
            displayItems();
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
        if (this.gameObject.transform.position.y < -50.0f)
        {
            health = 10.0f;
            portal.reload();
        }

        CalculateMovement();
    }

    IEnumerator Slash()
    {
        yield return new WaitForSeconds(attackDelay);
        Debug.Log("Attack range: " + Vector2.one * attackRange);
        RaycastHit2D[] enemiesHit = Physics2D.BoxCastAll(rb.position + new Vector2(currentDirection, 0f), Vector2.one * attackRange, 0f, Vector2.zero);
        foreach (RaycastHit2D hit in enemiesHit)
        {
            if (hit.transform.tag == "Enemy")
            {
                if (hit.transform.GetComponent<Enemy>() == null)
                {
                    GreenFlameBozz enemy = hit.transform.GetComponent<GreenFlameBozz>();
                    enemy.TakeDamage(damageAm);
                } else
                {
                    Enemy enemy = hit.transform.GetComponent<Enemy>();
                    enemy.TakeDamage(damageAm);
                }
                
            }
        }
    }

    public int getGold()
    {
        return gold;
    }

    public void setGold(int am)
    {
        gold -= am;
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
            currentDirection = 1;
        }
        else if (horizontalInput < 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1, transform.localScale.y, transform.localScale.z);
            animationController.SetBool("isMoving", true);
            currentDirection = -1;
        }
        else
        {
            animationController.SetBool("isMoving", false);
        }
    }

    void Dash()
    {
        rb.AddForce(new Vector2(dashVelocity * currentDirection, 0f));
        canDash = false;
        StartCoroutine(DashCooldown());
    }

    IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
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
        newItem.setPlayer(this);
        items[itemInt] = newItem;
        itemInt += 1;
        if (itemInt > items.Length - 1)
        {
            itemInt = 0;
        }
    }

    public void removeItem(Item oldItem)
    {
        int index = System.Array.IndexOf(items, oldItem);
        items[index] = null;
        for (int i = index; i < items.Length - 1; i++)
        {
            items[i] = items[i + 1];
        }
        itemInt -= 1;
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
        displayItems();
    }

    public void takeDamage(int amount)
    {
        health -= amount;
        StartCoroutine(PlayDamageIndicator());
        if (health < 0)
        {
            health = 10.0f;
            portal.reload();
            //Destroy(this.gameObject);
        }
    }

    IEnumerator PlayDamageIndicator()
    {
        sp.color = Color.grey;
        yield return new WaitForSeconds(0.25f);
        sp.color = Color.white;
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
            portal.reload();
            Destroy(this.gameObject);
        }
        if (other.transform.tag == "Bullet")
        {
            takeDamage(2);
        }
        if (other.transform.tag == "Market")
        {
            Debug.Log("Collided with Market");
            currMarket = other.transform.gameObject.GetComponent<MarketScript>();
            inMarket = true;
            player_text.text = ("Press B to Buy, S to Sell. Items sold: " + currMarket.info() + " " + displayItems());


        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Market")
        {
            if (inMarket == true)
            {
                currMarket = null;
                inMarket = false;
                player_text.text = displayItems();
            }
        }
    }

    public string displayItems()
    {
        string totalString = "Money: " + gold + " Inventory: ";
        int index = 1;
        foreach (Item item in items)
        {
            if (item != null)
            {
                totalString = totalString + index + ":" + item.name + ", ";
            }
            index += 1;
        }
        if (player_text != null)
        {
            player_text.text = totalString;
        }
        //Debug.Log(totalString);
        return totalString;

    }

    public void WaitEffect(float time, Item effectItem)
    {
        //Debug.Log("In wait effect");
        damageAm += 1;
        StartCoroutine(waitEffectCor(time, effectItem.name));
    }

    public IEnumerator waitEffectCor(float time, string name)
    {
        yield return new WaitForSeconds(time);
        endEffect(name);
    }

    public void endEffect(string itemName)
    {
        if (itemName == "Damage Potion")
        {
            damageAm = 1;
        }
        else if (itemName == "Speed Potion")
        {
            movementSpeed = 7f;
        }
        else if (itemName == "Jump Potion")
        {
            jumpVelocity = 400f;
        }
    }
}
