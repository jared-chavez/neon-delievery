using UnityEngine;

namespace NeonDelivery.View
{
    public class ParallaxEffect : MonoBehaviour
    {
        private float length, startPos;
        public GameObject cam;
        
        [Header("Configuración de Profundidad")]
        [Tooltip("0 = Sigue a la cámara, 1 = Estático. Usa valores entre 0.1 y 0.9.")]
        public float parallaxFactor;

        void Start()
        {
            // Guardamos la posición inicial y el tamaño del sprite para el bucle infinito
            startPos = transform.position.x;
            length = GetComponent<SpriteRenderer>().bounds.size.x;
        }

        void LateUpdate()
        {
            // Calculamos cuánto se ha movido el fondo respecto a la cámara
            float temp = (cam.transform.position.x * (1 - parallaxFactor));
            float dist = (cam.transform.position.x * parallaxFactor);

            // Aplicamos el movimiento
            transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);

            // Lógica de "Mar de concreto sin fin": Si el fondo se sale de cámara, se reposiciona
            if (temp > startPos + length) startPos += length;
            else if (temp < startPos - length) startPos -= length;
        }
    }
}