using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public GameObject attackPosition;
    public GameObject airAttackPosition;

    private Animator animator;
    public LayerMask enemyMask;
    public float attackRange;
    private bool isAttacking;
    private PlayerMovement playerMovement;
    private Rigidbody2D rb;

    public float attackCooldown = 0.5f;

    public float slamForce = 20f;

    [SerializeField] private float cooldownTimer = 0f;



    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        cooldownTimer -= Time.deltaTime;
        // bool isPressingDown = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
        bool isPressingDown = false;

        if (Input.GetKeyDown(KeyCode.C) && cooldownTimer <= 0f && !isAttacking && playerMovement.isGrounded)
        {
            isAttacking = true;
            animator.SetTrigger("attack");
            cooldownTimer = attackCooldown;

        }

        if (Input.GetKeyDown(KeyCode.C) && isPressingDown && cooldownTimer <= 0f && !isAttacking && !playerMovement.isGrounded)
        {
            isAttacking = true;
            animator.SetTrigger("airAttack");
            cooldownTimer = attackCooldown;

            playerMovement.canMove = false;

            rb.velocity = new Vector2(rb.velocity.x, -slamForce);
        }
    }

    public void Attack()
    {

        Collider2D[] enemys = Physics2D.OverlapCircleAll(attackPosition.transform.position, attackRange, enemyMask);

        foreach (Collider2D enemy in enemys)
        {

            // enemy.transform.GetComponent<Enemy>().TakeDamage(1);

            Enemy_Health_test1 enemyHealth = enemy.GetComponent<Enemy_Health_test1>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(1, transform.position);
            }
        }

    }

    public float airAttackDistance = 3f;

    public void AirAttack()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(
            transform.position,
            attackRange,
            Vector2.down,
            airAttackDistance,
            enemyMask
        );

        foreach (RaycastHit2D hit in hits)
        {
            hit.collider.GetComponent<Enemy>()?.TakeDamage(1);
        }
    }


    public void EndAttack()
    {
        isAttacking = false;
        playerMovement.canMove = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPosition.transform.position, attackRange);
        if (airAttackPosition != null)
        {
            Gizmos.DrawWireSphere(airAttackPosition.transform.position, attackRange);
        }
    }
}



