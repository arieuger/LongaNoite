using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LighterFlick : MonoBehaviour
{
    [SerializeField] private float minIntensity = 0f;
    [Range(1, 50)]
    [SerializeField] private int smoothing = 5;

    private float _maxIntensity;
    private Light2D _light;

    // Continuous average calculation via FIFO queue
    // Saves us iterating every time we update, we just change by the delta
    private Queue<float> _smoothQueue;
    private float _lastSum = 0;
    private bool _isLightNull;


    /// <summary>
    /// Reset the randomness and start again. You usually don't need to call
    /// this, deactivating/reactivating is usually fine but if you want a strict
    /// restart you can do.
    /// </summary>
    public void Reset() {
        _smoothQueue.Clear();
        _lastSum = 0;
    }

    void Start() {
        _smoothQueue = new Queue<float>(smoothing);
        _light = GetComponent<Light2D>();
        _isLightNull = _light == null;
        _maxIntensity = _light.intensity;
    }

    void Update() {
        if (_isLightNull) return;

        // pop off an item if too big
        while (_smoothQueue.Count >= smoothing) {
            _lastSum -= _smoothQueue.Dequeue();
        }

        // Generate random new item, calculate new average
        float newVal = Random.Range(minIntensity, _maxIntensity);
        _smoothQueue.Enqueue(newVal);
        _lastSum += newVal;

        // Calculate new smoothed average
        _light.intensity = _lastSum / (float)_smoothQueue.Count;
    }

}
