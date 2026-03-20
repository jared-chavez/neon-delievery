using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    // Tiempo que tarda en destruirse (ajústalo a la duración de tu animación)
    public float delay = 0.5f; 

    void Start()
    {
        // Destruye este objeto después de 'delay' segundos
        Destroy(gameObject, delay);
    }
}