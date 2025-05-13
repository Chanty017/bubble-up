using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bubblePrefab;


    public void ShootBubble(Vector3 position, Vector2 direction, Vector2 currentLinearVelocityPlayer)
    {
        var bubble = Instantiate(bubblePrefab, position, Quaternion.identity);
        bubble.GetComponent<Bubble>().Released(direction, currentLinearVelocityPlayer);
    }

}
