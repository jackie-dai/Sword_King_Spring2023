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

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            rb.velocity = Vector2.up * jumpVelocity;
            canJump = false;
        }
       
        if (Input.GetKey(KeyCode.Mouse0))
        {
            controller.Play("Slash");
            controller.SetBool("isHitting", true);
            StartCoroutine(Slash());
        }

        CalculateMovement();
    }

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
