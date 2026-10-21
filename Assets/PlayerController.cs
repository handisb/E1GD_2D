using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerController : MonoBehaviour
{
    float movementX = 0;
    float movementY = 0;
    float speed = 100;
    float jumpSpeed = 5000;
    int score = 0;
    bool onFloor = false;
    public bool onWall = false;
    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer sr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("running", !Mathf.Approximately(rb.linearVelocityX, 0));
        animator.SetBool("jumping", (rb.linearVelocityY > 0));
        animator.SetBool("falling", (rb.linearVelocityY < 0));
        if (!Mathf.Approximately(rb.linearVelocityX, 0))
        {
            sr.flipX = (rb.linearVelocityX < 0);
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(movementX * speed * Time.fixedDeltaTime, rb.linearVelocityY);
        if (onFloor && movementY > 0)
        {
            /*if (onWall)
            {
                rb.AddForceX(-10 * speed * Time.fixedDeltaTime);
            }*/
            animator.SetBool("walljumping", onFloor && onWall);
            rb.AddForceY(jumpSpeed * movementY * Time.fixedDeltaTime);
            onFloor = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            onFloor = true;
            print(collision.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            onFloor = true;
        }
        if (collision.gameObject.CompareTag("Coin"))
        {
            jumpSpeed *= 3f;
            score++;
        }
    }

    void OnMove(InputValue value)
    {
        Vector2 v = value.Get<Vector2>();

        movementX = v.x;
        movementY = v.y;
    }
}
