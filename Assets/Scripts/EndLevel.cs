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

    [SerializeField] private UnityEvent _onEndLevel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            StartCoroutine(OnEndLevel());
        }
    }
    
    private IEnumerator OnEndLevel()
    {
        _endLevelUI.SetActive(true);
        
        yield return new WaitForSeconds(2f);
        
        _onEndLevel.Invoke();
        
        yield return new WaitForSeconds(1f);
        
        SceneManager.LoadScene(_nextLevelName, LoadSceneMode.Single);
    }
}
