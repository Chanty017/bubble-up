using Unity.VisualScripting;
using UnityEngine;

public class CameraFollowVertical : MonoBehaviour
{
    public Transform player;  // Assign the player transform
    public float yOffset = 2f; // The offset after which the camera starts moving

    private float highestY; // Keeps track of the highest camera position

    [SerializeField] Transform respawnCollider;
    void Start()
    {
        if (player != null)
        {
            highestY = transform.position.y; // Initialize highestY to the starting camera position
        }
        ArrangeRespawnCollider();
    }

    void ArrangeRespawnCollider()
    {
        var targetPos = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0));

        respawnCollider.transform.position = targetPos;
    }

    void LateUpdate()
    {
        if (player == null) return;

        float targetY = player.position.y - yOffset; // Calculate target Y position

        if (targetY > highestY) // Move up only if the player moves beyond the threshold
        {
            highestY = targetY;
            transform.position = new Vector3(transform.position.x, highestY, transform.position.z);
        }
    }
}
