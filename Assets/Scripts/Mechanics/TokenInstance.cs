using UnityEngine;
using Platformer.Mechanics;
using Platformer.Gameplay;

namespace Platformer.Mechanics
{
    [RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
    public class TokenInstance : MonoBehaviour
    {
        // Definimos los tipos para fundamentar la temática
        public enum TokenType { IonBattery, DataPackage }
        
        [Header("Configuración Neon Delivery")]
        public TokenType type;
        public AudioClip tokenCollectAudio;
        
        [Header("Animación Simple (8-bit)")]
        public Sprite[] idleAnimation;
        public float animationSpeed = 0.1f;

        private SpriteRenderer _renderer;
        private int _frame;
        private float _timer;
        private bool _collected = false;

        void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            // Aseguramos que el colisionador sea Trigger para Jax
            GetComponent<Collider2D>().isTrigger = true;
        }

        void Update()
        {
            if (_collected) return;
            HandleAnimation();
        }

        private void HandleAnimation()
        {
            if (idleAnimation.Length == 0) return;

            _timer += Time.deltaTime;
            if (_timer >= animationSpeed)
            {
                _timer = 0;
                _frame = (_frame + 1) % idleAnimation.Length;
                _renderer.sprite = idleAnimation[_frame];
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            // Verificamos si es Jax quien entra en contacto
            var player = other.gameObject.GetComponent<PlayerController>();
            if (player != null && !_collected)
            {
                OnPlayerCollect(player);
            }
        }

        private void OnPlayerCollect(PlayerController player)
        {
            _collected = true;

            // Lógica de "Impulso" o "Recolección"
            if (TokenController.Instance != null)
            {
                TokenController.Instance.OnTokenCollected(this, player);
            }

            // Feedback sonoro opcional
            if (tokenCollectAudio != null)
            {
                AudioSource.PlayClipAtPoint(tokenCollectAudio, transform.position);
            }

            // Desactivamos el objeto (Borrón y cuenta nueva en el mapa)
            gameObject.SetActive(false);
        }
    }
}