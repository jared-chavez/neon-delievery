using UnityEngine;

[RequireComponent(typeof(AudioSource))] // Esto asegura que Unity le ponga un AudioSource a Jax automáticamente
public class JaxAudio : MonoBehaviour
{
    [Header("Reproductor")]
    private AudioSource audioSource;

    [Header("Efectos de Sonido (Asigna tus .wav aquí)")]
    public AudioClip jumpSound;     // Sugerencia: plasma.wav
    public AudioClip landSound;     // Sugerencia: mechanical.wav
    public AudioClip attack1Sound;  // Sugerencia: blaster.wav
    public AudioClip attack2Sound;  // Sugerencia: gunshot.wav
    public AudioClip hurtSound;     // Sugerencia: mechanical.wav (o uno nuevo)

    void Awake()
    {
        // Conectamos el reproductor de sonido
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    // --- FUNCIONES PÚBLICAS PARA REPRODUCIR SONIDO ---

    public void PlayJump()
    {
        if (jumpSound != null) audioSource.PlayOneShot(jumpSound);
    }

    public void PlayLand()
    {
        if (landSound != null) audioSource.PlayOneShot(landSound);
    }

    public void PlayAttack1()
    {
        if (attack1Sound != null) audioSource.PlayOneShot(attack1Sound);
    }

    public void PlayAttack2()
    {
        if (attack2Sound != null) audioSource.PlayOneShot(attack2Sound);
    }

    public void PlayHurt()
    {
        if (hurtSound != null) audioSource.PlayOneShot(hurtSound);
    }
}