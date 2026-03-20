using System.Collections;
using UnityEngine;
using Platformer.Mechanics; // Para poder acceder al script de Jax

public class NeonSpawn : MonoBehaviour
{
    [Header("Conexiones")]
    public GameObject jaxPlayer; // Arrastra a Jax aquí desde el Inspector
    
    [Header("Ajustes")]
    public float spawnDuration = 1.2f; // Ajusta esto a lo que dure tu animación morada

    void Start()
    {
        StartCoroutine(SpawnSequence());
    }

    IEnumerator SpawnSequence()
    {
        // 1. Obtenemos los componentes de Jax
        PlayerController controller = jaxPlayer.GetComponent<PlayerController>();
        SpriteRenderer jaxSprite = jaxPlayer.GetComponent<SpriteRenderer>();

        // 2. Congelamos a Jax y lo hacemos invisible temporalmente
        if (controller != null) controller.enabled = false;
        if (jaxSprite != null) jaxSprite.enabled = false;

        // 3. Esperamos a que la animación de este FX termine
        yield return new WaitForSeconds(spawnDuration);

        // 4. Revelamos a Jax y le devolvemos el control
        if (jaxSprite != null) jaxSprite.enabled = true;
        if (controller != null) controller.enabled = true;

        // 5. Destruimos el destello morado
        Destroy(gameObject);
    }
}