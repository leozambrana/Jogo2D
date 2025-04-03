using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rb;
    
    public float jumpForce = 5f;
    
    public bool isGrounded;
    
    private float _movement;

    public float moveSpeed = 5f;
    
    private bool _facingRight = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        _movement = Input.GetAxis("Horizontal");

        switch (_movement)
        {
            case < 0f when _facingRight:
                transform.eulerAngles = new Vector3(0, -180f, 0);
                _facingRight = false;
                break;
            case > 0 when !_facingRight:
                transform.eulerAngles = new Vector3(0, 0, 0);
                _facingRight = true;
                break;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
            isGrounded = false;
            animator.SetBool("Jump", true);
        }

        if (Math.Abs(_movement) > .1f)
        {
            animator.SetFloat("Run", 1);
        } 
        else if (_movement < .1f)
        {
            animator.SetFloat("Run", 0f);
        }

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
        }
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(_movement, 0f, 0f) * (Time.fixedDeltaTime * moveSpeed);
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("Jump", false);
        }
    }
}
