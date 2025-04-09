using UnityEngine;

public class ReaperBoss : MonoBehaviour, IDamageable
{
    public int maxHealth = 30;

    public float moveSpeed = 5f;
    public float attackDistance = 2f;

    public Transform player;
    public Animator animator;

    private bool _facingLeft = true;

    public Transform attackPoint;
    public float attackRadius = 1f;
    public LayerMask attackLayer;

    private void Update()
    {
        if (!FindObjectOfType<GameManager>().isGameActive) return;
        if (maxHealth <= 0)
        {
            Die();
        };
        
        
        float distance = Vector2.Distance(transform.position, player.position);

        // Virar pro player
        if (player.position.x > transform.position.x && _facingLeft)
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
            _facingLeft = false;
        }
        else if (player.position.x < transform.position.x && !_facingLeft)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            _facingLeft = true;
        }

        if (distance > attackDistance)
        {
            animator.SetBool("Attack", false);
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("Attack", true);
        }
    }

    // Chamada via Animation Event
    public void Attack(int damage)
    {
        var colliderInfo = Physics2D.OverlapCircle(attackPoint.position, attackRadius, attackLayer);
        if (colliderInfo)
        {
            if (colliderInfo.gameObject.GetComponent<Player>())
            {
                colliderInfo.gameObject.GetComponent<Player>().TakeDamage(damage);
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
        animator.SetTrigger("Die");
        Destroy(this.gameObject, 1f);
    }

    private void OnDrawGizmosSelected()
    {
        if (!attackPoint) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
