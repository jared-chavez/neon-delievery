using UnityEngine;

public class NeonTrafficBionico : MonoBehaviour
{
    [Header("Configuración de Vuelo")]
    public float speed = 5f;      
    public bool moveLeft = true;   

    [Header("Límites del Sector 1")]
    public float leftLimit = -25f; 
    public float rightLimit = 470f; 

    void Update()
    {
        // 1. Calcular dirección y movimiento
        float direction = moveLeft ? -1f : 1f;
        transform.Translate(Vector3.right * direction * speed * Time.deltaTime);

        // 2. Lógica de Bucle Infinito (Loop)
        if (moveLeft && transform.position.x < leftLimit)
        {
            // Si va a la izquierda y sale del límite, aparece en la derecha
            transform.position = new Vector3(rightLimit, transform.position.y, transform.position.z);
        }
        else if (!moveLeft && transform.position.x > rightLimit)
        {
            // Si va a la derecha y sale del límite, aparece en la izquierda
            transform.position = new Vector3(leftLimit, transform.position.y, transform.position.z);
        }
    }
}