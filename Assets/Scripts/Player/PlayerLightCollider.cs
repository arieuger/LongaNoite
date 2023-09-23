using System.Collections;
using UnityEngine;

public class PlayerLightCollider : MonoBehaviour
{

    [SerializeField] private float aliveInDarkness = 10f;
    [SerializeField] private Color healthyColor;
    [SerializeField] private Color deadColor;

    private SpriteRenderer _sr;
    private PlayerLighter _playerLighter;
    private bool _isInDarkness;
    private Coroutine _lastRoutine;

    private void Start()
    {
        _playerLighter = GetComponent<PlayerLighter>();
        _sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Light") && !_playerLighter.IsUsingLantern)
        {
            _lastRoutine = StartCoroutine(AliveInDarknessCountDown());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Light") && _lastRoutine != null)
        {
            StopCoroutine(_lastRoutine);
            _sr.color = healthyColor;
        }
    }

    private IEnumerator AliveInDarknessCountDown()
    {
        float normalizedTime = 0;
        yield return new WaitForSeconds(.75f);
        
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
