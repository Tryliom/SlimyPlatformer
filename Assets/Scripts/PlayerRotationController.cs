using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotationController : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private PlayerInputManager _playerInputManager;
    private SpriteRenderer _spriteRenderer;
    private PlayerColliderController _playerColliderController;
    private PlayerController _playerController;
    
    private static readonly int Jumping = Animator.StringToHash("Jumping");
    private static readonly int Running = Animator.StringToHash("Running");
    private static readonly int Death = Animator.StringToHash("Death");

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _playerInputManager = GetComponent<PlayerInputManager>();
        _playerColliderController = GetComponent<PlayerColliderController>();
        _playerController = GetComponent<PlayerController>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerController.isDead || _playerController.IsGamePaused()) return;
        
        if (_playerColliderController.IsInWater)
        {
            // Rotate him to face the velocity direction
            var velocity = _rigidbody.velocity;
            var angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg - 90f;
            
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            
            // Rotate him to face the direction he is moving
            if (_playerInputManager.moveValue > 0)
            {
                _spriteRenderer.flipX = false;
            }
            else if (_playerInputManager.moveValue < 0)
            {
                _spriteRenderer.flipX = true;
            }
        }
        else
        {
            _animator.SetBool(Running, true);

            if (_playerColliderController.IsOnGlue)
            {
                var rightGlue = _playerColliderController.IsOnRightGlue();
                var multiplier = rightGlue ? 1f : -1f;
                
                transform.rotation = Quaternion.Euler(0f, 0f, 90f * multiplier);
                
                if (_playerInputManager.moveWaterValue.y < 0)
                {
                    _spriteRenderer.flipX = rightGlue;
                }
                else if (_playerInputManager.moveWaterValue.y > 0)
                {
                    _spriteRenderer.flipX = !rightGlue;
                }
                else
                {
                    _animator.SetBool(Running, false);
                }
            }
            else
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);

                // Rotate him to face the direction he is moving
                if (_playerInputManager.moveValue > 0)
                {
                    _spriteRenderer.flipX = false;
                }
                else if (_playerInputManager.moveValue < 0)
                {
                    _spriteRenderer.flipX = true;
                }
                else
                {
                    _animator.SetBool(Running, false);
                }
            }
        }
    }
}
