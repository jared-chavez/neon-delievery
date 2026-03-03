using UnityEngine;
using Platformer.Mechanics; // Necesario para interactuar con Health

namespace Platformer.Mechanics
{
    [RequireComponent(typeof(Collider2D))]
    public class EnemyController : MonoBehaviour
    {
        [Header("Navegación del Sector 1")]
        public PatrolPath path;
        public float speed = 2.5f;
        private int currentWaypointIndex = 0;

        [Header("Componentes")]
        internal AnimationController control;
        internal Collider2D _collider;
        SpriteRenderer spriteRenderer;

        void Awake()
        {
            control = GetComponent<AnimationController>();
            _collider = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            
            // IMPORTANTE: Para la luz roja del Enforcer, el colisionador debe ser Trigger
            _collider.isTrigger = true; 
        }

        void Update()
        {
            if (path != null && path.waypoints.Length > 0)
            {
                MoverHaciaSiguientePunto();
            }
        }

        private void MoverHaciaSiguientePunto()
        {
            // Obtenemos el punto de destino de la ruta de patrulla
            Transform target = path.waypoints[currentWaypointIndex];
            
            // Calculamos el movimiento fluido
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, target.position, step);

            // Verificamos si Jax debe voltear (Flip) según la dirección del dron
            float direction = target.position.x - transform.position.x;
            if (control != null) control.move.x = Mathf.Clamp(direction, -1, 1);

            // Si llegamos al punto, pasamos al siguiente
            if (Vector2.Distance(transform.position, target.position) < 0.1f)
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % path.waypoints.Length;
            }
        }

        // Cambio crítico: De OnCollision (físico) a OnTrigger (detección de luz)
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                var health = other.GetComponent<Health>();
                if (health != null && health.IsAlive)
                {
                    Debug.Log("ENFORCER: Intruso detectado. Activando protocolo de eliminación.");
                    health.Die(); // Activa el cambio de color a rojo en la chaqueta de Jax
                }
            }
        }
    }
}