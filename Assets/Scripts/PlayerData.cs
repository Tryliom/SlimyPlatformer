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
    private int _deathCount = 0;

    public void AddCoin(Coin coin)
    {
        _coins++;
        _collectedCoins.Add(coin.GetPersistantId());
    }
    
    public void IncrementDeathCount()
    {
        _deathCount++;
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
    
    public int GetDeathCount()
    {
        return _deathCount;
    }
    
    public bool IsDashUnlocked()
    {
        return _dashUnlocked;
    }

    public void EraseProgress()
    {
        _coins = 0;
        _collectedCoins = new List<string>();
        _deathCount = 0;
        _dashUnlocked = false;
    }
}
