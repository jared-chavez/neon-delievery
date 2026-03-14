using UnityEngine;
public class NeonBullet : MonoBehaviour
{
    public float speed = 15f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * speed;
        
        // Se autodestruye a los 2 segundos para no consumir memoria de tu Mac
        Destroy(gameObject, 2f); 
    }

    // Esto se activará más adelante cuando toquemos a un Dron enemigo
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // Destroy(gameObject); 
    }
}