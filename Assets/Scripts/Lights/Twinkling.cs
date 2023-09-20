using System.Collections;
using UnityEngine.Rendering.Universal;
using UnityEngine;

public class Twinkling : MonoBehaviour {
    
    [SerializeField] private float minIntensity;
    [SerializeField] bool twinklingActive = true;

    private float _maxIntensity;
    private Light2D _lightComponent;
    
    void Start() {
        _lightComponent = GetComponent<Light2D>();
        _maxIntensity = _lightComponent.intensity;
        StartCoroutine(Twinkle());
    }

    private IEnumerator Twinkle() {
        while (true) {            

            if (twinklingActive) {
                
                // Tintileo
                for (int i = 0; i < Random.Range(2,8); i++) {
                    _lightComponent.intensity = Random.Range(minIntensity, minIntensity + 1.3f);
                    yield return new WaitForSeconds(Random.Range(0.01f, 0.2f));

                    _lightComponent.intensity = _maxIntensity;
                    yield return new WaitForSeconds(Random.Range(0.01f, 0.2f));
                }

                // Espera                
                yield return new WaitForSeconds(Random.Range(0.5f, 5f));
            
            } else yield return null;
            
        }

    }
}
