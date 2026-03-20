using UnityEngine;

public class LevelExit : MonoBehaviour
{
    [Header("Efectos Opcionales")]
    [Tooltip("Arrastra aquí tu prefab de teletransporte para un efecto al ganar")]
    public GameObject teleportFxPrefab; 
    public AudioClip victorySound; 

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // Verificamos si es Jax quien tocó la meta
        if (hitInfo.CompareTag("Player"))
        {
            // 1. Instanciamos el efecto de teletransporte sobre Jax (opcional)
            if (teleportFxPrefab != null)
            {
                Instantiate(teleportFxPrefab, hitInfo.transform.position, Quaternion.identity);
            }

            if (GameManager.Instance != null && GameManager.Instance.uiAudioSource != null && victorySound != null) {
                GameManager.Instance.uiAudioSource.PlayOneShot(victorySound);
            }

            // 2. Apagamos el sprite de Jax para simular que se teletransportó
            SpriteRenderer jaxSprite = hitInfo.GetComponent<SpriteRenderer>();
            if (jaxSprite != null)
            {
                jaxSprite.enabled = false;
            }

            // 3. Llamamos a la función de victoria en el GameManager
            if (GameManager.Instance != null)
            {
                GameManager.Instance.LevelComplete();
            }

            // 4. Destruimos la meta flotante para que no se pueda volver a tocar
            Destroy(gameObject);
        }
    }
}