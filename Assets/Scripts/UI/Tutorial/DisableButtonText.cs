using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableButtonText : MonoBehaviour
{
    [SerializeField] private string buttonName;
    private bool _canHide = false;

    void Update() {
        if (Input.GetButtonDown(buttonName) && _canHide)
            GetComponent<LerpAlpha>().IsDisabling = true;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            _canHide = true;
        }
    }
}
