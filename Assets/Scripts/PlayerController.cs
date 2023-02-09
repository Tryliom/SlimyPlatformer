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
    [SerializeField] private float _maxXSpeed = 10f;
    
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private GameObject _pauseMenuPanel;

    private bool _canJump = true;
    public bool isDead = false;
    
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private PlayerInputManager _playerInputManager;
    private PlayerColliderController _playerColliderController;
    
    private static readonly int Running = Animator.StringToHash("Running");
    private static readonly int Jumping = Animator.StringToHash("Jumping");

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _playerInputManager = GetComponent<PlayerInputManager>();
        _playerColliderController = GetComponent<PlayerColliderController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerInputManager.pressPauseValue)
        {
            _pauseMenuPanel.SetActive(true);
            _pauseMenuPanel.GetComponent<PauseMenu>().OnPauseButtonPressed();
            _playerInputManager.pressPauseValue = false;
        }
        
        if (isDead || IsGamePaused()) return;
        
        if (_playerColliderController.IsInWater)
        {
            MoveInWater();
        }
        else if (_playerColliderController.IsOnGlue)
        {
            MoveInGlue();
        }
        else
        {
            Move();
        }

        if (_playerInputManager.jumpValue)
        {
            Jump();
        }

        if (_playerColliderController.IsInWater || _playerColliderController.IsOnGlue && !_animator.GetBool(Jumping))
        {
            _rigidbody.gravityScale = 0f;
            
            _canJump = true;
        }
        else
        {
            _rigidbody.gravityScale = _rigidbody.velocity.y < 0f ? _fallGravity : _jumpGravity;
            _rigidbody.velocity = new Vector2(
                Mathf.Clamp(_rigidbody.velocity.x, -_maxXSpeed, _maxXSpeed), 
                Mathf.Clamp(_rigidbody.velocity.y, -_maxFallSpeed, float.MaxValue)
            );
        }

        if (_playerColliderController.IsGrounded && !_animator.GetBool(Jumping))
        {
            _canJump = true;
        }
    }
    
    public bool IsGamePaused()
    {
        return _pauseMenuPanel.activeSelf;
    }

    private void Move()
    {
        var moveVelocity = _playerInputManager.moveValue * _groundSpeed;

        _rigidbody.velocity = new Vector2(moveVelocity, _rigidbody.velocity.y);
    }
    
    private void MoveInGlue()
    {
        var moveVelocity = _playerInputManager.moveWaterValue.y * _groundSpeed;

        if (!_animator.GetBool(Jumping))
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, moveVelocity);
        }
    }
    
    private void MoveInWater()
    {
        var moveDirection = new Vector3(_playerInputManager.moveWaterValue.x, _playerInputManager.moveWaterValue.y, 0);
        var moveSpeed = moveDirection.magnitude;
        var moveDirectionNormalized = moveDirection.normalized;
        var moveVelocity = moveDirectionNormalized * moveSpeed * _groundSpeed;
        
        _rigidbody.velocity = new Vector2(moveVelocity.x, moveVelocity.z);

        _animator.SetBool(Running, false);
    }

    private void Jump()
    {
        if (_canJump)
        {
            _animator.SetBool(Jumping, true);
        }
        
        if (_canJump && _playerColliderController.IsOnGlue)
        {
            float xVelocity = 0f;

            if (_playerColliderController.IsOnRightGlue())
            {
                xVelocity = -_jumpForce / 2f;
            }
            else
            {
                xVelocity = _jumpForce / 2f;
            }

            _rigidbody.velocity = new Vector2(xVelocity, _jumpForce);
            _rigidbody.gravityScale = _jumpGravity;
        }
        else if (_canJump)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpForce);
            _rigidbody.gravityScale = _jumpGravity;
        }
        
        _canJump = false;
    }
}
