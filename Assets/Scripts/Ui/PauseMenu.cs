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

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Hub")
        {
            _returnToHubButton.SetActive(false);
        }
        
        gameObject.SetActive(false);
    }

    public void OnReturnToHubButtonPressed()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Hub");
    }

    public void OnResumeButtonPressed()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
    
    public void OnReturnToMenuButtonPressed()
    {
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
