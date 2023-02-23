using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitController : MonoBehaviour
{
    private static readonly int Impact = Animator.StringToHash("Impact");
    
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private CircleCollider2D _circleCollider2D;
    
    private Vector2 _direction;
    private Color _color;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _circleCollider2D = GetComponent<CircleCollider2D>();
        
        Destroy(gameObject, 5f);
        
        _rigidbody.velocity = _direction;
        _spriteRenderer.color = _color;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Ground") 
            || other.gameObject.CompareTag("Platform")  || other.gameObject.CompareTag("ResetDash") || other.gameObject.CompareTag("Death") || other.gameObject.CompareTag("Spit"))
        {
            _animator.SetTrigger(Impact);
            transform.localScale = new Vector3(3f, 3f, 3f);
            _rigidbody.velocity = Vector2.zero;
        }
    }
    
    public void DestroySpit()
    {
        Destroy(gameObject);
    }
    
    public void SetDirection(Vector2 velocity)
    {
        _direction = velocity;
        
        // Change the direction of the spit
        var angleRad = Mathf.Atan2(velocity.y, velocity.x);
        var angleDeg = (180 / Mathf.PI) * angleRad;
        
        transform.rotation = Quaternion.Euler(0, 0, angleDeg);
    }
    
    public void SetColor(Color color)
    {
        _color = color;
    }
}
