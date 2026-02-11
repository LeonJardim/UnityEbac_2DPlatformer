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

    [Header("Speed Setup")]
    public float speed = 30f;
    public float jumpForce = 55f;
    public float friction = 180f;

    [Header("Animation Setup")]
    [SerializeField] private float jumpScaleY = 1.8f;
    [SerializeField] private float jumpScaleX = 0.9f;
    [SerializeField] private float animationDuration = 0.15f;
    [SerializeField] private Ease ease = Ease.OutBack;
    private Vector3 initialScale;
    #endregion


    #region Enable/Disable Inputs
    private void Awake()
    {
        playerControls = new PlayerInputActions();
        rb = GetComponent<Rigidbody2D>();
        initialScale = rb.transform.localScale;
    }
    private void OnEnable()
    {
        move = playerControls.Player.Move; move.Enable();
        jump = playerControls.Player.Jump; jump.Enable();
    }
    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
    }
    #endregion


    private void Update()
    {
        HandleJump();
        HandleMovement();
    }
    

    private void HandleMovement() 
    {
        Vector2 inputDirection = move.ReadValue<Vector2>();
        float targetSpeed = inputDirection.x * speed;

        rb.linearVelocityX = Mathf.MoveTowards(rb.linearVelocityX, targetSpeed, friction * Time.deltaTime);
    }
    private void HandleJump() 
    {
        if (jump.triggered)
        {
            rb.linearVelocityY = jumpForce;

            rb.transform.localScale = initialScale;
            DOTween.Kill(rb.transform);
            rb.transform.DOScaleY(rb.transform.localScale.y * jumpScaleY, animationDuration).SetLoops(2, LoopType.Yoyo).SetEase(ease);
            rb.transform.DOScaleX(rb.transform.localScale.x * jumpScaleX, animationDuration).SetLoops(2, LoopType.Yoyo).SetEase(ease);
        }
        
    }

    
}
