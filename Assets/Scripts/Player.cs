using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameManager GameManager;
    public float JumpStrength = 100;
    public float Speed = 10;
    public Rigidbody2D rb;
    public Animator anim;
    public KeyCode KeyLeft = KeyCode.None;
    public KeyCode KeyRight = KeyCode.None;
    public KeyCode KeyJump = KeyCode.None;

    private bool canJump = true;
    public Vector2 startPosition;

    // Use this for initialization
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(canJump == true && Input.GetKey(KeyJump))
        {
            rb.AddForce(new Vector2(0f, JumpStrength));
            canJump = false;
        }

        if (Input.GetKey(KeyLeft))
        {
            rb.velocity = new Vector2(-Speed, rb.velocity.y);
            anim.SetBool("walk", true);
            transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
        }
        else if (Input.GetKey(KeyRight))
        {
            rb.velocity = new Vector2(Speed, rb.velocity.y);
            anim.SetBool("walk", true);
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
        else
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
            anim.SetBool("walk", false);
        }

        anim.SetBool("jump", !canJump);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.PlayerCollision();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (rb.velocity.y <= 0f)
        {
            canJump = true;
        }
    }

    public void ResetPosition()
    {
        transform.position = startPosition;
    }
}
