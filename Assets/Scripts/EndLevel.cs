using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{
    [SerializeField] private GameObject _endLevelUI;
    [SerializeField] private string _nextLevelName;
    
    private bool _alreadyTriggered;

    private void Start()
    {
        if (_endLevelUI != null)
        {
            _endLevelUI.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && !_alreadyTriggered)
        {
            _alreadyTriggered = true;
            StartCoroutine(OnEndLevel());
        }
    }
    
    private IEnumerator OnEndLevel()
    {
        if (_endLevelUI != null)
        {
            _endLevelUI.SetActive(true);
        }

        yield return new WaitForSeconds(1f);
        
        if (_nextLevelName != "")
        {
            SceneManager.LoadScene(_nextLevelName, LoadSceneMode.Single);
        }
    }
}
