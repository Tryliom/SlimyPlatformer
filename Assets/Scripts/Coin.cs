using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    
    private bool _triggered = false;
    
    private CoinsManager _coinsManager;
    
    private static readonly int PickedUp = Animator.StringToHash("PickedUp");

    public void Initialize()
    {
        // Get parent CoinsManager
        _coinsManager = transform.parent.GetComponent<CoinsManager>();
        
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
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
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
    
    public string GetPersistantId()
    {
        var id = $"Coin_{transform.position}";

        return id;
    }
}
