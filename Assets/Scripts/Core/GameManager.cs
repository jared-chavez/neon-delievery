using System.Collections;
using UnityEngine;
using TMPro; 
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum GameState { Playing, Paused, GameOver, LevelComplete }
    public GameState currentState = GameState.Playing;

    [Header("Jax Stats")]
    public int lives = 1; 
    public int score = 0; 
    public int power = 5; 
    public int combo = 0; // Sistema de combo integrado

    [Header("Power Recharge")]
    public float powerRechargeTime = 2f; 
    private float powerTimer = 0f;

    [Header("Jax Teleport")]
    public Transform jaxPlayer; 
    private Vector3 startPosition; 

    [Header("HUD Elements")]
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText; // UI del combo
    public Image healthBarImage; 
    public Sprite[] healthSprites; 
    public Image powerBarImage; 
    public Sprite[] powerSprites; 

    [Header("Screens & Overlays")]
    public GameObject darkOverlay; 
    public GameObject pauseScreen; 
    public GameObject gameOverScreen;
    public GameObject victoryScreen;

    [Header("Menu Arrays")]
    public TextMeshProUGUI[] pauseOptions;     
    public TextMeshProUGUI[] gameOverOptions;  
    public TextMeshProUGUI[] victoryOptions;   

    [Header("Audio")]
    public AudioSource uiAudioSource;   
    public AudioClip navigateSound;     
    public AudioClip confirmSound;      

    private int selectedIndex = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateUI();
        if (jaxPlayer != null) startPosition = jaxPlayer.position;
        ResumeGame(); 
    }

    void Update()
    {
        if (currentState == GameState.Playing)
        {
            if (Input.GetKeyDown(KeyCode.Escape)) TriggerPause();

            // Regeneración de energía
            if (power < 5)
            {
                powerTimer += Time.deltaTime;
                if (powerTimer >= powerRechargeTime)
                {
                    power++;
                    UpdateUI();
                    powerTimer = 0f; 
                }
            }
        }
        else if (currentState == GameState.Paused)
        {
            if (Input.GetKeyDown(KeyCode.Escape)) ResumeGame(); 
            else HandleMenuInput();
        }
        else
        {
            HandleMenuInput();
        }
    }

    private void HandleMenuInput()
    {
        TextMeshProUGUI[] currentOptions = GetCurrentOptionsArray();
        if (currentOptions == null || currentOptions.Length == 0) return;

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            selectedIndex--;
            if (selectedIndex < 0) selectedIndex = currentOptions.Length - 1;
            UpdateMenuVisuals(currentOptions);
            PlayUISound(navigateSound);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            selectedIndex++;
            if (selectedIndex >= currentOptions.Length) selectedIndex = 0;
            UpdateMenuVisuals(currentOptions);
            PlayUISound(navigateSound);
        }
        else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Z))
        {
            PlayUISound(confirmSound);
            ExecuteSelection();
        }
    }

    private void PlayUISound(AudioClip clip)
    {
        if (uiAudioSource != null && clip != null)
        {
            uiAudioSource.PlayOneShot(clip); 
        }
    }

    private void UpdateMenuVisuals(TextMeshProUGUI[] options)
    {
        for (int i = 0; i < options.Length; i++)
        {
            string cleanText = options[i].text.Replace("<u>", "").Replace("</u>", "");
            if (i == selectedIndex) options[i].text = "<u>" + cleanText + "</u>";
            else options[i].text = cleanText;
        }
    }

    private void ExecuteSelection()
    {
        if (currentState == GameState.Paused)
        {
            if (selectedIndex == 0) ResumeGame();
            else if (selectedIndex == 1) ReturnToStart();
            else if (selectedIndex == 2) QuitApp();
        }
        else if (currentState == GameState.GameOver)
        {
            if (selectedIndex == 0) RestartLevel();
            else if (selectedIndex == 1) QuitApp();
        }
        else if (currentState == GameState.LevelComplete)
        {
            if (selectedIndex == 0) ReturnToStart();
            else if (selectedIndex == 1) QuitApp();
        }
    }

    private TextMeshProUGUI[] GetCurrentOptionsArray()
    {
        if (currentState == GameState.Paused) return pauseOptions;
        if (currentState == GameState.GameOver) return gameOverOptions;
        if (currentState == GameState.LevelComplete) return victoryOptions;
        return null;
    }

    private void TriggerPause()
    {
        currentState = GameState.Paused;
        selectedIndex = 0;
        if (pauseScreen != null) pauseScreen.SetActive(true);
        UpdateMenuVisuals(pauseOptions);
        SetJaxControl(false);
        Time.timeScale = 0f;
    }

    // --- FUNCIONES PÚBLICAS (NECESARIAS PARA LOS BOTONES UI) ---
    public void ResumeGame()
    {
        currentState = GameState.Playing;
        if (pauseScreen != null) pauseScreen.SetActive(false);
        if (darkOverlay != null) darkOverlay.SetActive(false);
        if (gameOverScreen != null) gameOverScreen.SetActive(false);
        if (victoryScreen != null) victoryScreen.SetActive(false);
        SetJaxControl(true);
        Time.timeScale = 1f;
    }

    public void RestartLevel() 
    { 
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

    public void ReturnToStart() 
    { 
        Time.timeScale = 1f; 
        SceneManager.LoadScene("MainMenu"); 
    }

    public void QuitApp() 
    { 
        Debug.Log("Saliendo del juego..."); 
        Application.Quit(); 
    }
    // ----------------------------------------------------------

    public void LevelComplete()
    {
        currentState = GameState.LevelComplete;
        selectedIndex = 0;
        if (darkOverlay != null) darkOverlay.SetActive(true);
        if (victoryScreen != null) victoryScreen.SetActive(true);
        UpdateMenuVisuals(victoryOptions);
        SetJaxControl(false); 
        Time.timeScale = 0f;
    }

    public void LoseLife()
    {
        if (currentState != GameState.Playing) return;
        
        combo = 0; // Rompemos el combo al recibir daño
        lives--;
        if (lives < 0) lives = 0; 
        UpdateUI();

        if (jaxPlayer != null)
        {
            Animator jaxAnim = jaxPlayer.GetComponent<Animator>();
            // Usamos el nombre exacto de tu Trigger en Unity
            if (jaxAnim != null) jaxAnim.SetTrigger("Jax_Hurt"); 
        }

        if (lives <= 0) 
        {
            SetJaxControl(false); // Apagamos control al morir
            StartCoroutine(GameOverSequence());
        }
    }

    public void InstantDeath()
    {
        if (currentState != GameState.Playing) return;
        
        lives = 0; 
        UpdateUI(); 

        // APAGADOR PRINCIPAL: Detenemos el PlayerController para que
        // la animación no sea cancelada por el movimiento o el piso.
        SetJaxControl(false); 

        if (jaxPlayer != null)
        {
            Rigidbody2D rb = jaxPlayer.GetComponent<Rigidbody2D>();
            Animator jaxAnim = jaxPlayer.GetComponent<Animator>();

            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero; 
                rb.bodyType = RigidbodyType2D.Kinematic; 
            }

            if (jaxAnim != null)
            {
                // Forzamos directamente el estado gris que tienes en el Animator
                jaxAnim.Play("Jax_Hurt"); 
            }
        }

        StartCoroutine(GameOverSequence());
    }

    public void AddLife()
    {
        if (currentState != GameState.Playing) return;
        
        if (lives < 5) 
        {
            lives++;
            UpdateUI(); 
        }
    }

    private IEnumerator GameOverSequence()
    {
        currentState = GameState.GameOver;
        selectedIndex = 0;

        SetJaxControl(false);
        if (jaxPlayer != null)
        {
            Rigidbody2D rb = jaxPlayer.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero; 
                rb.bodyType = RigidbodyType2D.Kinematic; 
            }
        }

        yield return new WaitForSeconds(1f);

        if (darkOverlay != null) darkOverlay.SetActive(true);
        if (gameOverScreen != null) gameOverScreen.SetActive(true);
        UpdateMenuVisuals(gameOverOptions);
        Time.timeScale = 0f;
    }

    private void SetJaxControl(bool state)
    {
        if (jaxPlayer != null)
        {
            var controller = jaxPlayer.GetComponent<Platformer.Mechanics.PlayerController>();
            if (controller != null) controller.enabled = state;
        }
    }

    public void AddScore(int amount) 
    { 
        // Lógica de multiplicador de combo
        combo++; 
        int finalScore = amount * combo; 
        score += finalScore; 
        UpdateUI(); 
    }
    
    public void ConsumePower() 
    { 
        if (power > 0) 
        { 
            power--; 
            powerTimer = 0f; 
            UpdateUI(); 
        } 
    }
    
    public void RechargeStats() { lives = 1; power = 5; UpdateUI(); }

    public void UpdateUI()
    {
        if (livesText != null) livesText.text = lives.ToString();
        if (scoreText != null) scoreText.text = score.ToString();
        if (comboText != null) comboText.text = combo.ToString(); // Actualiza visualmente el combo
        
        if (healthBarImage != null && healthSprites != null && healthSprites.Length > 0)
        {
            int lifeIndex = Mathf.Clamp(lives, 0, healthSprites.Length - 1);
            healthBarImage.sprite = healthSprites[lifeIndex];
        }
        
        if (powerBarImage != null && powerSprites != null && powerSprites.Length > 0)
        {
            int powerIndex = Mathf.Clamp(power, 0, powerSprites.Length - 1);
            powerBarImage.sprite = powerSprites[powerIndex];
        }
    }

    public void UpdateGameState() { }
    public void UpdateGameState<T>(T newState) { }
}