using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _maxVelocity = 10f;
    [SerializeField] private GameObject _sprite;

    private Camera _mainCamera;
    private Rigidbody2D _rigidBody;
    private BulletShooter _bulletShooter;

    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = Camera.main;
        _rigidBody = GetComponent<Rigidbody2D>();
        _bulletShooter = GetComponent<BulletShooter>();
    }

    private void Update()
    {
        MoveCharacter();
        FaceCharacterTowardsMouse();
        FireBullet();
    }

    private void MoveCharacter()
    {
        var playerInput = Vector2.ClampMagnitude(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")), 1f);
        _rigidBody.velocity = new Vector2(playerInput.x, playerInput.y) * _maxVelocity; ;
    }

    private void FaceCharacterTowardsMouse()
    {
        var mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        var relativePosition = mousePosition - transform.position;
        _sprite.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(relativePosition.y, relativePosition.x) * Mathf.Rad2Deg - 90);
    }

    private void FireBullet()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _bulletShooter.Fire(_sprite.transform.up, CollisionLayer.PlayerBullet);
        }
    }
}
