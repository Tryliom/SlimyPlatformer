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
    
    private static readonly int Jumping = Animator.StringToHash("Jumping");
    private static readonly int Running = Animator.StringToHash("Running");

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _playerInputManager = GetComponent<PlayerInputManager>();
        _playerColliderController = GetComponent<PlayerColliderController>();    
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerColliderController.IsInWater || _animator.GetBool(Jumping))
        {
            // Rotate him to face the velocity direction
            var velocity = _rigidbody.velocity;
            var angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg - 90f;
            
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else
        {
            _animator.SetBool(Running, true);

            if (_playerColliderController.IsOnGlue)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                
                if (_playerInputManager.moveWaterValue.y < - Mathf.Epsilon)
                {
                    _spriteRenderer.flipX = true;
                }
                else if (_playerInputManager.moveWaterValue.y > Mathf.Epsilon)
                {
                    _spriteRenderer.flipX = false;
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
            }
        }
    }
}
