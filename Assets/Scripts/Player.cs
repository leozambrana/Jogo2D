using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameOver gameOver;
    public int currentCoin = 0;
    public TMP_Text coin;

    public int maxHealth = 3;
    public TMP_Text health;
    
    public Animator animator;
    public Rigidbody2D rb;
    
    public float jumpForce = 5f;
    
    public bool isGrounded;
    
    private float _movement;

    public float moveSpeed = 5f;
    
    private bool _facingRight = true;
    
    public Transform attackPoint;
    public float attackRadius = 1f;
    public LayerMask attackLayer;

    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (maxHealth <= 0)
        {
            maxHealth = 0;
            Die();
        }
        
        coin.text = currentCoin.ToString();
        health.text = maxHealth.ToString();
        
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

    public void Attack()
    {
        Debug.Log("AHH SE N CHEGOU AQUI A VACA FOI PRO BREJO");
        var collisionInfo = Physics2D.OverlapCircle(attackPoint.position, attackRadius, attackLayer);
        Debug.Log(collisionInfo);
        if (!collisionInfo) return;
        Debug.Log("Nao ta ne hitando");
        var damageable = collisionInfo.GetComponent<IDamageable>();
        Debug.Log(damageable);
        damageable?.TakeDamage(1);
    }

    private void OnDrawGizmosSelected()
    {
        if (!attackPoint) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            currentCoin++;
            other.gameObject.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Collected");
            Destroy(other.gameObject, 1f);
        }

        if (other.gameObject.CompareTag("VictoryPoint"))
        {
            FindObjectsOfType<SceneManagement>()[0].LoadLevel("BossFight");
        }
    }

    public void TakeDamage(int damage)
    {
        if(maxHealth <= 0) return;
        maxHealth -= damage;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void Die()
    {
        animator.SetTrigger("Death");
        FindObjectOfType<GameManager>().isGameActive = false;
        Destroy(this.gameObject, 1f);
        gameOver.Setup(currentCoin);
    }
}
