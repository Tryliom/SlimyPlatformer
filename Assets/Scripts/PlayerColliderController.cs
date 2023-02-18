using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColliderController : MonoBehaviour
{
    [SerializeField] private float _groundedDepth = 0.64f;
    [SerializeField] private float _groundedWidth = 0.1f;
    [SerializeField] private float _groundedHeight = 0.37f;
    
    [SerializeField] private float _rightGlueDepth = 0.54f;
    [SerializeField] private float _leftGlueDepth = 0.7f;
    
    [SerializeField] private PlayerData _playerData;
    
    [Header("Audio")] 
    [SerializeField] private GameObject _audioObject;

    private bool _isGrounded = false;
    private bool _isInAir = false;
    private bool _isOnGlue = false;
    
    public bool IsGrounded => _isGrounded;
    public bool IsInAir => _isInAir;
    public bool IsOnGlue => _isOnGlue;
    
    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private AudioController _audioController;

    private static readonly int Jumping = Animator.StringToHash("Jumping");
    private static readonly int Death = Animator.StringToHash("Death");

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _audioController = _audioObject.GetComponent<AudioController>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((IsOnRightGlue() || IsOnLeftGlue()) && !_isOnGlue)
        {
            _audioController.PlayGlueSfx();
            
            _isOnGlue = true;
            _animator.SetBool(Jumping, false);
        }
        else if (_isOnGlue && (!IsOnRightGlue() && !IsOnLeftGlue()))
        {
            _isOnGlue = false;
        }
        
        if (IsOnGround() && !_isGrounded && !_isOnGlue)
        {
            _audioController.ResetStarSfxIndex();
            
            _isGrounded = true;
            _isInAir = false;
            
            _animator.SetBool(Jumping, false);
        }
        else if (!IsOnGround() && _isGrounded && !_isOnGlue)
        {
            _isGrounded = false;
            _isInAir = true;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Death"))
        {
            OnDeath();
        }
        
        if (col.gameObject.CompareTag("ResetDash"))
        {
            GetComponent<PlayerController>().ResetDash(col.gameObject.GetComponent<ResetDash>().GetUniqueId());
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Death"))
        {
            OnDeath();
        }
    }

    private void OnDeath()
    {
        _audioController.PlayDeathSfx();
        
        _animator.SetTrigger(Death);
        GetComponent<PlayerController>().isDead = true;
        _rigidbody.velocity = Vector2.zero;
        _rigidbody.gravityScale = 0;
        transform.rotation = Quaternion.identity;
    }

    private bool IsOnGround()
    {
        var position = transform.position;

        return Physics2D.BoxCast(position, new Vector2(_groundedWidth, _groundedHeight), 0f, Vector2.down, _groundedDepth, LayerMask.GetMask("Ground")).collider != null;
    }

    public bool IsOnRightGlue()
    {
        var position = transform.position;
        
        return Physics2D.Raycast(position, Vector2.right, _rightGlueDepth, LayerMask.GetMask("Glue")).collider != null;
    }
    
    public bool IsOnLeftGlue()
    {
        var position = transform.position;
        
        return Physics2D.Raycast(position, Vector2.left, _leftGlueDepth, LayerMask.GetMask("Glue")).collider != null;
    }

    private void OnDrawGizmos()
    {
        var position = transform.position;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(position + Vector3.down * _groundedDepth, new Vector3(_groundedWidth, _groundedHeight, 0f));
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(position, Vector3.right * _rightGlueDepth);
        
        Gizmos.color = Color.green;
        Gizmos.DrawRay(position, Vector3.left * _leftGlueDepth);
    }
}
