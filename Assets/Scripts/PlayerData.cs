using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Player/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    private int _coins = 0;
    private List<string> _collectedCoins = new List<string>();
    private bool _dashUnlocked = false;
    private float _timeSec = 0;

    public void AddCoin(Coin coin)
    {
        _coins++;
        _collectedCoins.Add(coin.GetPersistantId());
    }
    
    public void IncreaseTime(float timeSec)
    {
        _timeSec += timeSec;
    }
    
    public void UnlockDash()
    {
        _dashUnlocked = true;
    }
    
    public int GetCoins()
    {
        return _coins;
    }
    
    public bool HasCoin(Coin coin)
    {
        return _collectedCoins.Contains(coin.GetPersistantId());
    }
    
    public string GetFormattedTime()
    {
        var timeSpan = TimeSpan.FromSeconds(_timeSec);
        return $"{timeSpan.Minutes:00}:{timeSpan.Seconds:00}s";
    }
    
    public bool IsDashUnlocked()
    {
        return _dashUnlocked;
    }

    public void EraseProgress()
    {
        Debug.Log("Erase progress");
        
        _coins = 0;
        _collectedCoins = new List<string>();
        _timeSec = 0;
        _dashUnlocked = false;
    }
}
