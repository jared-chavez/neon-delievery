using UnityEngine;

public class ScoreBattery : MonoBehaviour
{
    public int energyPoints = 10;

    // Colisión y Transacción
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag("Player"))
        {
            GameManager.Instance.AddScore(energyPoints);
            Destroy(gameObject);
        }
    }
}