using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLightCollider : MonoBehaviour
{

    [SerializeField] private float aliveInDarkness = 10f;
    [SerializeField] private Color healthyColor;
    [SerializeField] private Color deadColor;
    [SerializeField] private Image heartBar;

    private SpriteRenderer _sr;
    private PlayerLighter _playerLighter;
    private bool _isInDarkness;
    private Coroutine _lastRoutine;
    private bool _isRoutineRunning;

    private void Start()
    {
        _playerLighter = GetComponent<PlayerLighter>();
        _sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!gameObject.activeSelf || !other.CompareTag("Light") || _isRoutineRunning) return;
        
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
        else if (!isLighterOn && _isInDarkness && !_isRoutineRunning) _lastRoutine = StartCoroutine(AliveInDarknessCountDown());
    }

    private void StopCountDown()
    {
        if (!_isRoutineRunning) return;
        
        StopCoroutine(_lastRoutine);
        _sr.color = healthyColor;
        _isRoutineRunning = false;
        heartBar.fillAmount = 1f;
    }

    private IEnumerator AliveInDarknessCountDown()
    {
        if (_isRoutineRunning) yield break;
        
        _isRoutineRunning = true;
        float normalizedTime = 0;
        yield return new WaitForSeconds(.25f);

        while (normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / aliveInDarkness;
            // Debug.Log(aliveInDarkness - aliveInDarkness * normalizedTime);
            _sr.color = Color.Lerp(healthyColor, deadColor, normalizedTime);
            heartBar.fillAmount = 1f - normalizedTime;
            yield return null;
        }

        _isRoutineRunning = false;
    }
}
