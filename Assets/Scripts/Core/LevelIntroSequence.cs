using System.Collections;
using UnityEngine;

// Este script debe estar en un objeto vacío en la escena.
public class LevelIntroSequence : MonoBehaviour
{
    [Header("Referencias de la Escena")]
    public GameObject jaxPlayer; // Arrastra a Jax aquí desde la Jerarquía
    public Transform startSpawnPoint; // Objeto vacío donde iniciará Jax

    [Header("Prefab de Efecto")]
    public GameObject teleportFxPrefab; // Tu prefab de 2 frames

    [Header("Audio")]
    public AudioClip teleportIntroSound;

    [Header("Ajustes de Tiempo")]
    [Tooltip("Tiempo exacto que tarda el FX del Prefab en destruirse (mismo valor que el delay de SelfDestruct)")]
    public float fxDuration = 0.5f; 

    private void Awake()
    {
        // 1. Antes que nada (en Awake), nos aseguramos de que Jax esté invisible e inmóvil al cargar.
        if (jaxPlayer != null)
        {
            SetJaxVisible(false); // Función auxiliar abajo
        }
    }

    void Start()
    {
        // Posicionamos a Jax en el punto de spawn si existe
        if (startSpawnPoint != null && jaxPlayer != null)
        {
            jaxPlayer.transform.position = startSpawnPoint.position;
        }

        // Iniciamos la secuencia de introducción
        StartCoroutine(IntroCoroutine());
    }

    IEnumerator IntroCoroutine()
    {
        // Una pequeñísima espera (0.1s) para asegurar que la cámara o el HUD estén listos.
        yield return new WaitForSeconds(0.1f);

        // Reproducimos el sonido usando el GameManager
        if(GameManager.Instance != null && GameManager.Instance.uiAudioSource != null && teleportIntroSound != null) {
            GameManager.Instance.uiAudioSource.PlayOneShot(teleportIntroSound);
        }

        // 2. Ejecutamos la animación del teletransporte (Instanciamos el Prefab)
        if (teleportFxPrefab != null && startSpawnPoint != null)
        {
            // Instanciamos el efecto en el punto de inicio.
            // (El prefab se destruirá solo gracias a SelfDestruct)
            Instantiate(teleportFxPrefab, startSpawnPoint.position, Quaternion.identity);
            
            // Sonido de teletransporte (Opcional, si tienes uno configurado)
            // if(GameManager.Instance != null && GameManager.Instance.uiAudioSource != null && GameManager.Instance.confirmSound != null) {
            //     GameManager.Instance.uiAudioSource.PlayOneShot(GameManager.Instance.confirmSound);
            // }
        }

        // 3. Esperamos a que la animación termine (fxDuration).
        // Jax permanece oculto mientras esperamos.
        yield return new WaitForSeconds(fxDuration);

        // 4. Jax aparece "en el mundo" y recupera control.
        if (jaxPlayer != null)
        {
            SetJaxVisible(true);
        }
    }

    // Función auxiliar para no apagar a Jax por completo (SetActive),
    // sino solo su vista y movimiento para no romper scripts de cámara o físicas.
    void SetJaxVisible(bool visible)
    {
        // Apaga o prende el dibujo (Sprite Renderer)
        var spriteRenderer = jaxPlayer.GetComponent<SpriteRenderer>();
        if(spriteRenderer != null) spriteRenderer.enabled = visible;

        // Apaga o prende su controlador (cerebro) para que no se mueva.
        // OJO: Cambia 'Platformer.Mechanics.PlayerController' por el nombre exacto de tu script de Jax.
        var playerCtrl = jaxPlayer.GetComponent<Platformer.Mechanics.PlayerController>(); 
        if(playerCtrl != null) playerCtrl.enabled = visible;
    }
}