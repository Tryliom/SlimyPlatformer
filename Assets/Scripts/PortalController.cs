using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    [SerializeField] private Sprite _openPortalTexture;
    [SerializeField] private Sprite _closedPortalTexture;
    
    [SerializeField] private TextMeshPro _levelNameText;
    [SerializeField] private TextMeshPro _pressKeyText;
    [SerializeField] private TextMeshPro _lockedText;
    [SerializeField] private GameObject _lockedImage;

    [SerializeField] private int _coinsToOpen;
    [SerializeField] private string _sceneToLoad;
    [SerializeField] private string _levelName;
    
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private GameObject _player;
    
    private PlayerInputManager _playerInputManager;
    private SpriteRenderer _spriteRenderer;
    
    private bool _isTriggered;
    
    // Start is called before the first frame update
    void Start()
    {
        _pressKeyText.enabled = false;

        _levelNameText.text = _levelName;
        
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerInputManager = _player.GetComponent<PlayerInputManager>();
        
        if (_playerData.GetCoins() < _coinsToOpen)
        {
            Lock();
        }
        else
        {
            Unlock();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerInputManager.pressXValue && _isTriggered)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(_sceneToLoad);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (_playerData.GetCoins() >= _coinsToOpen)
            {
                _pressKeyText.enabled = true;
                
                Unlock();
                
                _isTriggered = true;
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            _pressKeyText.enabled = false;
            
            _isTriggered = false;
        }
    }

    private void Unlock()
    {
        _lockedText.enabled = false;
        _lockedImage.SetActive(false);
        _spriteRenderer.sprite = _openPortalTexture;
    }
    
    private void Lock()
    {
        _lockedText.enabled = true;
        _lockedImage.SetActive(true);
        _lockedText.text = $"x{_coinsToOpen}";
        _spriteRenderer.sprite = _closedPortalTexture;
    }
}
