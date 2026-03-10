using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Gameplay;
using Platformer.Mechanics;
using Platformer.Model;

namespace Platformer.Gameplay
{
    public class PlayerController : KinematicObject
    {
        [Header("Configuración de Jax (Byteados)")]
        public float maxSpeed = 7;
        public float jumpTakeOffSpeed = 7;
        public float dashSpeed = 20; 

        [Header("Arsenal de Sonido (SFX)")]
        public AudioSource shootSound;
        public AudioSource movementSound;

        [Header("Model & Detección")]
        public Animator animator;
        public SpriteRenderer spriteRenderer;
        public GameObject bulletPrefab; 
        public Transform firePoint;

        private bool isClimbing; 
        private float vMove;

        // Corrección CS0115: Quitamos override porque KinematicObject no tiene Awake virtual
        void Awake() 
        {
            // Inicialización de componentes si no se arrastran en el inspector
        }

        protected override void ComputeVelocity()
        {
            float move = Input.GetAxis("Horizontal");
            vMove = Input.GetAxisRaw("Vertical");

            // 1. Detección de Escalera (Capa Climbable)
            bool nearLadder = Physics2D.OverlapCircle(transform.position, 0.2f, LayerMask.GetMask("Climbable"));

            if (nearLadder && Mathf.Abs(vMove) > 0.1f)
            {
                isClimbing = true;
            }
            else if (!nearLadder)
            {
                isClimbing = false;
            }

            // 2. Lógica de Escalado con Pausa de Animación
            if (isClimbing)
            {
                velocity = new Vector2(0, vMove * maxSpeed); 
                
                // TRUCO: Si no hay movimiento, forzamos la velocidad a CERO absoluto
                if (vMove == 0) {
                    velocity.y = 0;
                    // Si tu KinematicObject tiene gravityModifier, lo ignoramos aquí
                }

                animator.speed = (vMove == 0) ? 0 : 1; 
                animator.SetBool("isClimbing", true);
                animator.SetBool("grounded", true); 
            }
            else
            {
                // Restauramos velocidad de animación normal
                animator.speed = 1; 
                animator.SetBool("isClimbing", false);
                animator.SetBool("grounded", IsGrounded); // Nota: 'I' mayúscula según tu KinematicObject

                // Control Horizontal
                if (move > 0.01f) spriteRenderer.flipX = false;
                else if (move < -0.01f) spriteRenderer.flipX = true;

                // Salto y Dash
                if (Input.GetButtonDown("Jump") && IsGrounded)
                {
                    velocity.y = jumpTakeOffSpeed;
                    if (movementSound != null) movementSound.Play();
                }

                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    animator.SetTrigger("dash");
                    velocity.x *= dashSpeed;
                }
            }

            // 3. Sistema de Disparo
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
                if (shootSound != null) shootSound.Play();
            }

            animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
            targetVelocity = new Vector2(move * maxSpeed, targetVelocity.y);
        }

        void Shoot()
        {
            if (bulletPrefab != null && firePoint != null)
                Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }

        public void ResetDash() { } // Compatibilidad con Tokens
    }
}