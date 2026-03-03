using UnityEngine;
using NeonDelivery.Core;
using Platformer.Gameplay;

namespace Platformer.Mechanics
{
    /// <summary>
    /// Marca el disparador del Elevador de Carga al final del nivel.
    /// Activa la ceremonia de victoria y el resumen de la entrega.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class VictoryZone : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D collider)
        {
            // Verificamos si es Jax quien entra al elevador
            var player = collider.gameObject.GetComponent<PlayerController>();
            
            if (player != null)
            {
                // Notificamos al sistema central que la misión ha terminado
                Debug.Log("SISTEMA: Jax ha alcanzado el punto de extracción.");
                
                // Buscamos el LevelManager para procesar los resultados finales
                var levelManager = Object.FindAnyObjectByType<LevelManager>();
                
                if (levelManager != null)
                {
                    levelManager.CompleteLevel();
                }
            }
        }
    }
}