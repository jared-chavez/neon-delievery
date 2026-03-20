using UnityEngine;

public class BatteryPickup : MonoBehaviour
{
    [Header("Configuración del Item")]
    public int scoreBonus = 50; // Puntos extra

    [Header("Audio")]
    public AudioClip pickupSound; // El sonido de agarrar la batería

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // Si lo que tocó la batería tiene la etiqueta "Player" (Jax)
        if (hitInfo.CompareTag("Player"))
        {
            // Verificamos si Jax realmente necesita salud (menor que el máximo de 5)
            if (GameManager.Instance.lives < 5)
            {
                // 1. REPRODUCIMOS EL SONIDO (Solo si necesita vida)
                if (GameManager.Instance != null && GameManager.Instance.uiAudioSource != null && pickupSound != null)
                {
                    GameManager.Instance.uiAudioSource.PlayOneShot(pickupSound);
                }

                // 2. Le decimos al GameManager que nos dé una vida
                GameManager.Instance.AddLife();
                
                // 3. Sumamos puntos
                GameManager.Instance.AddScore(scoreBonus);

                // 4. ¡La batería hace su trabajo y desaparece!
                Destroy(gameObject);
            }
            else
            {
                // Si Jax está lleno, no hacemos nada y la batería se queda ahí flotando intacta (SIN SONIDO).
                Debug.Log("Jax tiene la batería llena, el item permanece silencioso.");
            }
        }
    }
}