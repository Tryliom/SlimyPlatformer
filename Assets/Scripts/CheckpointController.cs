using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    [SerializeField] private Sprite _activatedSprite;
    
    private SpriteRenderer _spriteRenderer;
    
    private bool _activated = false;
    
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    public void Activate()
    {
        if (!_activated)
        {
            _activated = true;
            _spriteRenderer.sprite = _activatedSprite;
        }
    }
    
    public bool IsActivated()
    {
        return _activated;
    }
}
