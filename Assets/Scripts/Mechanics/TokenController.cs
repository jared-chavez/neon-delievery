using UnityEngine;
using Platformer.Gameplay;

namespace Platformer.Mechanics
{
    public class TokenController : MonoBehaviour
    {
        // Patrón Singleton para acceso global desde TokenInstance
        public static TokenController Instance { get; private set; }

        [Header("Estadísticas de la Misión")]
        public int collectedDataPackages = 0;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;
        }

        // Este es el método que recibe la "señal" del TokenInstance
        public void OnTokenCollected(TokenInstance token, PlayerController player)
        {
            switch (token.type)
            {
                case TokenInstance.TokenType.IonBattery:
                    HandleIonBattery(player);
                    break;

                case TokenInstance.TokenType.DataPackage:
                    HandleDataPackage(token);
                    break;
            }
        }

        private void HandleIonBattery(PlayerController player)
        {
            // Lógica de Impulso: Reseteamos el Dash de Jax al instante
            Debug.Log("ENERGÍA RESTAURADA: Dash disponible.");
            player.ResetDash(); 
        }

        private void HandleDataPackage(TokenInstance token)
        {
            // Lógica de Progresión: Sumamos al contador del nivel
            collectedDataPackages++;
            Debug.Log($"PAQUETE ASEGURADO: {collectedDataPackages} recuperados.");
        }
    }
}