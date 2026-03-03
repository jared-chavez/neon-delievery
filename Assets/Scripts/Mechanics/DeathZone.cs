using UnityEngine;
using Platformer.Mechanics;

namespace Platformer.Mechanics
{
    /// <summary>
    /// Component for the abyss of the Sector. 
    /// Detects when Jax falls into the "concrete sea" and activates his death sequence.
    /// </summary>
    public class DeathZone : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D collider)
        {
            var health = collider.gameObject.GetComponent<Health>();
            
            if (health != null && health.IsAlive)
            {
                health.Die();
            }
        }
    }
}