using UnityEngine;

public class CannonBullet : MonoBehaviour
{
    public Rigidbody2D rb;
    public int speed;

    public GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
       
    }

    public void Shoot(Vector2 direction)
    {
        rb.linearVelocity = direction.normalized * speed;
    }


}
