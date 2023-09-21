using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireflyCollider : MonoBehaviour
{

    [SerializeField] private FireflyFlight flight;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) flight.ActivateMovement();
    }
    
}
