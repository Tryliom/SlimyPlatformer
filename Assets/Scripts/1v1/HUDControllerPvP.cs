using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDControllerPvP : MonoBehaviour
{
    [SerializeField] private Data1v1 _data1v1;
    [SerializeField] private List<Image> _player1Hearts;
    [SerializeField] private List<Image> _player2Hearts;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int iP1 = 0;
        
        foreach (var heart in _player1Hearts)
        {
            if (iP1 < _data1v1.GetPlayer1Lives())
            {
                heart.enabled = true;
            }
            else
            {
                heart.enabled = false;
            }
            
            iP1++;
        }
        
        int iP2 = 0;
        
        foreach (var heart in _player2Hearts)
        {
            if (iP2 < _data1v1.GetPlayer2Lives())
            {
                heart.enabled = true;
            }
            else
            {
                heart.enabled = false;
            }
            
            iP2++;
        }
    }
}
