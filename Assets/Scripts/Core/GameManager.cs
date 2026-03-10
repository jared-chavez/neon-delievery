using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; 

public class GameManager : MonoBehaviour
{
    // Singleton: Permite que otros scripts (como Jax o las baterías) hablen con este script fácilmente
    public static GameManager Instance;
    [Header("Estadísticas de la Misión")]
    public int score = 0;
    public int lives = 3;

    [Header("Conexión con la Interfaz")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;

    void Awake()
    {
        // Configuración de la instancia única con 'I' mayúscula
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateUI(); // Actualiza los textos al inicio
    }

    // Función para sumar energía cuando Jax recoge una batería
    public void AddScore(int points)
    {
        score += points;
        UpdateUI();
    }

    // Función para restar vidas si Jax toca ácido o un dron
    public void LoseLife()
    {
        lives--;
        UpdateUI();

        if (lives <= 0)
        {
            GameOver();
        }
    }

    // Actualiza los elementos visuales de la pantalla
    private void UpdateUI()
    {
        scoreText.text = "ENERGÍA: " + score;
        livesText.text = "VIDAS: " + lives;
    }

    // Lógica temporal para cuando se acaban las vidas
    private void GameOver()
    {
        Debug.Log("¡SISTEMA CRÍTICO! Pantalla de Game Over requerida.");
        // Por ahora, reiniciaremos la escena para probar
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void UpdateGameState(object estadoRecibido)    {
        // La dejamos vacía temporalmente. 
        // Esto le dice a Unity: "Sí, la función existe, puedes seguir compilando".
    }
}