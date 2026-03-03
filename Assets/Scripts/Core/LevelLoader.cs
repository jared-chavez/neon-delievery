using UnityEngine;
using UnityEngine.SceneManagement;

namespace NeonDelivery.Core
{
    public enum GameState { Starting, Playing, Dialogue, Paused, GameOver }

    public class GameManager : MonoBehaviour
    {
        // Patrón Singleton para acceso global desde cualquier script
        public static GameManager Instance { get; private set; }

        public GameState State { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this) 
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            UpdateGameState(GameState.Starting);
        }

        public void UpdateGameState(GameState newState)
        {
            State = newState;

            switch (newState)
            {
                case GameState.Starting:
                    HandleStarting();
                    break;
                case GameState.Dialogue:
                    Time.timeScale = 0f; 
                    break;
                case GameState.Playing:
                    Time.timeScale = 1f;
                    break;
                case GameState.GameOver:
                    RestartLevel();
                    break;
            }
        }

        private void HandleStarting()
        {
            Debug.Log("Iniciando Sector 1...");
            UpdateGameState(GameState.Playing);
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}