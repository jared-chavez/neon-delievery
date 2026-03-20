using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Comportamiento IA")]
    public float speed = 3f;
    public float patrolDistance = 5f;

    [Header("Efectos")]
    public GameObject explosionPrefab; // Aquí pondremos el FX_DroneExplosion

    private float leftEdge;
    private float rightEdge;
    private bool movingLeft = true;

    void Start()
    {
        // Calculamos los límites basados en dónde pusiste al dron en la escena
        leftEdge = transform.position.x - patrolDistance;
        rightEdge = transform.position.x + patrolDistance;
    }

    void Update()
    {
        // Movimiento de patrullaje
        if (movingLeft)
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
            if (transform.position.x <= leftEdge) Voltear();
        }
        else
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            if (transform.position.x >= rightEdge) Voltear();
        }
    }

    void Voltear()
    {
        movingLeft = !movingLeft;
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
    }

    // Colisiones
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // 1. Si choca contra Jax
        if (hitInfo.CompareTag("Player"))
        {
            GameManager.Instance.LoseLife();
        }
        // 2. Si choca contra un disparo de Jax
        // ¡OJO! Asegúrate de que el Prefab de tus balas tenga el Tag "Bullet"
        else if (hitInfo.CompareTag("Bullet")) 
        {
            // Destruimos la bala para que no traspase al dron
            Destroy(hitInfo.gameObject); 
            
            // Destruimos al dron
            Explotar();
        }
    }

    public void Explotar()
    {
        // 1. Creamos la explosión visual en el lugar exacto del dron
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        // 2. Le damos puntos al jugador (100 puntos por dron)
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(100);
        }

        // 3. Eliminamos el dron de la escena
        Destroy(gameObject);
    }
}