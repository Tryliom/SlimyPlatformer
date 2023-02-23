using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class PlayerControllerPvP : MonoBehaviour
{
    [SerializeField] private float _groundSpeed = 5f;
    [SerializeField] private float _jumpForce = 12f;
    [SerializeField] private float _jumpGravity = 2.2f;
    [SerializeField] private float _fallGravity = 4f;
    [SerializeField] private float _maxFallSpeed = 15f;
    [SerializeField] private float _maxXSpeed = 10f;
    
    [Header("Dash")]
    [SerializeField] private float _dashForce = 18f;
    [SerializeField] private float _dashTimer = 0.3f;
    
    [Header("Attack")]
    [SerializeField] private float _attackCooldown = 0.3f;
    [SerializeField] private float _attackForce = 10f;
    [SerializeField] private GameObject _spitPrefab;

    [Header("Audio")] 
    [SerializeField] private GameObject _audioObject;

    [Header("Other")] 
    [SerializeField] private Color _playerColor;
    [SerializeField] private Data1v1 _data1v1;
    [SerializeField] private bool _isPlayer1;

    private bool _canJump = true;
    public bool isDead;
    
    private bool _canDash = true;
    private bool _isDashing;
    private float _dashValue;
    
    private bool _canAttack = true;
    private bool _isAttacking;

    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private PlayerInputManager _playerInputManager;
    private PlayerColliderController _playerColliderController;
    private AudioController _audioController;
    
    private static readonly int Running = Animator.StringToHash("Running");
    private static readonly int Jumping = Animator.StringToHash("Jumping");
    private static readonly int Dashing = Animator.StringToHash("Dashing");
    private static readonly int Attacking = Animator.StringToHash("Attacking");
    private static readonly int EndAttacking = Animator.StringToHash("EndAttacking");
    
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
        if (isDead)
        {
            _playerInputManager.jumpValue = false;
            _playerInputManager.leftDashValue = false;
            _playerInputManager.rightDashValue = false;
            _playerInputManager.dashValue = false;
            _playerInputManager.pressXValue = false;
            
            return;
        }

        if (_playerColliderController.IsOnGlue)
        {
            MoveInGlue();
        }
        else
        {
            Move();
        }

        if (_playerInputManager.pressXValue && _canAttack)
        {
            Attack();
        }
        
        if (_playerInputManager.jumpValue)
        {
            Jump();
        }

        if (_playerInputManager.leftDashValue || _playerInputManager.rightDashValue || _playerInputManager.dashValue)
        {
            Dash();
        }

        _playerInputManager.leftDashValue = false;
        _playerInputManager.rightDashValue = false;
        _playerInputManager.dashValue = false;
        _playerInputManager.jumpValue = false;
        _playerInputManager.pressXValue = false;

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
    
    private void Attack()
    {
        _animator.SetTrigger(Attacking);
        _canAttack = false;
        _isAttacking = true;
        
        _rigidbody.gravityScale = 0f;
        
        // If the player is dashing, stop it
        if (_isDashing)
        {
            _isDashing = false;
            _animator.SetBool(Dashing, false);
        }
        
        // If the player is jumping, stop it
        if (_animator.GetBool(Jumping))
        {
            _animator.SetBool(Jumping, false);
        }
        
        StartCoroutine(AttackTimer());
    }
    
    /**
     * Called by the animation event
     */
    private void LaunchAttack()
    {
        // Calculate the direction of the attack based on the player's orientation and if the player is on glue
        Vector3 direction;
        
        if (_playerColliderController.IsOnGlue)
        {
            if (_playerColliderController.IsOnRightGlue())
            {
                direction = _spriteRenderer.flipX ? Vector2.down : Vector2.up;
            }
            else
            {
                direction = _spriteRenderer.flipX ? Vector2.up : Vector2.down;
            }
        }
        else
        {
            direction = _spriteRenderer.flipX ? Vector2.left : Vector2.right;
        }
        
        // Create the projectile
        var projectile = Instantiate(_spitPrefab, transform.position + direction, Quaternion.identity);
        
        // Set the projectile's direction
        projectile.GetComponent<SpitController>().SetDirection(direction * _attackForce);

        // Color the projectile based on the player's color
        projectile.GetComponent<SpitController>().SetColor(_playerColor);
    }
    
    /**
     * Called at the end of the attack animation
     */
    private void OnAttackEnd()
    {
        _animator.SetTrigger(EndAttacking);
        _isAttacking = false;
        _canAttack = true;
        
        _rigidbody.gravityScale = _fallGravity;
    }
    
    private IEnumerator AttackTimer()
    {
        yield return new WaitForSeconds(_attackCooldown);
        
        _canAttack = true;

        //TODO: Play a sound when complete
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

    public void OnDeath(bool pvpKill)
    {
        isDead = true;
        
        // Cancel dash, jump and attack
        if (_isDashing)
        {
            _isDashing = false;
            _animator.SetBool(Dashing, false);
        }
        
        if (_animator.GetBool(Jumping))
        {
            _animator.SetBool(Jumping, false);
        }
        
        if (_isAttacking)
        {
            _isAttacking = false;
            _animator.SetTrigger(EndAttacking);
        }
        
        _canJump = true;
        _canDash = true;
        _canAttack = true;
        
        // Stop coroutines
        StopAllCoroutines();

        if (pvpKill)
        {
            if (_isPlayer1)
            {
                _data1v1.DecreasePlayer1Lives();
            }
            else
            {
                _data1v1.DecreasePlayer2Lives();
            }
        }
    }
    
    public bool IsDashing()
    {
        return _isDashing;
    }
}
