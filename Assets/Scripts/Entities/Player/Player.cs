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

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animationController = GetComponent<Animator>();
        animationController.SetBool("isHitting", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            rb.velocity = Vector2.up * jumpVelocity;
            canJump = false;
        }
       
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            animationController.SetTrigger("Attack");
            StartCoroutine(Attack());
        }

        CalculateMovement();
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(attackDelay);
        Debug.Log("Attack range: " + Vector2.one * attackRange);
        RaycastHit2D[] enemiesHit = Physics2D.BoxCastAll(rb.position + currentDirection, Vector2.one * attackRange, 0f, Vector2.zero);
        foreach (RaycastHit2D hit in enemiesHit)
        {
            if (hit.transform.tag == "Enemy")
            {
                Enemy enemy = hit.transform.GetComponent<Enemy>();
                enemy.TakeDamage();
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
