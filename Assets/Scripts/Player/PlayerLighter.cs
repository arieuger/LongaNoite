using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerLighter : MonoBehaviour
{
    [SerializeField] private bool canUseLantern;
    [SerializeField] private RuntimeAnimatorController animControllerNoLight;
    [SerializeField] private RuntimeAnimatorController animControllerWithLight;
    [SerializeField] private GameObject parentLights;

    private Animator _animator;
    private bool _isUsingLantern = false;
    
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) && canUseLantern) SwitchLanternUse();
    }

    private void SwitchLanternUse()
    {
        _isUsingLantern = !_isUsingLantern;

        _animator.runtimeAnimatorController = _isUsingLantern ? animControllerWithLight : animControllerNoLight;
        parentLights.SetActive(_isUsingLantern);
        for (int i = 0; i < parentLights.transform.childCount; i++)
        {
            parentLights.transform.GetChild(i).gameObject.SetActive(_isUsingLantern);
        }

    }
}
