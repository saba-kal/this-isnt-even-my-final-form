using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }


    [SerializeField] private float _maxVelocity = 10f;
    [SerializeField] private GameObject _rotationBody;

    private Camera _mainCamera;
    private Rigidbody2D _rigidbody;
    private ShootingManager _shootingManager;
    private DashAbility _dashAbility;
    private PowerLevelManager _powerLevelManager;
    private PlayerAbilityManager _playerAbilityManager;
    private CharacterHealth _characterHealth;
    private Vector2 _playerInput;
    private bool _disabled = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        _mainCamera = Camera.main;
        _rigidbody = GetComponent<Rigidbody2D>();
        _shootingManager = GetComponent<ShootingManager>();
        _dashAbility = GetComponent<DashAbility>();
        _powerLevelManager = GetComponent<PowerLevelManager>();
        _playerAbilityManager = GetComponent<PlayerAbilityManager>();
        _characterHealth = GetComponent<CharacterHealth>();
    }

    private void FixedUpdate()
    {
        if (_disabled)
        {
            return;
        }

        MoveCharacter();
    }

    private void Update()
    {
        if (_disabled)
        {
            return;
        }

        GetMovementInput();
        FaceCharacterTowardsMouse();
        FireBullets();
        InteractWithObjects();
        Dash();
    }

    public void SetMaxVelocity(float maxVelocity)
    {
        _maxVelocity = maxVelocity;
    }

    public void SetDisabled(bool disabled)
    {
        _disabled = disabled;
        _characterHealth.SetImmune(disabled);
        if (_disabled)
        {
            _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else
        {
            _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    public int GetPowerLevel()
    {
        return _powerLevelManager.GetPowerLevel();
    }

    private void MoveCharacter()
    {
        _rigidbody.velocity = _playerInput * _maxVelocity;
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
        if (_playerAbilityManager.PlayerAbilityIsUnlocked(PlayerAbilityType.NormalShot) &&
            Input.GetMouseButton(0))
        {
            _shootingManager.FireBulletShooters(CollisionLayer.PlayerBullet, BulletShooterType.Normal);
        }

        if (_playerAbilityManager.PlayerAbilityIsUnlocked(PlayerAbilityType.HeavyShot) &&
            Input.GetMouseButton(1))
        {
            _shootingManager.FireBulletShooters(CollisionLayer.PlayerBullet, BulletShooterType.Heavy);
        }
    }

    private void Dash()
    {
        if (_playerAbilityManager.PlayerAbilityIsUnlocked(PlayerAbilityType.Dash) &&
            Input.GetKey(KeyCode.Space))
        {
            _dashAbility.Dash();
        }
    }

    private void InteractWithObjects()
    {
        if (Input.GetKey(KeyCode.F))
        {
            var interactible = InteractableObject.GetNearestInteractable(transform.position);
            interactible?.Interact();
        }
    }
}
