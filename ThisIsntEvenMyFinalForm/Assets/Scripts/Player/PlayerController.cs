using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _maxVelocity = 10f;
    [SerializeField] private GameObject _rotationBody;

    private Camera _mainCamera;
    private Rigidbody2D _rigidBody;
    private ShootingManager _shootingManager;
    private PowerLevelManager _powerLevelManager;
    private Vector2 _playerInput;

    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = Camera.main;
        _rigidBody = GetComponent<Rigidbody2D>();
        _shootingManager = GetComponent<ShootingManager>();
        _powerLevelManager = GetComponent<PowerLevelManager>();
    }

    private void FixedUpdate()
    {
        MoveCharacter();
    }

    private void Update()
    {
        GetMovementInput();
        FaceCharacterTowardsMouse();
        FireBullets();
        PowerUp();
    }

    private void MoveCharacter()
    {
        _rigidBody.velocity = _playerInput * _maxVelocity;
    }

    private void GetMovementInput()
    {
        _playerInput = Vector2.ClampMagnitude(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")), 1f);
    }

    private void FaceCharacterTowardsMouse()
    {
        var mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        var relativePosition = mousePosition - transform.position;
        _rotationBody.transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(relativePosition.y, relativePosition.x) * Mathf.Rad2Deg, Vector3.forward);
    }

    private void FireBullets()
    {
        if (Input.GetMouseButton(0))
        {
            _shootingManager.FireBulletShooters(CollisionLayer.PlayerBullet);
        }
    }

    private void PowerUp()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _powerLevelManager.PowerUp();
        }
    }
}