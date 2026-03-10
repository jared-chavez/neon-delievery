using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    private float length, startpos;
    public GameObject cam;
    public float parallaxFactor;
    private bool hasSprite;

    void Start()
    {
        startpos = transform.position.x;
        
        // Verificamos si el objeto tiene un SpriteRenderer
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        
        if (sprite != null)
        {
            length = sprite.bounds.size.x;
            hasSprite = true;
        }
        else
        {
            hasSprite = false;
        }
    }

    void Update()
    {
        // Si no hay cámara asignada, salimos para evitar errores en consola
        if (cam == null) return;

        float temp = (cam.transform.position.x * (1 - parallaxFactor));
        float dist = (cam.transform.position.x * parallaxFactor);

        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        // Solo aplicamos la repetición infinita si el objeto tiene un sprite (como el cielo)
        if (hasSprite)
        {
            if (temp > startpos + length) startpos += length;
            else if (temp < startpos - length) startpos -= length;
        }
    }
}