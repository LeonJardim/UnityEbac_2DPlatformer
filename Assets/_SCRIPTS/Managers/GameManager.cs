using UnityEngine;
using Leon.Core.Singleton;
using System.Collections.Generic;
using DG.Tweening;

public class GameManager : Singleton<GameManager>
{
    [Header("Player")]
    public GameObject playerPrefab;

    [Header("Enemy")]
    public List<GameObject> enemies;

    [Header("References")]
    [SerializeField] private Transform startPoint;
    [SerializeField] private GameObject actors;

    [Header("Animation")]
    [SerializeField] private float duration = 0.2f;
    [SerializeField] private float delay = 1f;
    [SerializeField] private Ease ease = Ease.OutBack;

    private GameObject _player;

    private void Start()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        _player = Instantiate(playerPrefab);
        _player.transform.position = startPoint.position;
        _player.transform.DOScale(0, duration).SetEase(ease).SetDelay(delay).From();
    }
}
