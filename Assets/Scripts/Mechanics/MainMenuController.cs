using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("Background Effect")]
    public RawImage backgroundImage;
    public float scrollSpeed = 0.05f;

    [Header("Start Animation (Neon Fade)")]
    public Image startButtonImage; 
    
    [Tooltip("Velocidad de oscilación suave del parpadeo (Lento es mejor)")]
    public float fadeSpeed = 2f; 

    [Tooltip("Duración del parpadeo rápido antes de cambiar de escena")]
    public float confirmBlinkDuration = 1f;

    [Tooltip("Velocidad del parpadeo rápido de confirmación")]
    public float confirmBlinkSpeed = 0.1f;

    [Header("Audio")]
    [Tooltip("El componente que reproducirá el sonido (puede ser un objeto vacío con AudioSource)")]
    public AudioSource uiAudioSource;
    [Tooltip("El archivo de audio que sonará al presionar START")]
    public AudioClip startSound;

    private bool isStarting = false;
    private Coroutine currentFadeCoroutine;

    void Start()
    {
        // Al iniciar la pantalla, empezamos el parpadeo suave e infinito
        if (startButtonImage != null)
        {
            currentFadeCoroutine = StartCoroutine(NeonFadeIdle());
        }
    }

    void Update()
    {
        // Movimiento infinito del fondo
        if (backgroundImage != null)
        {
            Rect uvRect = backgroundImage.uvRect;
            uvRect.x += scrollSpeed * Time.deltaTime;
            backgroundImage.uvRect = uvRect;
        }

        // DETECCIÓN DE TECLADO
        if (!isStarting && Input.anyKeyDown)
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        if (isStarting) return;
        isStarting = true;

        // --- REPRODUCIMOS EL SONIDO AL INSTANTE ---
        if (uiAudioSource != null && startSound != null)
        {
            uiAudioSource.PlayOneShot(startSound);
        }

        // Detenemos el parpadeo suave
        if (currentFadeCoroutine != null) StopCoroutine(currentFadeCoroutine); 
        
        // Arrancamos la transición cinematográfica rápida
        StartCoroutine(ActiveBlinkAndFadeOut());
    }

    // --- CORRUTINA 1: Parpadeo Suave (Idle) ---
    IEnumerator NeonFadeIdle()
    {
        while (true) // Bucle infinito
        {
            // Usamos Mathf.Sin para crear una oscilación suave entre 0 y 1 basada en el tiempo
            float alpha = (Mathf.Sin(Time.time * fadeSpeed) + 1f) / 2f; 
            
            // Aplicamos el nuevo alpha a la imagen
            SetButtonAlpha(alpha);
            
            yield return null; // Esperamos al siguiente fotograma
        }
    }

    // --- CORRUTINA 2: Parpadeo Rápido y Desvanecimiento Final (Confirm) ---
    IEnumerator ActiveBlinkAndFadeOut()
    {
        if (startButtonImage == null) yield break;

        float timer = 0f;
        
        // 1. Parpadeo rápido (estilo arcade) para confirmar
        while (timer < confirmBlinkDuration)
        {
            startButtonImage.enabled = !startButtonImage.enabled;
            yield return new WaitForSeconds(confirmBlinkSpeed);
            timer += confirmBlinkSpeed * 2f; // Multiplicamos por 2 porque hay dos esperas por ciclo
        }
        
        // Aseguramos que empiece el desvanecimiento estando visible
        startButtonImage.enabled = true; 

        // 2. Desvanecimiento suave y lento hasta desaparecer (alpha = 0)
        float currentAlpha = startButtonImage.color.a;
        while (currentAlpha > 0.01f) // Hasta que sea casi invisible
        {
            // Movemos el alpha actual hacia cero de forma constante
            currentAlpha = Mathf.MoveTowards(currentAlpha, 0f, fadeSpeed * Time.deltaTime);
            SetButtonAlpha(currentAlpha);
            yield return null; // Esperamos al siguiente fotograma
        }

        // Aseguramos que sea completamente transparente al final
        SetButtonAlpha(0f); 

        // ¡Al Sector 1!
        SceneManager.LoadScene("Sector1");
    }

    // Función auxiliar para cambiar solo el alpha de la imagen
    void SetButtonAlpha(float alpha)
    {
        if (startButtonImage == null) return;
        Color color = startButtonImage.color;
        color.a = alpha; // Modificamos el valor alpha (transparencia)
        startButtonImage.color = color;
    }
}