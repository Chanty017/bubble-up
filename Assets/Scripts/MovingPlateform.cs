using UnityEngine;

public class MovingPlateform : MonoBehaviour
{
    private Vector3 startPosition;

    public float distance;

    public float speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        var movement = Mathf.PingPong(Time.time * speed, distance);
        transform.position = new Vector3(startPosition.x + movement, startPosition.y , startPosition.z);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            transform.SetParent(other.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            transform.SetParent(null);
        }
    }

}
