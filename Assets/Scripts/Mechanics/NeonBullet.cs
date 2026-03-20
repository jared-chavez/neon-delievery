using UnityEngine;

public class NeonBullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float speed = 20f;
    public Rigidbody2D rb;
    public GameObject explosionPrefab;
    
    [Tooltip("Tiempo máximo de vida en segundos antes de autodestruirse si no choca con nada")]
    public float lifeTime = 2f; 

    void Start()
    {
        // 1. LA REGLA DE ORO DE OPTIMIZACIÓN: 
        // Si la bala no choca con nada en 2 segundos, se destruye sola.
        // Esto previene las fugas de memoria por balas infinitas.
        Destroy(gameObject, lifeTime);

        // 2. Le da impulso a la bala en la dirección a la que mira el FirePoint
        if (rb != null)
        {
            rb.linearVelocity = transform.right * speed;
        }
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // SEGURO DE VIDA: Ignorar el colisionador de Jax
        // También ignoramos otros "Triggers" (como las zonas de ácido o el área de visión del dron)
        if (hitInfo.CompareTag("Player") || hitInfo.isTrigger) 
        {
            return; 
        }

        // Destruir al dron si choca con uno (Aquí podrías sumar puntos al combo en el futuro)
        if (hitInfo.CompareTag("Enemy"))
        {
            Destroy(hitInfo.gameObject); 
        }

        // Invocar el FX de explosión en el punto de impacto
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, transform.rotation);
        }
        
        // Destruir la bala al chocar con cualquier cosa sólida (pared, suelo, enemigo)
        Destroy(gameObject);
    }
}