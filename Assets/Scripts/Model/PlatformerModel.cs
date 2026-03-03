using UnityEngine;
using Platformer.Mechanics;
using Platformer.Gameplay;

namespace Platformer.Model
{
    [System.Serializable]
    public class PlatformerModel
    {
        [Header("Referencias de Escena")]
        public Unity.Cinemachine.CinemachineCamera virtualCamera;
        public PlayerController player;
        public Transform spawnPoint;

        [Header("Parámetros de Movimiento (Ajuste de Precisión)")]
        public float jumpModifier = 1.5f;
        public float jumpDeceleration = 0.5f;

        [Header("Estado de la Misión (Neon Delivery)")]
        // Datos específicos para el equipo Byteados
        
        /// <summary>
        /// Cantidad de paquetes de datos recuperados en el nivel.
        /// </summary>
        public int dataPackagesCollected = 0;

        /// <summary>
        /// Indica si Jax tiene el paquete principal para la entrega final.
        /// </summary>
        public bool hasDeliveryPackage = true; 

        /// <summary>
        /// Tiempo transcurrido desde el inicio del Sector .
        /// </summary>
        public float levelTimer = 0f;
    }
}