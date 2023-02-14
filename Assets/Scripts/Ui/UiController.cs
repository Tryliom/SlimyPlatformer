using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiController : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private TextMeshProUGUI _coinsText;
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _levelNameText;

    // Update is called once per frame
    private void Update()
    {
        var sceneName = SceneManager.GetActiveScene().name;
        
        if (sceneName != "Hub" && sceneName != "Ending" && sceneName != "MainMenu" && sceneName != "TrueEnding")
        {
            _playerData.IncreaseTime(Time.deltaTime);
        }
        
        _coinsText.text = $"x{_playerData.GetCoins()}";
        _timeText.text = _playerData.GetFormattedTime();
        _levelNameText.text = sceneName;
    }
}
