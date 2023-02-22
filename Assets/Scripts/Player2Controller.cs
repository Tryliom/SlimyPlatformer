using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class Player2Controller : MonoBehaviour
{
    [SerializeField] private GameObject _shootingPoint;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private float _bulletLifeTime;
    [SerializeField] private float _shootCooldown;
    
    [Header("Movement")]
    [SerializeField] private float _radius = 5f;
    [SerializeField] private float _speed = 60f;

    [Header("Sprite")]
    [SerializeField] private Sprite _canShootSprite;
    [SerializeField] private Sprite _cannotShootSprite;
    
    private SpriteRenderer _spriteRenderer;
    private PlayerInputManager _playerInputManager;
    
    private Camera _mainCamera;
    
    private float _shootCooldownTimer = 0f;
    // Move around the screen with the joystick and shoot with the button
    public float _anglePosition = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerInputManager = GetComponent<PlayerInputManager>();
        
        _mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (_shootCooldownTimer > 0)
        {
            _shootCooldownTimer -= Time.deltaTime;
        }
        
        if (_shootCooldownTimer <= 0)
        {
            _spriteRenderer.sprite = _canShootSprite;
        }
        
        if (_playerInputManager.jumpValue)
        {
            Shoot();
        }
        
        _playerInputManager.jumpValue = false;

        _anglePosition += -_speed * Time.deltaTime * _playerInputManager.moveValue.x;
        
        if (_anglePosition > 360f)
        {
            _anglePosition -= 360f;
        }
        else if (_anglePosition < 0f)
        {
            _anglePosition += 360f;
        }
        
        var radianAngle = Mathf.Deg2Rad * (_anglePosition);
        var direction = new Vector2(Mathf.Cos(radianAngle), Mathf.Sin(radianAngle));
        Vector2 centerScreen = _mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
        
        // The canon look to right side by default
        transform.position = centerScreen + direction * _radius;
        transform.rotation = Quaternion.Euler(0f, 0f, _anglePosition - 180f);
    }
    
    private void Shoot()
    {
        if (_shootCooldownTimer > 0)
        {
            return;
        }
        
        _shootCooldownTimer = _shootCooldown;
        _spriteRenderer.sprite = _cannotShootSprite;
        
        var bullet = Instantiate(_bulletPrefab, _shootingPoint.transform.position, Quaternion.identity);
        var radianAngle = Mathf.Deg2Rad * (_anglePosition - 180f);
        
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(_bulletSpeed * Mathf.Cos(radianAngle), _bulletSpeed * Mathf.Sin(radianAngle));
        Destroy(bullet, _bulletLifeTime);
    }
}
