using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerLightCollider : MonoBehaviour
{

    [SerializeField] private float aliveInDarkness = 10f;
    [SerializeField] private Color healthyColor;
    [SerializeField] private Color deadColor;

    private SpriteRenderer _sr;
    private bool _isInDarkness;
    private Coroutine lastRoutine = null;

    private void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Light"))
        {
            lastRoutine = StartCoroutine(AliveInDarknessCountDown());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Light") && lastRoutine != null)
        {
            StopCoroutine(lastRoutine);
            _sr.color = healthyColor;
        }
    }

    private IEnumerator AliveInDarknessCountDown()
    {
        float normalizedTime = 0;
        while (normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / aliveInDarkness;
            // Debug.Log(aliveInDarkness - aliveInDarkness * normalizedTime);
            _sr.color = Color.Lerp(healthyColor, deadColor, normalizedTime);
            yield return null;
        }
        Debug.Log("Se murió!!!");
    }
}
