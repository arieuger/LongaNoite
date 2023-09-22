using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header ("Movement and velocity")]
    // Movemento e velocidade
    [SerializeField] private float movementSpeed;
    private Rigidbody2D _rb;
    private float _horizontalMovement;
    public float HorizontalMovement => _horizontalMovement;
    private Vector3 _velocity = Vector3.zero;
    private bool _lookingRight = true;

    private bool _shouldMove = true;
    public bool ShouldMove
    {
        get { return _shouldMove;  }
        set { _shouldMove = value; }
    }
    
    [Header ("Jump and groundcheck")]
    // Salto e suelo
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private Transform groundController;
    [SerializeField] private Vector3 dimensionBox;
    [SerializeField] private float fallGravityScale;
    [SerializeField] private float coyoteTime;
    private float _defaultGravityScale;
    private float _coyoteTimeCounter;
    private bool _isGrounded;
    public bool IsGrounded => _isGrounded;
    private bool _jump;
    public bool Jump => _jump;

    [Header("Wall Jumping")] 
    [SerializeField] private float wallJumpTime = 0.2f;
    [SerializeField] private float wallSlideSpeed = 0.3f;
    [SerializeField] private float wallDistance = 0.5f;
    private bool _isWallSliding = false;
    private RaycastHit2D _wallCheckHit;
    private float _jumpTime;

    // Dashing
    [Header ("Dashing")]
    [SerializeField] private float dashingPower = 24f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 1f;
    private bool _canDash = true;
    private bool _isDashing;

    [Header ("Particle Effects")]
    // Partículas
    [SerializeField] private ParticleSystem footstepsEffect;
    [SerializeField] private ParticleSystem jumpEffect;
    [SerializeField] private ParticleSystem dashEffect;
    private ParticleSystem.EmissionModule _footEmission;
    private ParticleSystem.MinMaxCurve _initialFootEmissionRot;

    // Animacións
    private Animator _animator;
    private static readonly int Movement = Animator.StringToHash("horizontalMovement");
    private static readonly int VerticalMovement = Animator.StringToHash("verticalMovement");
    private static readonly int Grounded = Animator.StringToHash("isGrounded");
    private static readonly int IsDashing = Animator.StringToHash("isDashing");
    private static readonly int IsSliding = Animator.StringToHash("isSliding");

    // Singleton
    public static PlayerMovement Instance { get; private set; }
    private void Awake() 
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }
    
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _footEmission = footstepsEffect.emission;
        _initialFootEmissionRot = _footEmission.rateOverTime;
        _defaultGravityScale = _rb.gravityScale;
    }

    void Update()
    {
        UpdateAnimations();

        if (_shouldMove) _horizontalMovement = Input.GetAxis("Horizontal") * movementSpeed;
        else _horizontalMovement = 0;
        
        if (Input.GetButtonDown("Jump") && _shouldMove) _jump = true;
        if (Input.GetKeyDown(KeyCode.LeftShift) && _canDash) StartCoroutine(Dash()); // TODO: Key to Button
    
        if (_isGrounded) _coyoteTimeCounter = coyoteTime;
        else _coyoteTimeCounter -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        if (_isDashing) return;
        // TODO: Health?
        _isGrounded = Physics2D.OverlapBox(groundController.position, dimensionBox, 0f, groundLayers);
        Move(_horizontalMovement * Time.fixedDeltaTime);
        
        // Wall Sliding and Jump
        wallDistance = _lookingRight ? Mathf.Abs(wallDistance) : Mathf.Abs(wallDistance) * -1;
        _wallCheckHit = Physics2D.Raycast(transform.position, new Vector2(wallDistance, 0), Mathf.Abs(wallDistance), groundLayers);
        Debug.DrawRay(transform.position, new Vector2(wallDistance, 0), Color.red);
        
        if (_wallCheckHit && !_isGrounded && _horizontalMovement != 0)
        {
            _isWallSliding = true;
            _jumpTime = Time.time + wallJumpTime;
        } else if (_jumpTime < Time.time || _horizontalMovement == 0)
        {
            _isWallSliding = false;
        }

        if (_isWallSliding)
        {
            var velocity = _rb.velocity;
            _rb.velocity = new Vector2(velocity.x, Mathf.Clamp(velocity.y, -wallSlideSpeed, float.MaxValue));
            // _rb.velocity = new Vector2(velocity.x, wallSlideSpeed);
        }
        
        CheckGravityScale();
    }

    private void UpdateAnimations() {
        _animator.SetFloat(Movement, Mathf.Abs(_horizontalMovement));
        _animator.SetFloat(VerticalMovement, _isGrounded || _isWallSliding ? 0 : _rb.velocity.y);
        _animator.SetBool(Grounded, _isGrounded);
        _animator.SetBool(IsDashing, _isDashing);
        _animator.SetBool(IsSliding, _isWallSliding);
    }
    
    private void Move(float moving) {
        // Desplazamento
        if (!_isWallSliding || _jump)
        {
            Vector3 targetVelocity = new Vector2(moving, _rb.velocity.y);
            _rb.velocity = targetVelocity;
        }

        // Xiro
        if (moving > 0 && !_lookingRight) Turn();
        else if (moving < 0 && _lookingRight) Turn();

        // efectos de partículas
        if (moving != 0 && _isGrounded && !_isDashing) _footEmission.rateOverTime = _initialFootEmissionRot;
        else _footEmission.rateOverTime = 0f;

        // Salto
        if (_jump && ((_isGrounded || _coyoteTimeCounter > 0f) || _isWallSliding))
        {
            float xForce = 0;
            if (_isWallSliding)
            {
                xForce = 5f * (_lookingRight ? -1f : 1f);
                // Turn();
                _isWallSliding = false;
            }
            
            _isGrounded = false;
            _rb.AddForce(new Vector2(xForce, jumpForce), ForceMode2D.Impulse);
            _coyoteTimeCounter = 0f;
            jumpEffect.Play();
        }
        _jump = false;
    }

    private void Turn() {
        _lookingRight = !_lookingRight;
        _isWallSliding = false;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        CameraFollow.Instance.TurnCamera();
    }
    
    private void CheckGravityScale()
    {
        _rb.gravityScale = _rb.velocity.y < -0.1f && !_isWallSliding ? fallGravityScale : _defaultGravityScale;
        CameraFollow.Instance.FollowFallingUpOrDown(_rb.velocity.y >= -0.1f);
    }

    private IEnumerator Dash()
    {
        _canDash = false;
        _isDashing = true;
        dashEffect.Play();
        _rb.gravityScale = 0f;
        _rb.velocity = new(transform.localScale.x * dashingPower, 0f);
        
        yield return new WaitForSeconds(dashingTime);

        _rb.gravityScale = _defaultGravityScale;
        _isDashing = false;
        
        yield return new WaitForSeconds(0.1f);
        dashEffect.Stop();

        yield return new WaitForSeconds(dashingCooldown);
        _canDash = true;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(groundController.position, dimensionBox);
    }
}