using UnityEngine;

namespace Platformer.Mechanics
{
    public class FlyingObject : MonoBehaviour
    {
        [Header("Configuración de Vuelo")]
        public float speed = 2.0f;        
        public bool moveLeft = true;     

        [Header("Límites del Mundo")]
        public float leftBound = -25f;    
        public float rightBound = 25f;    

        void Update()
        {
            // Movimiento constante
            float direction = moveLeft ? -1f : 1f;
            transform.Translate(Vector3.right * direction * speed * Time.deltaTime);

            // Lógica de Teletransporte (Loop Infinito)
            if (moveLeft && transform.position.x < leftBound)
            {
                Vector3 newPos = transform.position;
                newPos.x = rightBound;
                transform.position = newPos;
            }
            else if (!moveLeft && transform.position.x > rightBound)
            {
                Vector3 newPos = transform.position;
                newPos.x = leftBound;
                transform.position = newPos;
            }
        }
    }
}