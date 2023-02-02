using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiController : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private TextMeshProUGUI _coinsText;

    // Update is called once per frame
    private void Update()
    {
        _coinsText.text = _playerData.GetCoins().ToString();
    }
}
