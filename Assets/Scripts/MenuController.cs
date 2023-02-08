using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    
    [Header("Main menu panel")]
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _continuePanel;
    [SerializeField] private GameObject _newGamePanel;
    
    [Header("Game save panel")]
    [SerializeField] private TextMeshProUGUI _coinsText;
 
    private void Start()
    {
        _menuPanel.SetActive(true);
        _continuePanel.SetActive(false);
        _newGamePanel.SetActive(false);
    }
    
    public void OnPlayButtonPressed()
    {
        // Load menu to choose between continue saved game or start new game
        _menuPanel.SetActive(false);

        // Check if there is a saved game
        if (_playerData.GetCoins() > 0)
        {
            _continuePanel.SetActive(true);
            _newGamePanel.SetActive(false);
            
            // Show coins
            _coinsText.text = $"x{_playerData.GetCoins()}";
        }
        else
        {
            _continuePanel.SetActive(false);
            _newGamePanel.SetActive(true);
        }
    }
    
    public void OnContinueButtonPressed()
    {
        // Load saved game in to hub level
        SceneManager.LoadScene("Hub");
    }
    
    public void OnNewGameButtonPressed()
    {
        // Start new game
        _playerData.EraseProgress();
        
        // Load hub level
        SceneManager.LoadScene("Hub");
    }
    
    public void OnQuitButtonPressed()
    {
        // Quit game
        Application.Quit();
    }
}
