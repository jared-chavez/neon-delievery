using UnityEngine;

namespace NeonDelivery.View
{
    public class SimpleRainEffect : MonoBehaviour
    {
        [Header("Configuración de Lluvia")]
        public float verticalSpeed = 5.0f;
        public float horizontalWind = -1.0f; // Inclinación suave
        
        private Vector3 startPosition;
        private float textureHeight;

        void Start()
        {
            startPosition = transform.position;
            // Obtenemos el tamaño del sprite para el bucle infinito
            textureHeight = GetComponent<SpriteRenderer>().bounds.size.y;
        }

        void Update()
        {
            // Movimiento constante hacia abajo y un poco hacia la izquierda
            float newY = Mathf.Repeat(Time.time * verticalSpeed, textureHeight);
            float newX = Time.time * horizontalWind;

            transform.position = startPosition + new Vector3(newX % 1f, -newY, 0);
        }
    }
}