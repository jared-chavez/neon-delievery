using UnityEngine;
using UnityEngine.UI; // Necesario para modificar la UI

[RequireComponent(typeof(Graphic))] // Se asegura de que haya una Imagen o Texto
public class UIBlinker : MonoBehaviour
{
    [Header("Ajustes de Parpadeo")]
    [Tooltip("A mayor número, más rápido parpadea")]
    public float blinkSpeed = 3f; 

    private Graphic uiElement;

    void Awake()
    {
        // Obtenemos la Imagen o el Texto al que le pusimos este script
        uiElement = GetComponent<Graphic>();
    }

    void Update()
    {
        // EL SECRETO: Usamos 'unscaledTime' porque el juego está pausado (timeScale = 0)
        Color c = uiElement.color;
        
        // Mathf.Sin crea una onda que sube y baja suavemente la transparencia (Alpha)
        c.a = Mathf.Abs(Mathf.Sin(Time.unscaledTime * blinkSpeed));
        
        uiElement.color = c;
    }
}