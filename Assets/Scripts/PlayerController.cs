using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _groundSpeed = 5f;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _jumpGravity = 1f;
    [SerializeField] private float _fallGravity = 2f;
    [SerializeField] private float _maxFallSpeed = 20f;

    private bool _inWater = false;
    private bool _isGrounded = false;
    private bool _isInAir = false;
    private bool _canJump = true;
    private bool _canJumpMidAir = true;
    
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private PlayerInputManager _playerInputManager;
    private SpriteRenderer _spriteRenderer;
    
    private static readonly int Running = Animator.StringToHash("Running");
    private static readonly int Jumping = Animator.StringToHash("Jumping");

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _playerInputManager = GetComponent<PlayerInputManager>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_inWater)
        {
            moveInWater();
        }
        else
        {
            move();
        }
        
        if (_playerInputManager.jumpValue)
        {
            jump();
        }
        
        _playerInputManager.jumpValue = false;
        
        if (_inWater)
        {
            _rigidbody.gravityScale = 0f;
            
            _canJump = true;
            _canJumpMidAir = true;
        }
        else
        {
            _rigidbody.gravityScale = _rigidbody.velocity.y < 0f ? _fallGravity : _jumpGravity;
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, Mathf.Clamp(_rigidbody.velocity.y, -_maxFallSpeed, float.MaxValue));
        }

        if (_isGrounded)
        {
            _canJump = true;
            _canJumpMidAir = true;
        }

        if (_inWater || _animator.GetBool(Jumping))
        {
            // Rotate him to face the velocity direction
            var velocity = _rigidbody.velocity;
            var angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg - 90f;
            
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else
        {
            _animator.SetBool(Running, true);
            
            // Rotate him to face the direction he is moving
            if (_rigidbody.velocity.x > Mathf.Epsilon)
            {
                _spriteRenderer.flipX = false;
            }
            else if (_rigidbody.velocity.x < - Mathf.Epsilon)
            {
                _spriteRenderer.flipX = true;
            }
            else
            {
                _animator.SetBool(Running, false);
            }
            
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            _inWater = false;
            _isGrounded = true;
            _isInAir = false;
            
            _animator.SetBool(Jumping, false);
        }
    }
    
    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            _isGrounded = false;
            _isInAir = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Water"))
        {
            _inWater = true;
            _isGrounded = false;
            _isInAir = false;
        }
        else if (col.gameObject.CompareTag("Glue"))
        {
            //TODO: Make him stick to the glue, allow him to move only up and down
        }
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Water"))
        {
            _inWater = false;
            _isInAir = true;
        }
    }

    private void move()
    {
        var moveVelocity = _playerInputManager.moveValue * _groundSpeed;
        
        _rigidbody.velocity = new Vector2(moveVelocity, _rigidbody.velocity.y);
    }
    
    private void moveInWater()
    {
        var moveDirection = new Vector3(_playerInputManager.moveWaterValue.x, 0f, _playerInputManager.moveWaterValue.y);
        var moveSpeed = moveDirection.magnitude;
        var moveDirectionNormalized = moveDirection.normalized;
        var moveVelocity = moveDirectionNormalized * moveSpeed * _groundSpeed;
        
        _rigidbody.velocity = new Vector2(moveVelocity.x, moveVelocity.z);
        
        if (moveVelocity.x > 0f)
        {
            transform.rotation = Quaternion.Euler(0f, 0, 0f);
        }
        else if (moveVelocity.x < 0f)
        {
            transform.rotation = Quaternion.Euler(0f, -180, 0f);
        }
        
        _animator.SetBool(Running, false);
    }
    
    private void jump()
    {
        if (_canJump && _isGrounded || _inWater)
        {
            _canJump = false;
            _canJumpMidAir = true;
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpForce);
            _rigidbody.gravityScale = _jumpGravity;
            
            _animator.SetBool(Jumping, true);
        }
        else if (_canJumpMidAir && _isInAir)
        {
            _canJump = true;
            _canJumpMidAir = false;
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpForce);
            _rigidbody.gravityScale = _jumpGravity;
            
            _animator.SetBool(Jumping, true);
        }
    }
}
