using UnityEngine;

namespace Level
{
    public class WallTeleportTrigger : MonoBehaviour
    {
        [SerializeField] private Transform _teleportPoint;

        private void OnTriggerEnter2D(Collider2D other)
        {
            var playerMarker = other.GetComponent<Player.PlayerMarker>();
            if (playerMarker != null)
            {

                other.transform.root.position = _teleportPoint.position;

            }
        }
    }
}
