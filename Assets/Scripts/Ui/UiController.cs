using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private TextMeshProUGUI _coinsText;
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _levelNameText;
    
    [Header("Player relative icons")]
    [SerializeField] private GameObject _jumpIcon;
    [SerializeField] private GameObject _dashIcon;
    [SerializeField] private GameObject _player;
    
    private PlayerController _playerController;
    private Image _jumpIconImage;
    private Image _dashIconImage;

    // Start is called before the first frame update
    void Start()
    {
        _playerController = _player.GetComponent<PlayerController>();
        _jumpIconImage = _jumpIcon.GetComponent<Image>();
        _dashIconImage = _dashIcon.GetComponent<Image>();

        if (!_playerData.IsDashUnlocked())
        {
            _dashIconImage.color = Color.clear;
        }
        
        if (!_playerData.IsJumpUiUnlocked())
        {
            _jumpIconImage.color = Color.clear;
        }
    }
    
    // Update is called once per frame
    private void Update()
    {
        var sceneName = SceneManager.GetActiveScene().name;
        
        if (sceneName != "Hub" && sceneName != "Ending" && sceneName != "True ending")
        {
            _playerData.IncreaseTime(Time.deltaTime);
        }
        
        _coinsText.text = $"x{_playerData.GetCoins()}";
        _timeText.text = _playerData.GetFormattedTime();
        _levelNameText.text = sceneName;

        if (_playerData.IsDashUnlocked())
        {
            _dashIconImage.color = _playerController.CanDash() ? Color.white : Color.gray;
        }
        
        if (_playerData.IsJumpUiUnlocked())
        {
            _jumpIconImage.color = _playerController.CanJump() ? Color.white : Color.gray;
        }
    }
}
