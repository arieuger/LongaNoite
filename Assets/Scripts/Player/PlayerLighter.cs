using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLighter : MonoBehaviour
{
    public bool IsUsingLantern { private set; get; }
    
    [SerializeField] private bool canUseLantern;
    [SerializeField] private RuntimeAnimatorController animControllerNoLight;
    [SerializeField] private RuntimeAnimatorController animControllerWithLight;
    [SerializeField] private GameObject parentLights;
    [SerializeField] private float lighterTime = 5f;
    [SerializeField] private float coolDownTime = 3f;
    [SerializeField] private Image fireBar;
    [SerializeField] private Image backgroundBar;
    [SerializeField] private Color countdownColor;
    [SerializeField] private Color coolDownColor;
    [SerializeField] private Color BackgroundColor;

    private Animator _animator;
    private PlayerLightCollider _playerLightCollider;
    private Coroutine _lastRoutine;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _playerLightCollider = GetComponent<PlayerLightCollider>();
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

        if (_lastRoutine != null) StopCoroutine(_lastRoutine);

        if (IsUsingLantern) _lastRoutine = StartCoroutine(PlayerLighterCountDown());
        else _lastRoutine = StartCoroutine(PlayerLighterCoolDown()); 
        _playerLightCollider.WhenLighterSwitched(IsUsingLantern);
    }

    private IEnumerator PlayerLighterCoolDown()
    {
        canUseLantern = false;
        float normalizedTime = 0;
        fireBar.fillOrigin = (int) Image.OriginHorizontal.Left;
        fireBar.color = coolDownColor;
        backgroundBar.color = countdownColor;
        
        while (normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / coolDownTime;
            fireBar.fillAmount = 1f - normalizedTime;
            yield return null;
        }
        fireBar.fillOrigin = (int) Image.OriginHorizontal.Right;
        fireBar.color = countdownColor;
        fireBar.fillAmount = 1f;
        backgroundBar.color = BackgroundColor;
        
        canUseLantern = true;

    }

    private IEnumerator PlayerLighterCountDown()
    {
        float normalizedTime = 0;
        
        while (normalizedTime <= 1f)
        {
            normalizedTime += Time.deltaTime / lighterTime;
            fireBar.fillAmount = 1f - normalizedTime;
            yield return null;
        }
        SwitchLanternUse();
    }
}
