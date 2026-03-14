using UnityEngine;
namespace Platformer.Gameplay {}
namespace Platformer.Mechanics
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteRenderer))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Física Biónica de Jax")]
        public float moveSpeed = 8f;        
        public float jumpForce = 14f;       
        public float backjumpForceX = 6f;   
        public float backjumpForceY = 12f;  

        [Header("Detección de Suelo")]
        public Transform groundCheck;       
        public float groundCheckRadius = 0.2f;
        public LayerMask groundLayer;       

        [Header("Armamento FX")]
        public GameObject bulletPrefab; 
        public Transform firePoint;     
        
        private Rigidbody2D rb;
        private Animator animator;
        private SpriteRenderer spriteRenderer;

        private float horizontalInput;
        private bool isGrounded;
        private bool isCrouching;
        private bool isShooting;

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

            // Agacharse
            isCrouching = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
            animator.SetBool("isCrouching", isCrouching);

            // Disparo (Solo instanciamos en el frame que se presiona)
            if (Input.GetKeyDown(KeyCode.Z) || Input.GetButtonDown("Fire1"))
            {
                animator.SetBool("isShooting", true);
                DispararFX();
            }
            else if (Input.GetKeyUp(KeyCode.Z) || Input.GetButtonUp("Fire1"))
            {
                animator.SetBool("isShooting", false);
            }

            // Salto
            if (Input.GetButtonDown("Jump") && isGrounded && !isCrouching)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }

            // Evasión (Backjump)
            if (Input.GetKeyDown(KeyCode.C) && isGrounded)
            {
                animator.SetTrigger("backjump");
                ExecuteBackjump();
            }

            // Dirección Visual
            if (horizontalInput > 0) spriteRenderer.flipX = false;
            else if (horizontalInput < 0) spriteRenderer.flipX = true;

            // Sincronización del Animator (Parche para el deslizamiento)
            float velocidadVisual = isCrouching ? 0f : Mathf.Abs(horizontalInput);
            animator.SetFloat("velocityX", velocidadVisual);
            animator.SetBool("grounded", isGrounded);
        }

        void FixedUpdate()
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

            if (!isCrouching)
            {
                rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
            }
            else
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            }
        }

        private void ExecuteBackjump()
        {
            float direction = spriteRenderer.flipX ? 1f : -1f;
            rb.linearVelocity = new Vector2(direction * backjumpForceX, backjumpForceY);
        }

        private void DispararFX()
        {
            if(bulletPrefab != null && firePoint != null)
            {
                // Creamos la bala en la posición y rotación del FirePoint
                Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
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

        // Puente para evitar errores del TokenController viejo
        public void ResetDash() { }
    }
}