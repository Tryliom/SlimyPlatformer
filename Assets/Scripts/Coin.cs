using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private CircleCollider2D _circleCollider2D;
    
    private bool _triggered = false;
    
    private CoinsManager _coinsManager;
    
    private static readonly int PickedUp = Animator.StringToHash("PickedUp");
    private static readonly int DisabledAnimation = Animator.StringToHash("DisabledAnimation");

    public void Initialize()
    {
        // Get parent CoinsManager
        _coinsManager = transform.parent.GetComponent<CoinsManager>();
        
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _circleCollider2D = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !_triggered)
        {
            _coinsManager.Collect(this);
            _animator.SetTrigger(PickedUp);
            _triggered = true;
        }
    }
    
    public void SetCollected()
    {
        // Change sprite
        _spriteRenderer.color = Color.gray;
        
        _animator.SetTrigger(DisabledAnimation);
        
        // Disable collider
        _circleCollider2D.enabled = false;
    }

    public string GetPersistantId()
    {
        var id = $"Coin_{transform.position}";

        return id;
    }
}
