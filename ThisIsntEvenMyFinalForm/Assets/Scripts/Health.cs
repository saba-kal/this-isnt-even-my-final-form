using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int _maxHealth;

    private int _currentHealth;
    private bool _isPlayer;

    void Start()
    {
        _currentHealth = _maxHealth;
        _isPlayer = gameObject.layer == (int)CollisionLayer.Player;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var gameObjectLayer = collision.gameObject.layer;
        var opposingLayer = _isPlayer ? CollisionLayer.EnemyBullet : CollisionLayer.PlayerBullet;

        if (gameObjectLayer == (int)opposingLayer)
        {
            _currentHealth -= 1;
            Destroy(collision.gameObject);
        }

        if (_currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
