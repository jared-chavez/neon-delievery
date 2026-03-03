using UnityEngine;
using UnityEngine.UI;

namespace NeonDelivery.Core
{
    public class LevelManager : MonoBehaviour
    {
        [Header("Referencias de UI de Victoria")]
        [SerializeField] private GameObject victoryPanel;
        [SerializeField] private Text finalTimeText;
        [SerializeField] private Text dataPackagesText;

        private float startTime;

        private void Start()
        {
            startTime = Time.time;
            victoryPanel.SetActive(false);
        }

        public void CompleteLevel()
        {
            Debug.Log("Nivel completado: Jax ha entregado el paquete.");

            GameManager.Instance.UpdateGameState(GameState.Dialogue);

            float t = Time.time - startTime;
            string minutes = ((int) t / 60).ToString("00");
            string seconds = (t % 60).ToString("00");

            victoryPanel.SetActive(true);

            if (finalTimeText != null)
                finalTimeText.text = $"Tiempo Final: {minutes}:{seconds}";
            
            // dataPackagesText.text = "Paquetes: " + PlayerStats.DataCount;
        }

        public void RestartGame()
        {
            // Método para el botón de "Reintentar" en el panel
            GameManager.Instance.RestartLevel();
        }
    }
}