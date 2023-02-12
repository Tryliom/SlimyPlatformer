using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiController : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private TextMeshProUGUI _coinsText;
    [SerializeField] private TextMeshProUGUI _deathCountText;

    // Update is called once per frame
    private void Update()
    {
        _coinsText.text = $"x{_playerData.GetCoins()}";
        _deathCountText.text = $"x{_playerData.GetDeathCount()}";
    }
}
