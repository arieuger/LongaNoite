using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LighterChangeSize : MonoBehaviour
{

    private float _minSize;
    private float _maxSize;
    private Light2D _lightComponent;
    
    // Start is called before the first frame update
    void Start()
    {
        _lightComponent = GetComponent<Light2D>();
        
        var localScale = _lightComponent.transform.localScale;
        _minSize = localScale.x - .02f;
        _maxSize = localScale.x + .02f;

        StartCoroutine(LighterChangeSizeCo());
    }

    private IEnumerator LighterChangeSizeCo()
    {
        while (true)
        {
            float newSize = Random.Range(_minSize, _maxSize);
            _lightComponent.transform.localScale = new Vector3(newSize, newSize, 1f);
            
            yield return new WaitForSeconds(Random.Range(0.2f, 0.7f));
        }
    }    
    
}
