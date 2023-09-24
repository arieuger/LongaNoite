using UnityEngine;

public class DisableMovementText : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            GetComponent<LerpAlpha>().IsDisabling = true;
        }
    }

}