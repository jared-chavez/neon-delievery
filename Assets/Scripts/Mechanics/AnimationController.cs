using UnityEngine;

namespace Platformer.Mechanics
{
    [RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
    public class AnimationController : MonoBehaviour
    {
        [Header("Configuración de Animación")]
        public float maxSpeed = 7f; // Debe coincidir con la de PlayerController
        
        // Estas variables las llena el PlayerController cada frame
        [HideInInspector] public Vector2 move;
        [HideInInspector] public bool grounded;

        private SpriteRenderer spriteRenderer;
        private Animator animator;

        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            // 1. Sincronizamos el parámetro del Animator con la velocidad real de Jax
            // Dividimos entre maxSpeed para que el valor esté entre 0 y 1 (estándar de Blend Trees)
            animator.SetFloat("velocityX", Mathf.Abs(move.x) / maxSpeed);
            
            // 2. Sincronizamos el estado de "suelo" para las animaciones de salto
            animator.SetBool("grounded", grounded);

            // Nota: El 'Flip' del sprite ya lo maneja el PlayerController directamente
        }
    }
}