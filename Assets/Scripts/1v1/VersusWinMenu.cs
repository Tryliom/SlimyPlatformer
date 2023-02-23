using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VersusWinMenu : MonoBehaviour
{
    [SerializeField] private Data1v1 _data1v1;
    [SerializeField] private TextMeshProUGUI _winText;
    
    // Start is called before the first frame update
    void Start()
    {
        if (_data1v1.GetPlayer1Lives() == 0)
        {
            _winText.text = "Player 2 Wins !";
        }
        else if (_data1v1.GetPlayer2Lives() == 0)
        {
            _winText.text = "Player 1 Wins !";
        }
    }

    public void OnBackButtonPressed()
    {
        // Switch to main menu
        SceneManager.LoadScene("VersusMenu");
    }
}
