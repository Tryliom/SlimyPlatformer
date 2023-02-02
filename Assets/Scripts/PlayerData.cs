using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Player/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    private int _coins = 0;
    private bool _attackUnlocked = false;
    
    public void AddCoins(int coins)
    {
        _coins += coins;
    }
    
    public void UnlockAttack()
    {
        _attackUnlocked = true;
    }
    
    public int GetCoins()
    {
        return _coins;
    }
    
    public bool IsAttackUnlocked()
    {
        return _attackUnlocked;
    }
}
