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

    private bool _inWater = false;
    private bool _isGrounded = false;
    private bool _isInAir = false;
    private bool _isOnGlue = false;

    public bool IsInWater => _inWater;
    public bool IsGrounded => _isGrounded;
    public bool IsInAir => _isInAir;
    public bool IsOnGlue => _isOnGlue;
    
    private Animator _animator;
    
    private static readonly int Jumping = Animator.StringToHash("Jumping");

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((IsOnRightGlue() || IsOnLeftGlue()) && !_isOnGlue)
        {
            _isOnGlue = true;
            _animator.SetBool(Jumping, false);
        }
        else if (_isOnGlue && (!IsOnRightGlue() && !IsOnLeftGlue()))
        {
            _isOnGlue = false;
        }
        
        if (IsOnGround() && !_isGrounded && !_isOnGlue)
        {
            _inWater = false;
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
        if (col.gameObject.CompareTag("Water") && !_inWater)
        {
            _inWater = true;
            _isGrounded = false;
            _isInAir = false;
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