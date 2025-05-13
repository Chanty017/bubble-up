using System.Collections;
using UnityEngine;

public class FloatingBubble : MonoBehaviour
{
    public int speed;
    private bool floatBullbble;
    public Transform floatPlatform;

    private void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        if (floatBullbble)
        {
            transform.Translate(Vector2.up * speed * Time.deltaTime);

        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            floatBullbble = true;


            other.transform.SetParent(floatPlatform.transform);

            Debug.Log("Entered");
            StartCoroutine(BubbleTime());

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(null);

            floatBullbble = false;
            Destroy(gameObject);

        }
    }
    IEnumerator BubbleTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(6);
            Destroy(gameObject);
        }
    }
}
