using UnityEngine;

public class RockPrefab : MonoBehaviour
{
    public float fallSpeed;
    void Update()
    {
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);

    }
}
