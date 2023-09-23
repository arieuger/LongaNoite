using System.Collections;
using UnityEngine;

public class PlayerLighter : MonoBehaviour
{
    public bool IsUsingLantern { private set; get; }
    
    [SerializeField] private bool canUseLantern;
    [SerializeField] private RuntimeAnimatorController animControllerNoLight;
    [SerializeField] private RuntimeAnimatorController animControllerWithLight;
    [SerializeField] private GameObject parentLights;
    [SerializeField] private float lighterTime = 5f;

    private Animator _animator;
    private Coroutine _lastRoutine;
    
    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) && canUseLantern) SwitchLanternUse();
    }

    private void SwitchLanternUse()
    {
        IsUsingLantern = !IsUsingLantern;

        _animator.runtimeAnimatorController = IsUsingLantern ? animControllerWithLight : animControllerNoLight;
        parentLights.SetActive(IsUsingLantern);
        for (int i = 0; i < parentLights.transform.childCount; i++)
        {
            parentLights.transform.GetChild(i).gameObject.SetActive(IsUsingLantern);
        }

        if (IsUsingLantern) _lastRoutine = StartCoroutine(PlayerLighterCountDown());
        else if (_lastRoutine != null) StopCoroutine(_lastRoutine);
    }

    private IEnumerator PlayerLighterCountDown()
    {
        float normalizedTime = 0;
        
        while (normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / lighterTime;
            yield return null;
        }
        SwitchLanternUse();
    }
}
