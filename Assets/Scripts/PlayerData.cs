using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Player/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    private int _coins = 0;
    private List<string> _collectedCoins = new List<string>();
    private bool _attackUnlocked = false;
    private bool _dashUnlocked = false;

    public void AddCoin(Coin coin)
    {
        _coins++;
        _collectedCoins.Add(coin.GetPersistantId());
    }
    
    public void UnlockAttack()
    {
        _attackUnlocked = true;
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
    
    public bool IsAttackUnlocked()
    {
        return _attackUnlocked;
    }
    
    public bool IsDashUnlocked()
    {
        return _dashUnlocked;
    }

    public void EraseProgress()
    {
        _coins = 0;
        _collectedCoins = new List<string>();
        _attackUnlocked = false;
        _dashUnlocked = false;
    }
}
