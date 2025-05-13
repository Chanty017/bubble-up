using UnityEngine;

public class GateOpen : MonoBehaviour
{
    
    public GameObject gate;
    private bool gateOpen;

    public void Start()
    {
        gate.SetActive(true);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerMovableObject") || other.gameObject.CompareTag("Player"))
        {
            gate.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerMovableObject")||other.gameObject.CompareTag("Player"))
        {
            gate.SetActive(true);
        }
    }

}
