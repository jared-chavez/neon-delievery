using UnityEngine;
using Platformer.Mechanics;
using Platformer.Model;

namespace Platformer.Gameplay
{
    public class PlayerController : KinematicObject
    {
        [Header("Configuración de Jax")]
        public float maxSpeed = 7f;
        public float jumpTakeOffSpeed = 7f;
        public float dashSpeed = 20f;

        [Header("Referencias Vitales")]
        public Health health; // CORRECCIÓN: Variable faltante para el error CS1061
        public PlatformerModel model;
        private Animator animatorComponent;
        private SpriteRenderer spriteRenderer;

        private Vector2 move;
        private bool jump;
        private bool isCrouching = false;
        private bool isShooting = false;
        private bool canDash = true;

        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            animatorComponent = GetComponent<Animator>();
            if (health == null) health = GetComponent<Health>(); // Asegura que Jax tenga vida
        }

        protected override void Update()
        {
            if (model != null && model.player != null && health.IsAlive)
            {
                move.x = Input.GetAxis("Horizontal");

                if (Input.GetButtonDown("Jump") && IsGrounded) jump = true;

                isCrouching = Input.GetAxis("Vertical") < -0.1f && IsGrounded;
                isShooting = Input.GetButton("Fire1") || Input.GetKey(KeyCode.K);

                if (Input.GetKeyDown(KeyCode.LeftShift) && canDash) StartCoroutine(PerformDash());
            }
            else
            {
                move.x = 0;
            }

            UpdateAnimatorParameters();
            base.Update();
        }

        private void UpdateAnimatorParameters()
        {
            if (animatorComponent == null) return;

            animatorComponent.SetFloat("velocityX", Mathf.Abs(velocity.x));
            animatorComponent.SetBool("grounded", IsGrounded);
            animatorComponent.SetBool("isCrouching", isCrouching);
            animatorComponent.SetBool("isShooting", isShooting);
        }

        protected override void ComputeVelocity()
        {
            if (jump && IsGrounded)
            {
                velocity.y = jumpTakeOffSpeed;
                jump = false;
            }

            targetVelocity = isCrouching ? Vector2.zero : move * maxSpeed;

            if (move.x > 0.01f) spriteRenderer.flipX = false;
            else if (move.x < -0.01f) spriteRenderer.flipX = true;
        }

        // CORRECCIÓN: Método necesario para TokenController.cs
        public void ResetDash() 
        {
            canDash = true;
        }

        private System.Collections.IEnumerator PerformDash()
        {
            canDash = false;
            if (animatorComponent != null) animatorComponent.SetTrigger("dash");
            
            float direction = spriteRenderer.flipX ? -1 : 1;
            velocity.x = direction * dashSpeed;
            
            yield return new WaitForSeconds(0.2f);
            yield return new WaitForSeconds(1f); // Cooldown
            canDash = true;
        }
    }
}