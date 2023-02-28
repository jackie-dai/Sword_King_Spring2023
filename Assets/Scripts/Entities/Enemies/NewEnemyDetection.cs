using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEnemyDetection : MonoBehaviour
{
    [SerializeField] Vector2 target;
    [SerializeField] float speed = 5.0f;
    [SerializeField] bool startedMovement = false;
    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            target = new Vector2(collision.transform.position.x, transform.parent.transform.position.y);

            if (!startedMovement)
            {
                StartCoroutine(Movement());
                startedMovement = true;
            }
            
            // transform.parent.transform.position = collision.transform.position;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            Debug.Log("Exited");

            target = new Vector2 (collision.transform.position.x, transform.parent.transform.position.y);

            if (startedMovement)
            {
                startedMovement = false;
            }

        }
    }

    IEnumerator Movement()
    {
        Debug.Log("In Movement");

        while (Vector2.Distance(transform.parent.position, target) > 0.5)
        {
            yield return new WaitForSeconds(0.0000001f);
            float step = speed * Time.deltaTime;
            transform.parent.position = Vector2.MoveTowards(transform.parent.position, target, step);
        }

        startedMovement = false;

    }
}


