using System;
using UnityEngine;

public class PatrolEnemy : MonoBehaviour, IDamageable
{
    
    public int maxHealth = 5;
    
    public float moveSpeed = 2f;
    
    public Transform checkPoint;
    
    public float distance = 1f;
    
    public LayerMask layerMask;
    
    private bool _facingLeft = true;
    
    private bool _inRange = false;
    
    public Transform player;

    public float attackRange = 10f;

    public float retrieveDistance = 2.5f;
    
    public float chaseSpeed = 4f;
    
    public Animator animator;

    public Transform attackPoint;
    public float attackRadius = 1f;
    public LayerMask attackLayer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (!FindObjectOfType<GameManager>().isGameActive) return;
        
        
        if (maxHealth <= 0)
        {
            Die();
        }

        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            _inRange = true;
        }
        else
        {
            _inRange = false;
        }

        if (_inRange)
        {

            if (player.position.x > transform.position.x && _facingLeft)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                _facingLeft = false;
            } else if (player.position.x < transform.position.x && !_facingLeft)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                _facingLeft = true;
            }
            
            if (Vector2.Distance(transform.position, player.position) > retrieveDistance)
            {
                animator.SetBool("Attack1", false);
                transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
            }
            else
            {
                animator.SetBool("Attack1", true);
            }
        }
        else
        {
            transform.Translate(Vector2.left * (Time.deltaTime * moveSpeed));
        
            var hit = Physics2D.Raycast(checkPoint.position, Vector2.down, distance, layerMask);
            if (!hit && _facingLeft)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                _facingLeft = false;
            } else if (!hit && !_facingLeft)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                _facingLeft = true;
            }
        }
    }

    //Called inside on attack animation
    public void Attack()
    {
        var colliderInfo = Physics2D.OverlapCircle(attackPoint.position, attackRadius, attackLayer);

        if (colliderInfo)
        {
            if (colliderInfo.gameObject.GetComponent<Player>())
            {
                colliderInfo.gameObject.GetComponent<Player>().TakeDamage(1);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (maxHealth <= 0) return;
        maxHealth -= damage;
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        if(!checkPoint)  return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(checkPoint.position, Vector2.down * distance);

        if (!transform) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        
        if (!attackPoint) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
