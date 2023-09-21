using UnityEngine;

public class Raindrop : MonoBehaviour
{
    private Animator _animator;
    private static readonly int Splash = Animator.StringToHash("splash");
    private Vector3 _initialPosition;
    
    void Start()
    {
        _animator = GetComponent<Animator>();
        _initialPosition = transform.position;
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Platform")) return;
        _animator.SetTrigger(Splash);
    }

    void DestroyAndNew()
    {
        Leak.Instance.DestroyAndNew(gameObject, _initialPosition);
    }
    
}
