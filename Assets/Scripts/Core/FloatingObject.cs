using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    [Header("Ajustes de Flotado")]
    public float amplitude = 0.2f; // Qué tan arriba y abajo se mueve
    public float frequency = 2f;    // Qué tan rápido se mueve

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Calculamos la nueva posición Y usando una onda Seno
        float newY = startPosition.y + Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}