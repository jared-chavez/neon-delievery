using UnityEngine;

namespace Platformer.Mechanics
{
    public class PatrolPath : MonoBehaviour
    {
        // Puntos que definen la ruta de patrullaje en el Distrito Industrial
        public Transform[] waypoints;

        private void OnDrawGizmos()
        {
            // Visualización en el editor de Unity para facilitar el diseño del nivel
            if (waypoints == null || waypoints.Length < 2) return;

            Gizmos.color = Color.cyan;
            for (int i = 0; i < waypoints.Length - 1; i++)
            {
                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
            }
        }
    }
}