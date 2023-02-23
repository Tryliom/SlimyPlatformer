using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data1v1", menuName = "Data/1v1", order = 1)]
public class Data1v1 : ScriptableObject
{
    private int _player1Lives = 5;
    private int _player2Lives = 5;
    
    public void DecreasePlayer1Lives()
    {
        _player1Lives--;
    }
    
    public void DecreasePlayer2Lives()
    {
        _player2Lives--;
    }
    
    public int GetPlayer1Lives()
    {
        return _player1Lives;
    }
    
    public int GetPlayer2Lives()
    {
        return _player2Lives;
    }
    
    public void Reset()
    {
        _player1Lives = 5;
        _player2Lives = 5;
    }
}
