using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public GameManager GameManager;
    public Rigidbody2D rb;

    void FixedUpdate()
    {
        rb.velocity = new Vector2(0f, -GameManager.ScrollingSpeed);

        if(transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }
}
