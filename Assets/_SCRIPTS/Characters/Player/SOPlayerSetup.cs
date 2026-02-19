using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "SoPlayer_Setup", menuName = "Scriptable Objects/SoPlayer_Setup")]
public class SOPlayerSetup : ScriptableObject
{
    [Header("Speed Setup")]
    public float speed = 30f;
    public float sprintSpeed = 50f;
    public float jumpForce = 55f;
    public float friction = 180f;

    [Header("Shooting")]
    public float fireRate = 0.3f;

    [Header("Squat/Strech")]
    public float jumpScaleY = 1.6f;
    public float jumpScaleX = 0.7f;
    public float animationDuration = 0.15f;
    public Ease ease = Ease.OutBack;

    [Header("Animations")]
    public float turningDuration = 0.2f;
    public string animRunBool = "Run";
    public string animYSpeed = "YSpeed";
    public string animOnFloorBool = "OnFloor";
    public string animDeath = "Death";

    [Header("Physics Box Cast")]
    public float castDistance = 0.0f;
    public Vector2 castBoxSize = new(2.5f, 0.5f);
    public LayerMask groundLayer;
}
