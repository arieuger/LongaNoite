using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLantern : MonoBehaviour
{
    [SerializeField] private bool canUseLantern;
    [SerializeField] private RuntimeAnimatorController animController;

    private Animator _animator;
    private bool _isUsingLantern = false;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) && canUseLantern) SwitchLanternUse();
    }

    private void SwitchLanternUse()
    {
        _isUsingLantern = !_isUsingLantern;
        
    }
}
