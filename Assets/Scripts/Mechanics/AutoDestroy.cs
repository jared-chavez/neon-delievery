using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [Tooltip("Tiempo en segundos antes de que el objeto desaparezca")]
    public float delay = 0.5f; // Medio segundo es ideal para animaciones FX rápidas

    void Start()
    {
        // Inicia la cuenta regresiva de destrucción en cuanto el objeto nace
        Destroy(gameObject, delay);
    }
}