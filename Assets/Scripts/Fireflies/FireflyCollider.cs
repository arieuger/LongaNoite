using UnityEngine;

public class FireflyCollider : MonoBehaviour
{

    [SerializeField] private FireflyFlight flight;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            flight.ActivateMovement();
            gameObject.SetActive(false);
        }
    }
    
}
