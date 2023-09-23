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
    private bool _IsRoutineRunning;

    private void Start()
    {
        _playerLighter = GetComponent<PlayerLighter>();
        _sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Light") || _IsRoutineRunning) return;
        
        _isInDarkness = true;
        _lastRoutine = StartCoroutine(AliveInDarknessCountDown());
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Light") || _lastRoutine == null) return;
        
        _isInDarkness = false;
        StopCountDown();
    }

    public void WhenLighterSwitched(bool isLighterOn)
    {
        if (_isInDarkness && isLighterOn && _lastRoutine != null) StopCountDown();
        else if (!isLighterOn && _isInDarkness && !_IsRoutineRunning) _lastRoutine = StartCoroutine(AliveInDarknessCountDown());
    }

    private void StopCountDown()
    {
        if (!_IsRoutineRunning) return;
        
        StopCoroutine(_lastRoutine);
        _sr.color = healthyColor;
        _IsRoutineRunning = false;
    }

    private IEnumerator AliveInDarknessCountDown()
    {
        if (_IsRoutineRunning) yield break;
        
        _IsRoutineRunning = true;
        float normalizedTime = 0;
        yield return new WaitForSeconds(.75f);

        while (normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / aliveInDarkness;
            // Debug.Log(aliveInDarkness - aliveInDarkness * normalizedTime);
            _sr.color = Color.Lerp(healthyColor, deadColor, normalizedTime);
            yield return null;
        }

        _IsRoutineRunning = false;
    }
}
