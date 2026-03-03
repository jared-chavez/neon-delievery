using System.Collections;
using UnityEngine;
using TMPro; // Necesario para el manejo avanzado de texto
using NeonDelivery.Core;

namespace NeonDelivery.UI
{
    public class VeraDialogueUI : MonoBehaviour
    {
        [Header("Referencias de Interfaz")]
        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private GameObject dialoguePanel;

        [Header("Configuración del Glitch")]
        [SerializeField] private float typingSpeed = 0.05f;
        [SerializeField] private float glitchChance = 0.15f;
        private string characters = "!@#$%^&*()_+1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public void ShowDialogue(string message)
        {
            dialoguePanel.SetActive(true);
            StopAllCoroutines();
            StartCoroutine(TypeMessageWithGlitch(message));
        }

        private IEnumerator TypeMessageWithGlitch(string targetMessage)
        {
            string currentDisplay = "";
            
            // Pausamos el juego mientras V.E.R.A. habla
            GameManager.Instance.UpdateGameState(GameState.Dialogue);

            for (int i = 0; i < targetMessage.Length; i++)
            {
                // Efecto Glitch: Antes de mostrar la letra real, mostramos una aleatoria
                if (Random.value < glitchChance)
                {
                    dialogueText.text = currentDisplay + characters[Random.Range(0, characters.Length)];
                    yield return new WaitForSeconds(typingSpeed / 2);
                }

                currentDisplay += targetMessage[i];
                dialogueText.text = currentDisplay;
                yield return new WaitForSeconds(typingSpeed);
            }

            // Esperar a que el jugador presione una tecla para continuar
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
            
            CloseDialogue();
        }

        private void CloseDialogue()
        {
            dialoguePanel.SetActive(false);
            // Regresamos a la acción en el Bajo Neón
            GameManager.Instance.UpdateGameState(GameState.Playing);
        }
    }
}