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
    
    [Header("Dash")]
    [SerializeField] private float _dashForce = 20f;
    [SerializeField] private float _dashTimer = 0.1f;
    
    [Header("Other")]
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private GameObject _pauseMenuPanel;

    [Header("Audio")] 
    [SerializeField] private GameObject _audioObject;

    private bool _canJump = true;
    public bool isDead;
    
    private bool _canDash = true;
    private bool _isDashing;
    private float _dashValue;
    
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private PlayerInputManager _playerInputManager;
    private PlayerColliderController _playerColliderController;
    private AudioController _audioController;
    
    private static readonly int Running = Animator.StringToHash("Running");
    private static readonly int Jumping = Animator.StringToHash("Jumping");
    private static readonly int Dashing = Animator.StringToHash("Dashing");

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerInputManager = GetComponent<PlayerInputManager>();
        _playerColliderController = GetComponent<PlayerColliderController>();
        _audioController = _audioObject.GetComponent<AudioController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsGamePaused() || isDead)
        {
            _playerInputManager.jumpValue = false;
            _playerInputManager.leftDashValue = false;
            _playerInputManager.rightDashValue = false;
            _playerInputManager.dashValue = false;
            _playerInputManager.pressPauseValue = false;
            
            return;
        }
        
        if (_playerInputManager.pressPauseValue)
        {
            _pauseMenuPanel.GetComponent<PauseMenu>().Show();
            _playerInputManager.pressPauseValue = false;
        }

        if (_playerColliderController.IsOnGlue)
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
        
        _playerInputManager.jumpValue = false;
        
        if ((_playerInputManager.leftDashValue || _playerInputManager.rightDashValue || _playerInputManager.dashValue) && _playerData.IsDashUnlocked())
        {
            Dash();
        }
        
        _playerInputManager.leftDashValue = false;
        _playerInputManager.rightDashValue = false;
        _playerInputManager.dashValue = false;

        if (_playerColliderController.IsOnGlue && !_animator.GetBool(Jumping) && !_animator.GetBool(Dashing))
        {
            _rigidbody.gravityScale = 0f;
            
            _canJump = true;
            _canDash = true;
        }
        else
        {
            _rigidbody.gravityScale = _rigidbody.velocity.y < 0f ? _fallGravity : _jumpGravity;
            _rigidbody.velocity = new Vector2(
                Mathf.Clamp(_rigidbody.velocity.x, -_maxXSpeed, _maxXSpeed), 
                Mathf.Clamp(_rigidbody.velocity.y, -_maxFallSpeed, float.MaxValue)
            );
        }

        if (_playerColliderController.IsGrounded && !_animator.GetBool(Jumping) && !_animator.GetBool(Dashing))
        {
            _canJump = true;
            _canDash = true;
        }

        if (_isDashing)
        {
            _rigidbody.velocity = new Vector2(_dashValue * _dashForce, 0);
        }
    }
    
    public bool IsGamePaused()
    {
        return _pauseMenuPanel.activeSelf;
    }

    private void Move()
    {
        var moveVelocity = _playerInputManager.moveValue.x * _groundSpeed;

        _rigidbody.velocity = new Vector2(moveVelocity, _rigidbody.velocity.y);
    }
    
    private void MoveInGlue()
    {
        var moveVelocity = _playerInputManager.moveValue.y * _groundSpeed;

        if (!_animator.GetBool(Jumping) && !_animator.GetBool(Dashing))
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, moveVelocity);
        }
    }

    private void Jump()
    {
        if (_canJump)
        {
            _animator.SetBool(Jumping, true);
            
            _audioController.PlayJumpSfx();
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
            
            // If the player is dashing, stop it
            if (_isDashing)
            {
                _isDashing = false;
                _animator.SetBool(Dashing, false);
            }
        }
        else if (_canJump)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpForce);
            _rigidbody.gravityScale = _jumpGravity;
            
            // If the player is dashing, stop it
            if (_isDashing)
            {
                _isDashing = false;
                _animator.SetBool(Dashing, false);
            }
        }
        
        _canJump = false;
    }
    
    private void Dash()
    {
        if (_canDash)
        {
            _audioController.PlayDashSfx();
            _isDashing = true;
            _canDash = false;
            _dashValue = _playerInputManager.leftDashValue ? -1f : 1f;
            
            if (_playerInputManager.dashValue)
            {
                if (_playerInputManager.moveValue.x > 0)
                {
                    _dashValue = 1f;
                }
                else if (_playerInputManager.moveValue.x < 0)
                {
                    _dashValue = -1f;
                }
                else
                {
                    _dashValue = _spriteRenderer.flipX ? -1f : 1f;
                }
            }
            else if (_playerInputManager.leftDashValue)
            {
                _spriteRenderer.flipX = true;
            }
            else
            {
                _spriteRenderer.flipX = false;
            }
            
            _animator.SetBool(Dashing, true);
            
            StartCoroutine(DashTimer());
        }
    }
    
    private IEnumerator DashTimer()
    {
        yield return new WaitForSeconds(_dashTimer);
        
        _isDashing = false;
        _animator.SetBool(Dashing, false);
    }
    
    public void ResetDash(string id)
    {
        _audioController.PlayStarSfx(id);
        
        _canDash = true;
        
        // Make the player jump
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpForce);
        
        // If the player is dashing, stop it
        if (_isDashing)
        {
            _isDashing = false;
            _animator.SetBool(Dashing, false);
        }
    }

    public bool CanJump()
    {
        return _canJump;
    }
    
    public bool CanDash()
    {
        return _canDash;
    }
    
    public void OnCollectCoin()
    {
        _audioController.PlayCoinSfx();
    }
    
    public void OnDoorUnlock()
    {
        _audioController.PlayDoorUnlockedSfx();
    }
}
