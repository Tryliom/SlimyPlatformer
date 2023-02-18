using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private GameObject _returnToHubButton;
    [SerializeField] private GameObject _resumeButton;
    
    [Header("Audio")] 
    [SerializeField] private GameObject _audioObject;
    
    private AudioController _audioController;
    
    // Start is called before the first frame update
    void Start()
    {
        _audioController = _audioObject.GetComponent<AudioController>();
        
        if (SceneManager.GetActiveScene().name == "Hub")
        {
            _returnToHubButton.SetActive(false);
        }
        
        gameObject.SetActive(false);
    }

    public void OnReturnToHubButtonPressed()
    {
        _audioController.PlayButtonSfx();
        
        Time.timeScale = 1;
        SceneManager.LoadScene("Hub");
    }

    public void OnResumeButtonPressed()
    {
        _audioController.PlayButtonSfx();
        
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
    
    public void OnReturnToMenuButtonPressed()
    {
        _audioController.PlayButtonSfx();
        
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void Show()
    {
        gameObject.SetActive(true);
        _eventSystem.SetSelectedGameObject(_resumeButton);
        Time.timeScale = 0;
    }
}
