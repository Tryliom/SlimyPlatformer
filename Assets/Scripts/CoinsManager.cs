using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsManager : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    
    private List<Coin> _coins = new List<Coin>();
    
    // Start is called before the first frame update
    void Start()
    {
        _coins = new List<Coin>(GetComponentsInChildren<Coin>());
        
        foreach (var coin in _coins)
        {
            coin.Initialize();
            
            if (_playerData.HasCoin(coin))
            {
                coin.SetCollected();
            }
        }
    }

    public void Collect(Coin coin)
    {
        if (!_playerData.HasCoin(coin))
        {
            _playerData.AddCoin(coin);
        }
    }
}
