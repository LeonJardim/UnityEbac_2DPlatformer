using DG.Tweening;
using Leon.Core.InputActions;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    #region Variables
    [Header("References")]
    [SerializeField] private Transform sprite;
    [SerializeField] private GunBase gun;
    public SOPlayerSetup soP;
    
    private Rigidbody2D rb;
    private Animator animator;
    private PlayerInputActions playerControls;
    private InputAction move;
    private InputAction jump;
    private InputAction sprint;
    private InputAction fire;
    private Coroutine _currentCoroutine;
    private HealthBase _health;

    private bool _isSprinting = false;
    private bool _lastFloorCheck = false;
    private Vector3 _initialScale;
    private bool _isDead = false;
    private bool _facingRight = true;
    private bool _isTurning = false;
    #endregion


    #region Awake and Enable/Disable Inputs
    private void Awake()
    {
        playerControls = new PlayerInputActions();
        _initialScale = sprite.localScale;
        animator = sprite.GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        _health = GetComponent<HealthBase>();
        if (_health != null )
        {
            _health.OnKill += DeathAnimation;
        }
    }
    private void OnEnable()
    {
        move = playerControls.Player.Move; move.Enable();
        jump = playerControls.Player.Jump; jump.Enable();
        sprint = playerControls.Player.Sprint; sprint.Enable();
        fire = playerControls.Player.Attack; fire.Enable();
    }
    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
        sprint.Disable();
        fire.Disable();
    }
    #endregion


    private void Update()
    {
        if (_isDead) return;
        HandleJump();
        HandleMovement();
        HandleGun();
    }
    

    private void HandleMovement() 
    {    // Movement
        Vector2 inputDirection = move.ReadValue<Vector2>();
        float targetSpeed = inputDirection.x * GetSpeed;

        rb.linearVelocityX = Mathf.MoveTowards(rb.linearVelocityX, targetSpeed, soP.friction * Time.deltaTime);

         // Set Animations
        if (Mathf.Abs(rb.linearVelocityX) > 0.1f)
        {
            animator.SetBool(soP.animRunBool, true);
            animator.speed = _isSprinting ? 2f : 1f;
            FlipSprite();
        }
        else
        {
            animator.SetBool(soP.animRunBool, false);
            animator.speed = 1f;
        }
        
    }
    private void HandleJump() 
    {
        bool isOnFloor = IsOnFloor();

        if (Mathf.Abs(rb.linearVelocityY) > 0) { animator.SetFloat(soP.animYSpeed, rb.linearVelocityY > 0f ? 1f : -1f); }
        else { animator.SetFloat(soP.animYSpeed, 0f); }

        if (isOnFloor && jump.triggered)
        {
            rb.linearVelocityY = soP.jumpForce;
            JumpAnimation();
        }
        if (isOnFloor && !_lastFloorCheck)
        {
            JumpAnimation(true);
            _lastFloorCheck = true;
        }

        _lastFloorCheck = isOnFloor;
    }
    private void HandleGun()
    {
        if (gun == null || gun.enabled == false) return;

        if (fire.WasPressedThisFrame())
        {
            _currentCoroutine = StartCoroutine(StartShooting());
        }
        else if (fire.WasReleasedThisFrame())
        {
            if (_currentCoroutine != null) StopCoroutine(_currentCoroutine);
        }
    }

    IEnumerator StartShooting()
    {
        while (true)
        {
            gun.facingLeft = !_facingRight;
            gun.Shoot();
            yield return new WaitForSeconds(soP.fireRate);
        }
    }
    private void JumpAnimation(bool landing = false)
    {
        sprite.localScale = _initialScale;
        DOTween.Kill(sprite);
        float sX = soP.jumpScaleX;
        float sY = soP.jumpScaleY;
        float d = soP.animationDuration;
        if (landing)
        {
            sX = 1.0f / sX;
            sY = 1.0f / sY;
            d *= 0.5f;
        }
        sprite.DOScaleY(sprite.localScale.y * sY, d).SetLoops(2, LoopType.Yoyo).SetEase(soP.ease);
        sprite.DOScaleX(sprite.localScale.x * sX, d).SetLoops(2, LoopType.Yoyo).SetEase(soP.ease);
    }
    private void FlipSprite()
    {
        if (transform.localScale.x * rb.linearVelocityX < 0 && !_isTurning)
        {
            _isTurning = true;
            transform.DOScaleX(-transform.localScale.x, soP.turningDuration).OnComplete(() =>
            {
                _isTurning = false;
                _facingRight = transform.localScale.x > 0;
            });
        }
    }
    private void DeathAnimation()
    {
        _health.OnKill -= DeathAnimation;
        animator.SetTrigger(soP.animDeath);
        _isDead = true;
    }


    #region Variable Functions
    private bool IsOnFloor()
    {
        if (Physics2D.BoxCast(transform.position, soP.castBoxSize, 0f, Vector2.down, soP.castDistance, soP.groundLayer))
            {  animator.SetBool(soP.animOnFloorBool, true); return true;}
        else
            { animator.SetBool(soP.animOnFloorBool, false); return false; }
    }
    //private void OnDrawGizmos() { Gizmos.DrawWireCube(transform.position + (Vector3.down * soP.castDistance), soP.castBoxSize); }
    public float GetSpeed
    {
        get
        {
            if (sprint.IsPressed())
            {
                _isSprinting = true;
                return soP.sprintSpeed;
            }
            else
            {
                _isSprinting = false;
                return soP.speed;
            }
        }
        set { soP.speed = value; }
    }
    #endregion
}
