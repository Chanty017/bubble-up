using UnityEngine;

public class BreakablePlate : MonoBehaviour
{
    private bool isPlayerOnPlatform = false;
    private float timeOnPlatform = 0f;
    public float destroyDelay; // Time in seconds before the platform is destroyed
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player") // Check if the colliding object is named "Player"
        {
            isPlayerOnPlatform = true;
            var colorChange = GetComponent<SpriteRenderer>();
            colorChange.color = Color.red;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player") // Check if the exiting object is named "Player"
        {
            isPlayerOnPlatform = false;
            timeOnPlatform = 0f; // Reset timer when the player leaves the platform
        }
    }

    private void Update()
    {
        if (isPlayerOnPlatform)
        {
            timeOnPlatform += Time.deltaTime;

            if (timeOnPlatform >= destroyDelay)
            {
                Destroy(gameObject); // Destroy the platform
            }
        }
    }

}
