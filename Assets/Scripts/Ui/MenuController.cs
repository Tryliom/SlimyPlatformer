using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private EventSystem _eventSystem;
    
    [Header("Main menu panel")]
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _continuePanel;
    [SerializeField] private GameObject _newGamePanel;
    
    [Header("First button to select")]
    [SerializeField] private GameObject _menuButton;
    [SerializeField] private GameObject _continueButton;
    [SerializeField] private GameObject _newGameButton;
    
    [Header("Game save panel")]
    [SerializeField] private TextMeshProUGUI _coinsText;
 
    private void Start()
    {
        _menuPanel.SetActive(true);
        _continuePanel.SetActive(false);
        _newGamePanel.SetActive(false);
        
        // Select menu button
        _eventSystem.SetSelectedGameObject(_menuButton);
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
            
            // Select continue button
            _eventSystem.SetSelectedGameObject(_continueButton);
        }
        else
        {
            _continuePanel.SetActive(false);
            _newGamePanel.SetActive(true);
            
            // Select new game button
            _eventSystem.SetSelectedGameObject(_newGameButton);
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
