using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

    [SerializeField] GameObject sword;

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

        if (Input.GetButtonDown("Fire1") && hasSword)
        {
            Debug.Log("FIRE!");
            UseSword();
        }

        float horizontalSpeed = Mathf.Abs(rb.linearVelocity.x);
        anim.SetFloat("Speed", horizontalSpeed); // defining speed to the running animation

    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

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
            Debug.Log("Touched the Sword!");

            EquipSword();
            Destroy(other.gameObject);
        }
    }

    private void EquipSword()
    {
        hasSword = true;
    }


    private void UseSword()
    {
        if (!sword.activeSelf)
        {
            StartCoroutine(showSword());// must use StartCoroutine to execute IEnumerator methods
        }
    }

    private IEnumerator showSword()
    {
        // activate the sword
        sword.SetActive(true);

        // wait 1 sec
        yield return new WaitForSeconds(swordSwingDuration);

        // deactivate sword
        sword.SetActive(false);
    }

}
