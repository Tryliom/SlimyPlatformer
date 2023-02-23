using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VersusController : MonoBehaviour
{
    [SerializeField] private Data1v1 _data1v1;

    private int _p1Lives;
    private int _p2Lives;
    private readonly int _arenaNumber = 5;
    
    // Start is called before the first frame update
    void Start()
    {
        _p1Lives = _data1v1.GetPlayer1Lives();
        _p2Lives = _data1v1.GetPlayer2Lives();
    }

    // Update is called once per frame
    void Update()
    {
        if (_p1Lives != _data1v1.GetPlayer1Lives() || _p2Lives != _data1v1.GetPlayer2Lives())
        {
            _p1Lives = _data1v1.GetPlayer1Lives();
            _p2Lives = _data1v1.GetPlayer2Lives();
            
            if (_p1Lives == 0 || _p2Lives == 0)
            {
                SceneManager.LoadScene("VersusWinMenu");
                
                return;
            }
            
            // Switch scene
            int currentSceneIndex = SceneManager.GetActiveScene().name[5] - '0';
            int nextSceneIndex = Random.Range(0, _arenaNumber);
            
            while (nextSceneIndex == currentSceneIndex)
            {
                nextSceneIndex = Random.Range(0, _arenaNumber);
            }
            
            SceneManager.LoadScene($"Arena{nextSceneIndex}");
        }
    }
}
