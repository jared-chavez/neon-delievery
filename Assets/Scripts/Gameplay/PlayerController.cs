using UnityEngine;

namespace Platformer.Gameplay {}

namespace Platformer.Mechanics
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteRenderer))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Jax Bionic Physics")]
        public float moveSpeed = 8f;        
        public float jumpForce = 14f;       
        public float backjumpForceX = 6f;   
        public float backjumpForceY = 12f;  
        public float climbSpeed = 5f;       

        [Header("Ground Detection")]
        public Transform groundCheck;       
        public float groundCheckRadius = 0.2f;
        public LayerMask groundLayer;       

        [Header("Weapon FX")]
        public GameObject bulletPrefab; 
        public Transform firePoint;     
        public float bulletSpeed = 20f;

        private Rigidbody2D rb;
        private Animator animator;
        private SpriteRenderer spriteRenderer;

        private float horizontalInput;
        private float verticalInput;        
        private bool isGrounded;
        private bool isCrouching;
        private bool isShooting;

        private bool isAtLadder = false;
        private bool isClimbing = false;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            rb.gravityScale = 3.5f; 
        }

        void Update()
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");

            // Climbing Logic
            if (isAtLadder && Mathf.Abs(verticalInput) > 0.1f)
            {
                isClimbing = true;
            }

            // Crouch (Only if not climbing)
            isCrouching = (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && !isClimbing && isGrounded;
            animator.SetBool("isCrouching", isCrouching);

            // Shoot Logic with Power Validation
            if (Input.GetKeyDown(KeyCode.Z) || Input.GetButtonDown("Fire1"))
            {
                // Verify if GameManager exists and has enough power
                if (GameManager.Instance != null && GameManager.Instance.power > 0)
                {
                    animator.SetBool("isShooting", true);
                    ShootFX();
                }
                else
                {
                    Debug.Log("Out of power! Find a battery.");
                }
            }
            else if (Input.GetKeyUp(KeyCode.Z) || Input.GetButtonUp("Fire1"))
            {
                animator.SetBool("isShooting", false);
            }

            // Jump (Releases from ladder)
            if (Input.GetButtonDown("Jump") && (isGrounded || isClimbing) && !isCrouching)
            {
                isClimbing = false; 
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }

            // Evasion (Backjump)
            if (Input.GetKeyDown(KeyCode.C) && isGrounded && !isClimbing)
            {
                animator.SetTrigger("backjump");
                ExecuteBackjump();
            }

            // Visual Direction
            if (horizontalInput > 0) spriteRenderer.flipX = false;
            else if (horizontalInput < 0) spriteRenderer.flipX = true;

            // Animator Sync
            float visualSpeed = isCrouching ? 0f : Mathf.Abs(horizontalInput);
            animator.SetFloat("velocityX", visualSpeed);
            animator.SetBool("grounded", isGrounded);
            animator.SetBool("isClimbing", isClimbing);

            // PATCH: Freeze animation if idle on ladder
            if (isClimbing && Mathf.Abs(verticalInput) < 0.1f && Mathf.Abs(horizontalInput) < 0.1f)
            {
                animator.speed = 0f; 
            }
            else
            {
                animator.speed = 1f; 
            }
        }

        void FixedUpdate()
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

            if (isClimbing)
            {
                rb.gravityScale = 0f; 
                rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, verticalInput * climbSpeed);
            }
            else
            {
                rb.gravityScale = 3.5f; 
                
                if (!isCrouching)
                {
                    rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
                }
                else
                {
                    rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                }
            }
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Ladder"))
            {
                isAtLadder = true;
            }
        }

        void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Ladder"))
            {
                isAtLadder = false;
                isClimbing = false; 
                animator.speed = 1f; // Safety reset when exiting ladder
            }
        }

        private void ExecuteBackjump()
        {
            float direction = spriteRenderer.flipX ? 1f : -1f;
            rb.linearVelocity = new Vector2(direction * backjumpForceX, backjumpForceY);
        }

        // --- FUNCIÓN DE DISPARO TOTALMENTE ACTUALIZADA ---
        private void ShootFX()
        {
            if(bulletPrefab != null && firePoint != null)
            {
                // 1. Instanciamos la bala
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                
                // 2. Obtenemos sus componentes de física y gráficos
                Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
                SpriteRenderer bulletSprite = bullet.GetComponent<SpriteRenderer>();

                // 3. Calculamos la dirección (1 es derecha, -1 es izquierda)
                float direction = spriteRenderer.flipX ? -1f : 1f;

                if (bulletRb != null)
                {
                    // 4. Aplicamos el Tiro Recto (Velocidad solo en X)
                    bulletRb.linearVelocity = new Vector2(bulletSpeed * direction, 0f);
                }

                if (bulletSprite != null)
                {
                    // 5. Volteamos el sprite de la bala para que apunte a donde debe
                    bulletSprite.flipX = spriteRenderer.flipX;
                }

                // Consumir energía de la barra
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.ConsumePower();
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (groundCheck != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
            }
        }

        public void ResetDash() { }
    }
}