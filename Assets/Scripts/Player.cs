using UnityEngine;
using System.Collections;
using System;

public class Player : MonoBehaviour
{

    [SerializeField] PlayerSword sword;

    [Header("Movement Settings")]
    public float moveSpeed = 8f;
    public float jumpForce = 12f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator anim;

    private float moveInput;
    private bool isGrounded;

    public bool hasSword = false;
    public float swordSwingDuration = 0.2f;

    public static event Action OnPlayerDeath;

    public float jumpCutMultiplier = 0.5f; // multiplier for jump height when releasing jump button early (lower value = shorter jump)


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
        }

        if (Input.GetButtonDown("Fire1") && hasSword)
        {
            UseSword();
        }

        float horizontalSpeed = Mathf.Abs(rb.linearVelocity.x);
        anim.SetFloat("Speed", horizontalSpeed); // defining speed to the running animation

    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = 4f;
        }
        else
        {
            rb.gravityScale = 3f; 
        }

        if (moveInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Sword"))
        {
            EquipSword();
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Anvil"))
        {
            sword.Repair();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Slime"))
        {
            Die();
        }
    }

    private void EquipSword()
    {
        hasSword = true;
    }


    private void UseSword()
    {
        if (!sword.gameObject.activeSelf)
        {
            StartCoroutine(showSword());// must use StartCoroutine to execute IEnumerator methods
        }
    }

    private IEnumerator showSword()
    {
        // activate the sword
        sword.gameObject.SetActive(true);

        // wait 1 sec
        yield return new WaitForSeconds(swordSwingDuration);

        // deactivate sword
        sword.gameObject.SetActive(false);
    }

    private void Die()
    {

        // trigger the event if there are listeners
        OnPlayerDeath?.Invoke();

        // disable the player object or visuals
        gameObject.SetActive(false);
    }

}
