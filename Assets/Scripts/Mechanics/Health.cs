using System;
using UnityEngine;
using NeonDelivery.Core;

namespace Platformer.Mechanics
{
    public class Health : MonoBehaviour
    {
        public int maxHP = 1;
        int currentHP;

        [Header("Visual Configuration (Jacket LED)")]
        [SerializeField] private SpriteRenderer jacketRenderer;
        [SerializeField] private Color aliveColor = Color.cyan;
        [SerializeField] private Color deadColor = Color.red;

        public bool IsAlive => currentHP > 0;

        void Awake()
        {
            currentHP = maxHP;
            if (jacketRenderer == null) jacketRenderer = GetComponent<SpriteRenderer>();
            
            UpdateJacketColor();
        }

        public void Decrement()
        {
            currentHP = Mathf.Clamp(currentHP - 1, 0, maxHP);
            UpdateJacketColor();

            if (currentHP == 0)
            {
                Die();
            }
        }

        public void Die()
        {
            currentHP = 0;
            UpdateJacketColor();
            
            Debug.Log("Jax ha quedado fuera de combate.");
            
            if (GameManager.Instance != null)
            {
                GameManager.Instance.UpdateGameState(GameState.GameOver);
            }
        }

        private void UpdateJacketColor()
        {
            if (jacketRenderer != null)
            {
                jacketRenderer.color = IsAlive ? aliveColor : deadColor;
            }
        }

        public void Increment()
        {
            currentHP = Mathf.Clamp(currentHP + 1, 0, maxHP);
            UpdateJacketColor();
        }
    }
}