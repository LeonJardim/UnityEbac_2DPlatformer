using DG.Tweening;
using Leon.Core.InputActions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    #region Variables
    [Header("References")]
    [SerializeField] private Transform sprite;
    private Rigidbody2D rb;
    private Animator animator;
    private PlayerInputActions playerControls;
    private InputAction move;
    private InputAction jump;
    private InputAction sprint;

    [Header("Speed Setup")]
    public float speed = 30f;
    public float sprintSpeed = 50f;
    public float jumpForce = 55f;
    public float friction = 180f;
    private bool _isSprinting = false;
    private bool _lastFloorCheck = false;

    [Header("Squat/Strech Setup")]
    [SerializeField] private float jumpScaleY = 1.6f;
    [SerializeField] private float jumpScaleX = 0.7f;
    [SerializeField] private float animationDuration = 0.15f;
    [SerializeField] private Ease ease = Ease.OutBack;
    private Vector3 _initialScale;

    [Header("Animations")]
    [SerializeField] private float turningDuration = 0.2f;
    [SerializeField] private string animRunBool = "Run";
    [SerializeField] private string animYSpeed = "YSpeed";
    [SerializeField] private string animOnFloorBool = "OnFloor";
    private bool _isTurning = false;

    [Header("Box Cast")]
    [SerializeField] private float castDistance;
    [SerializeField] private Vector2 castBoxSize;
    [SerializeField] private LayerMask groundLayer;
    #endregion


    #region Enable/Disable Inputs
    private void Awake()
    {
        playerControls = new PlayerInputActions();
        rb = GetComponent<Rigidbody2D>();
        animator = sprite.GetComponent<Animator>();
        _initialScale = sprite.localScale;
    }
    private void OnEnable()
    {
        move = playerControls.Player.Move; move.Enable();
        jump = playerControls.Player.Jump; jump.Enable();
        sprint = playerControls.Player.Sprint; sprint.Enable();
    }
    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
        sprint.Disable();
    }
    #endregion


    private void Update()
    {
        HandleJump();
        HandleMovement();
    }
    

    private void HandleMovement() 
    {    // Movement
        Vector2 inputDirection = move.ReadValue<Vector2>();
        float targetSpeed = inputDirection.x * GetSpeed;

        rb.linearVelocityX = Mathf.MoveTowards(rb.linearVelocityX, targetSpeed, friction * Time.deltaTime);

         // Set Animations
        if (Mathf.Abs(rb.linearVelocityX) > 0.1f)
        {
            animator.SetBool(animRunBool, true);
            animator.speed = _isSprinting ? 2f : 1f;
            FlipSprite();
        }
        else
        {
            animator.SetBool(animRunBool, false);
            animator.speed = 1f;
        }
        
    }
    private void HandleJump() 
    {
        bool isOnFloor = IsOnFloor();

        if (Mathf.Abs(rb.linearVelocityY) > 0) { animator.SetFloat(animYSpeed, rb.linearVelocityY > 0f ? 1f : -1f); }
        else { animator.SetFloat(animYSpeed, 0f); }

        if (isOnFloor && jump.triggered)
        {
            rb.linearVelocityY = jumpForce;
            JumpAnimation();
        }
        if (isOnFloor && !_lastFloorCheck)
        {
            JumpAnimation(true);
            _lastFloorCheck = true;
        }

        _lastFloorCheck = isOnFloor;
    }
    private void JumpAnimation(bool landing = false)
    {
        sprite.localScale = _initialScale;
        DOTween.Kill(sprite);
        float sX = jumpScaleX;
        float sY = jumpScaleY;
        float d = animationDuration;
        if (landing)
        {
            sX = 1.0f / sX;
            sY = 1.0f / sY;
            d *= 0.5f;
        }
        sprite.DOScaleY(sprite.localScale.y * sY, d).SetLoops(2, LoopType.Yoyo).SetEase(ease);
        sprite.DOScaleX(sprite.localScale.x * sX, d).SetLoops(2, LoopType.Yoyo).SetEase(ease);
    }
    private void FlipSprite()
    {
        if (transform.localScale.x * rb.linearVelocityX < 0 && !_isTurning)
        {
            _isTurning = true;
            transform.DOScaleX(-transform.localScale.x, turningDuration)
                .OnComplete(() => _isTurning = false);
        }
    }


    #region Variable Functions
    private bool IsOnFloor()
    {
        if (Physics2D.BoxCast(transform.position, castBoxSize, 0f, Vector2.down, castDistance, groundLayer))
            {  animator.SetBool(animOnFloorBool, true); return true;}
        else
            { animator.SetBool(animOnFloorBool, false); return false; }
    }
    //private void OnDrawGizmos() { Gizmos.DrawWireCube(transform.position + (Vector3.down * castDistance), castBoxSize); }
    public float GetSpeed
    {
        get
        {
            if (sprint.IsPressed())
            {
                _isSprinting = true;
                return sprintSpeed;
            }
            else
            {
                _isSprinting = false;
                return speed;
            }
        }
        set { speed = value; }
    }
    #endregion
}
