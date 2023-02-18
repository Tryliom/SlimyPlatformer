using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreditsController : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private TextMeshProUGUI _timeText;

    // Start is called before the first frame update
    void Start()
    {
        _timeText.text = $"Time: {_playerData.GetFormattedTime()}";
    }

    public void OnQuitButtonPressed()
    {
        // Quit game
        Application.Quit();
    }
}
