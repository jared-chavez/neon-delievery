using UnityEngine;

public class FXCleanup : MonoBehaviour
{
    [Tooltip("Tiempo en segundos antes de que el efecto desaparezca")]
    public float destroyDelay = 0.5f; // Basado en tus 6 frames, 0.5s es un buen estimado

    void Start()
    {
        // Al nacer, el objeto ya sabe que tiene fecha de caducidad.
        // Esto limpia la escena automáticamente.
        Destroy(gameObject, destroyDelay);
    }
}