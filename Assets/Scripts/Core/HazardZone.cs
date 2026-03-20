using UnityEngine;
using UnityEngine.SceneManagement;

public class HazardAcid : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip acidSound; 

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // Verificamos si Jax (etiqueta 'Player') tocó el ácido
        if (hitInfo.CompareTag("Player"))
        {
            // Debug.Log("Jax tocó el ácido. Muerte Instantánea!"); // Úsalo para testear
            if (GameManager.Instance != null && GameManager.Instance.uiAudioSource != null && acidSound != null) {
                GameManager.Instance.uiAudioSource.PlayOneShot(acidSound);
            }

            // Llamamos a la función crítica del GameManager
            if (GameManager.Instance != null)
            {
                GameManager.Instance.InstantDeath();
            }

            // Opcional: Detenemos el movimiento físico de Jax al instante
            // para que no parezca que sigue corriendo dentro del ácido.
            Rigidbody2D rb = hitInfo.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero; // Detiene el impulso actual
            }
        }
    }
}