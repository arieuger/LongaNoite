using System.Collections;
using UnityEngine;
using TMPro;

public class LerpAlpha : MonoBehaviour {

    private TMP_Text _text;

    public bool IsDisabling { private set; get; }

    void Start() {
        _text = GetComponent<TMP_Text>();
        StartCoroutine(LerpAlphaCo());
    }

    public IEnumerator LerpAlphaCo() {

        float duration = 1.5f;
        float lerpTimer = 0f;
        float minAlpha = 0.1f;
        float maxAlpha = 0.7f;

        while (!IsDisabling) {
            Color color = _text.color;
            lerpTimer += Time.deltaTime;
            float lerp = Mathf.PingPong(lerpTimer, duration) / duration;
            color.a = Mathf.Lerp(minAlpha, maxAlpha, Mathf.SmoothStep(minAlpha, maxAlpha, lerp));
            _text.color = color;
            yield return null;
        }

        StartCoroutine(DisableText());
    }

    public IEnumerator DisableText() {
        while (_text.color.a >= 0.05f) {
            Color color = _text.color;
            color.a -= Time.deltaTime;
            _text.color = color;
            yield return null;
        }
        gameObject.SetActive(false);
    }
}