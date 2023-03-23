using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEnemyDetection : MonoBehaviour
{
    [SerializeField] Vector2 target;
    [SerializeField] float speed = 5.0f;
    [SerializeField] bool startedMovement = false;
    [SerializeField] bool inDetectionRange = false;
    [SerializeField] GameObject player;


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
            player = collision.gameObject;
            target = new Vector2(collision.transform.position.x, transform.parent.transform.position.y);
            inDetectionRange = true;
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
            inDetectionRange = false;
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
        while (inDetectionRange) {
            while (Vector2.Distance(transform.parent.position, target) > 0.5)
            {
                yield return new WaitForSeconds(0.0000001f);
                float step = speed * Time.deltaTime;
                transform.parent.position = Vector2.MoveTowards(transform.parent.position, target, step);
                target = new Vector2(player.transform.position.x, transform.parent.transform.position.y);
            }
            yield return new WaitForSeconds(0.25f);
        }

        startedMovement = false;

    }
}


