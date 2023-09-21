using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raindrop : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody2D _rb;
    private static readonly int Splash = Animator.StringToHash("splash");
    
    void Start()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            _rb.bodyType = RigidbodyType2D.Static;
            _animator.SetTrigger(Splash);
        }
    }

    void DestroyAndNew()
    {
        Leak.Instance.DestroyAndNew(gameObject);
    }
    
}
