using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float currentHealth;
    [SerializeField] private float maxHealth;

    [Header("Knockback")]
    private bool isKnockback = false;
    [SerializeField] private float knockbackForceX = 6f;
    [SerializeField] private float knockbackForceY = 5f;
    [SerializeField] private float knockbackTime = 0.2f;


    private PlayerRespawn playerRespawn;

    public GameObject effect;
    private bool isDead = false;
    private Rigidbody2D rb;
    private PlayerMovement playerMovement;
    void Start()
    {
        currentHealth = maxHealth;
        playerRespawn = GetComponent<PlayerRespawn>();

        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame                                          
    void Update()
    {

    }

    IEnumerator Die()
    {
        isDead = true;

        Instantiate(effect, transform.position, Quaternion.identity);

        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        // 🔒 Freeze toàn bộ trục
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        // Hide Player
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<PlayerMovement>().enabled = false;

        yield return new WaitForSeconds(2f);

        currentHealth = maxHealth;
        playerRespawn.Respawn();

        // 🔓 Mở lại constraint (thường giữ freeze rotation)
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        // Show Player
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
        GetComponent<PlayerMovement>().enabled = true;

        isDead = false;
    }

    IEnumerator Knockback(Vector2 hitDirection)
    {
        if (isKnockback) yield break;
        isKnockback = true;
        playerMovement.enabled = false;
        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(hitDirection.x * knockbackForceX, knockbackForceY), ForceMode2D.Impulse);

        yield return new WaitForSeconds(knockbackTime);

        playerMovement.enabled = true;
        isKnockback = false;
    }

    public void TakeDamage(float damage, Vector2 enemyPosition)
    {
        if (isDead) return;

        currentHealth -= damage;

        Vector2 hitDirection = (transform.position.x - enemyPosition.x > 0) ? Vector2.right : Vector2.left;

        StartCoroutine(Knockback(hitDirection));

        if (currentHealth <= 0 && !isDead)
        {
            StartCoroutine(Die());
        }


    }



}
