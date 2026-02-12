using DG.Tweening;
using Leon.Core.InputActions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    #region Variables
    private Rigidbody2D rb;
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

    [Header("Animation Setup")]
    [SerializeField] private float jumpScaleY = 1.8f;
    [SerializeField] private float jumpScaleX = 0.9f;
    [SerializeField] private float animationDuration = 0.15f;
    [SerializeField] private Ease ease = Ease.OutBack;
    private Vector3 _initialScale;

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
        _initialScale = rb.transform.localScale;
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
    {
        _isSprinting = sprint.IsPressed();
        Vector2 inputDirection = move.ReadValue<Vector2>();
        float targetSpeed = inputDirection.x;
        targetSpeed *= _isSprinting ? sprintSpeed : speed;

        rb.linearVelocityX = Mathf.MoveTowards(rb.linearVelocityX, targetSpeed, friction * Time.deltaTime);
    }
    private void HandleJump() 
    {
        bool isOnFloor = IsOnFloor();

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
        rb.transform.localScale = _initialScale;
        DOTween.Kill(rb.transform);
        float sX = jumpScaleX;
        float sY = jumpScaleY;
        float d = animationDuration;
        if (landing)
        {
            sX = 1.0f / sX;
            sY = 1.0f / sY;
            d *= 0.5f;
        }
        rb.transform.DOScaleY(rb.transform.localScale.y * sY, d).SetLoops(2, LoopType.Yoyo).SetEase(ease);
        rb.transform.DOScaleX(rb.transform.localScale.x * sX, d).SetLoops(2, LoopType.Yoyo).SetEase(ease);
    }

    private bool IsOnFloor()
    {
        if (Physics2D.BoxCast(transform.position, castBoxSize, 0f, Vector2.down, castDistance, groundLayer))
            { return true; }
        else
            { return false; }
    }
    /*
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + Vector3.down * castDistance, castBoxSize);
    }
    */
}
