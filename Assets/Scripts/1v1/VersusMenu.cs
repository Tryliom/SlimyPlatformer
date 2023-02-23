using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VersusMenu : MonoBehaviour
{
    [SerializeField] private Data1v1 _data1v1;
    
    private readonly int _arenaNumber = 5;

    public void OnStartButtonPressed()
    {
        _data1v1.Reset();
        
        // Switch scene
        SceneManager.LoadScene($"Arena{Random.Range(0, _arenaNumber)}");
    }
    
    public void OnBackButtonPressed()
    {
        // Switch to main menu
        SceneManager.LoadScene("MainMenu");
    }
}
